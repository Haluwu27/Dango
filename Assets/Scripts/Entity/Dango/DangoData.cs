using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Renderer Rend => _rend;
    public Rigidbody Rb => _rigidbody;
    public Animator Animator => _animator;

    //オブジェクトプールマネージャー
    DangoPoolManager _poolManager;

    //救済団子が所属しているフロア
    FloorArray _salvationFloor;
    List<FloorArray> _canShotList = new();

    static bool[] completedInitialization = new bool[5];

    private void Awake()
    {
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
    }

    public void OnEnable()
    {
        //移動可能にする
        _isMoveable = true;
        gameObject.SetLayerIncludeChildren(0);
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
            _rigidbody.AddForce(transform.forward * currentSpeed);
        }

        //ここで回転処理でも作る
        if (_rigidbody.velocity.magnitude < 0.01f) transform.Rotate(0, Random.Range(90f, 270f), 0);
    }

    public void ReleaseDangoPool(int stabCount)
    {
        //部屋の団子総数をへらす
        _floorManager.FloorArrays[(int)_floor].RemoveDangoCount(1, _color);

        //この団子が救済団子であれば
        if (_salvationFloor != null)
        {
            //部屋の登録情報を消す
            _salvationFloor.SetSalvationDango(null);

            //この団子の登録情報を消す
            _salvationFloor = null;
        }

        if (completedInitialization[stabCount - 3])
        {
            //救済処理
            Salvation(stabCount, _color);
        }
        else
        {
            for (DangoColor i = DangoColor.None + 1; i < DangoColor.Other - 1; i++)
            {
                Salvation(stabCount, i);
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
        Logger.Log("救済発動" + color + "\n" + "フロア：" + _canShotList[rand].FloorDatas[0].name);
    }

    public DangoColor GetDangoColor() => _color;

    public void SetDangoColor(DangoColor type)
    {
        _color = type;
    }

    public FloorManager.Floor Floor => _floor;
    public void SetFloor(FloorManager.Floor floor) => _floor = floor;
    public void SetFloorManager(FloorManager floorManager) => _floorManager = floorManager;
    public void SetIsMoveable(bool enable) => _isMoveable = enable;
}
