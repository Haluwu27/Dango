using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Easing.Management;

/// <summary>
/// 団子に関するマネージャークラス
/// </summary>
public class DangoData : MonoBehaviour
{
    bool _isMoveable = true;

    //団子が持つ色データ
    DangoColor _color = DangoColor.None;

    FloorManager.Floor _floor;
    FloorManager _floorManager;

    [SerializeField] Renderer _rend;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Animator _animator;
    [SerializeField] Collider _collider;

    public Renderer Rend => _rend;
    public Rigidbody Rb => _rigidbody;
    public Animator Animator => _animator;

    //オブジェクトプールマネージャー
    DangoPoolManager _poolManager;

    //救済団子が所属しているフロア
    FloorArray _salvationFloor;
    List<FloorArray> _canShotList = new();

    StageData _stageData;
    const float SCALE_ANIM_TIME = 0.2f;

    static bool[] completedInitialization = new bool[5];

    Transform _parent;
    Transform _childTrans;

    private void Awake()
    {
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();

        _childTrans = transform.GetChild(0).GetChild(0);
    }

    public void OnEnable()
    {
        //移動可能にする
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        _isMoveable = true;
        _animator.speed = 1f;
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;

        if (_parent != null) transform.parent = _parent;

        gameObject.SetLayerIncludeChildren(0);

        Logger.Assert(gameObject.layer == 0);
    }

    private void FixedUpdate()
    {
        MoveAndRotation();
    }

    private void MoveAndRotation()
    {
        if (!_isMoveable) return;

        if (_rigidbody.velocity.magnitude < 10)
        {
            //指定したスピードから現在の速度を引いて加速力を求める
            float currentSpeed = 10 - _rigidbody.velocity.magnitude;
            //調整された加速力で力を加える
            _rigidbody.AddForce(transform.forward.normalized * currentSpeed, ForceMode.Acceleration);
        }

        //ここで回転処理でも作る
        if (_rigidbody.velocity.magnitude < 0.01f) transform.Rotate(0, Random.Range(90f, 270f), 0);
    }

    private void ReleaseDangoPool(int stabCount)
    {
        //この団子が救済団子であれば
        if (_salvationFloor != null)
        {
            //部屋の登録情報を消す
            _salvationFloor.SetSalvationDango(null);

            //この団子の登録情報を消す
            _salvationFloor = null;
        }
        else
        {
            //部屋の団子総数をへらす
            _floorManager.FloorArrays[(int)_floor].RemoveDangoCount(1, _color);
        }

        if (completedInitialization[stabCount - 3])
        {
            //救済処理
            Salvation(stabCount, _color);
        }
        else
        {
            foreach (var color in _stageData.FloorDangoColors())
            {
                Salvation(stabCount, color);
            }

            completedInitialization[stabCount - 3] = true;
        }
        //プールに返却する
        _poolManager.DangoPool[(int)_color - 1].Release(this);
    }

    private void Salvation(int stabCount, DangoColor color)
    {
        //侵入可能フロアのテーブルから、そのテーブルにある今回消えた色の個数を格納
        for (int i = 0; i <= stabCount - 3; i++)
        {
            foreach (var d5 in _floorManager.IntrudableTable[stabCount - 3 - i])
            {
                //ひとつでも存在していたら以降は行わない
                if (d5.DangoCounts[(int)color - 1] > 0) return;
            }
        }

        //救済ショットができるリストの追加
        _canShotList.Clear();

        foreach (var list in _floorManager.SalvationTable[stabCount - 3])
        {
            if (list.AlreadyExistSavlationDango()) continue;

            _canShotList.Add(list);
        }

#if UNITY_EDITOR
        //この処理はデバッグ用なためビルドには通しません
        //もし発射可能なフロアが存在しなかったら以降は行わない
        if (_canShotList.Count == 0)
        {
            Logger.Warn("救済発射可のうエリアがありません。設定を見直してください。" + "\n" + "現在のD5：" + stabCount);
            return;
        }
#endif

        //発射可能なランダムなフロアを選択し、救済ショット
        int rand = Random.Range(0, _canShotList.Count);
        var salvationDango = _canShotList[rand].DangoInjections[0].EnforcementShot(color);
        salvationDango._salvationFloor = _canShotList[rand];
        _canShotList[rand].SetSalvationDango(salvationDango);
        //Logger.Log("救済発動" + color + "\n" + "フロア：" + _canShotList[rand].FloorDatas[0].name);
    }

    public async void StabAnimation(Animator playerAnimator, int stabCount, Transform parent)
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;

        try
        {
            //playerのアニメーション終了まで待機
            while (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                //Logger.Assert(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN5") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN7A"));
                await UniTask.Yield();
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }

        //剣に追従させる
        transform.localPosition = Vector3.zero;
        _childTrans.localPosition = Vector3.zero;

        _parent = transform.parent;

        //親を変更
        transform.parent = parent;

        Vector3 scale = Vector3.one;
        float value = 1f;
        float currentTime = 0f;

        try
        {
            while (value > 0.01f)
            {
                await UniTask.Yield();

                currentTime += Time.deltaTime;
                value = 1 - EasingManager.EaseProgress(TM.Easing.EaseType.OutCirc, currentTime, SCALE_ANIM_TIME, 0, 0);
                scale.Set(value, value, value);
                
                transform.localPosition = Vector3.zero;
                _childTrans.localPosition = Vector3.zero;
                transform.localScale = scale;
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }

        ReleaseDangoPool(stabCount);
    }

    public DangoColor GetDangoColor() => _color;

    public void SetDangoColor(DangoColor type)
    {
        _color = type;
    }

    public FloorManager.Floor Floor => _floor;
    public void SetFloor(FloorManager.Floor floor) => _floor = floor;
    public void SetFloorManager(FloorManager floorManager)
    {
        _floorManager = floorManager;
        _stageData = _floorManager.StageData;
    }
    public void SetIsMoveable(bool enable) => _isMoveable = enable;
}