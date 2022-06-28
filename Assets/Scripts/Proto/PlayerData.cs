using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(RideMoveObj))]
public class PlayerData : MonoBehaviour
{
    #region inputSystem
    //落下アクション定数
    const int FALLACTION_STAY_AIR_FRAME = 50;
    const int FALLACTION_FALL_POWER = 30;
    const int FALLACTION_MOVE_POWER = 10;

    //スコアと満腹度のレート
    const float SCORE_TIME_RATE = 0.2f;

    //落下アクション中か
    private bool _isFallAction;
    public bool IsFallAction
    {
        get => _isFallAction;
        private set
        {
            if (value != IsFallAction)
            {
                if (!value) ResetSpit();
                _isFallAction = value;
            }
        }
    }

    //入力値
    private Vector2 _moveAxis;
    private Vector2 _roteAxis;

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private GameObject playerCamera;

    private RoleDirectingScript directing;

    private DangoRole dangoRole = DangoRole.instance;

    //移動処理
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _moveAxis = context.ReadValue<Vector2>().normalized;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _moveAxis = Vector2.zero;
        }
    }

    //ジャンプ処理
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (IsGround)
            {
                _rigidbody.AddForce(Vector3.up * (_jumpPower + _Maxdango), ForceMode.Impulse);
            }
        }
    }

    //団子弾(取り外し)
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //串に何もなかったら実行しない。
            if (_dangos.Count == 0) return;

            //[Debug]何が消えたかわかるやつ
            //今までは、dangos[dangos.Count - 1]としなければなりませんでしたが、
            //C#8.0以降では以下のように省略できるようです。
            //問題は、これを知らない人が読むとわけが分からない。
            Logger.Log(_dangos[^1]);

            //消す処理。
            _dangos.RemoveAt(_dangos.Count - 1);
            DangoUISC.DangoUISet(_dangos);
        }
    }

    //突き刺しアニメーション
    public void OnAttack(InputAction.CallbackContext context)
    {
        //落下アクション中受け付けない。
        if (IsFallAction) return;

        if (context.phase == InputActionPhase.Performed)
        {
            if (FallAction()) return;

            //突き刺せる数を超えていた場合、実行しない
            if (_dangos.Count >= _Maxdango)
            {
                //なんらかの突けないことを知らせる処理推奨。

                Logger.Warn("突き刺せる数を超えています");
                _eventText.text = "それ以上させないよ！";
                return;
            }

            //ここに突き刺しアニメーションを推奨。
            spitManager.isSticking = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);

        }
        if (context.phase == InputActionPhase.Canceled)
        {
            ResetSpit();
        }
    }

    private void ResetSpit()
    {
        spitManager.isSticking = false;
        spitManager.gameObject.transform.localRotation = Quaternion.identity;
        spitManager.gameObject.transform.localPosition = new Vector3(0, 0.4f, 1.1f);
    }

    //食べる
    public void OnEatDango(InputAction.CallbackContext context)
    {
        //串に刺さってなかったら実行しない。
        if (_dangos.Count == 0) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                Logger.Log("食べチャージ開始！");
                _eventText.text = "食べチャージ中！";
                //SE推奨

                break;
            case InputActionPhase.Performed:
                Logger.Log("食べた！！");
                //SE推奨

                //食べた団子の点数を取得
                var score = dangoRole.CheckRole(_dangos);

                //演出関数の呼び出し
                _directing.Dirrecting(_dangos);

                _eventText.text = "食べた！" + (int)score + "点！";

                //満腹度を上昇
                _satiety += score * SCORE_TIME_RATE;

                //スコアを上昇
                GameManager.GameScore += score * 100f;

                //串をクリア。
                _dangos.Clear();

                //UI更新
                DangoUISC.DangoUISet(_dangos);
                break;
        }
    }

    //回転処理
    public void OnRote(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _roteAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _roteAxis = Vector2.zero;
        }

    }

    //（現状使用しません）
    public void OnCompression(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //for(int i = 0; i < debuffs.; i++)
        }
    }

    /// <summary>
    /// 刺突アクション
    /// </summary>
    /// <returns>可能かどうか</returns>
    private bool FallAction()
    {
        //接地している または 落下アクション中なら実行しない
        if (IsGround || IsFallAction) return false;

        //マックスまで刺してなかったら急降下突き刺しモーションに移行
        if (_dangos.Count < _Maxdango)
        {
            spitManager.isSticking = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, -2f, 0);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        StartCoroutine(StayAir());

        return true;
    }

    private IEnumerator StayAir()
    {
        IsFallAction = true;
        int time = FALLACTION_STAY_AIR_FRAME;

        while (--time > 0)
        {
            yield return new WaitForFixedUpdate();

            //滞空処理
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x / FALLACTION_MOVE_POWER, 0, _rigidbody.velocity.z / FALLACTION_MOVE_POWER);
        }

        _rigidbody.AddForce(Vector3.down * FALLACTION_FALL_POWER, ForceMode.Impulse);
    }

    #endregion

    //プレイヤーの能力
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _jumpPower = 10f;

    [SerializeField] private SpitManager spitManager = default!;
    [SerializeField] private DangoUIScript DangoUISC = default!;
    [SerializeField] private GameObject makerPrefab = default!;
    GameObject _maker = null;
    RoleDirectingScript _directing;

    //仮UI用加筆分
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _eventText;

    /// <summary>
    /// 満腹度、制限時間の代わり（単位:[sec]）
    /// </summary>
    /// フレーム数で管理しますが、ここでは秒管理で構いません。
    private float _satiety = 100f;

    /// <summary>
    /// 串、持ってる団子
    /// </summary>
    /// 今まではnew List<DangoColor>()としなければなりませんでしたが
    /// C#9.0以降はこのように簡素化出来るそうです。
    private List<DangoColor> _dangos = new();

    /// <summary>
    /// 刺せる数、徐々に増える
    /// </summary>    
    private int _Maxdango = 3;

    private float time = 0;

    private bool _isGround = false;

    public bool IsGround
    {
        get => _isGround;
        private set
        {
            if (value)
            {
                IsFallAction = false;
                _maker.SetActive(false);
            }

            _isGround = value;
        }
    }


    public Vector3 MoveVec { get; private set; }

    private void OnEnable()
    {
        InitDangos();
    }

    private void Start()
    {
        if (DangoUISC == null)
        {
            DangoUISC = GameObject.Find("Canvas").transform.Find("DangoBackScreen").GetComponent<DangoUIScript>();
        }

        if (_timeText == null)
        {
            _timeText = GameObject.Find("Canvas").transform.Find("Time").GetComponent<TextMeshProUGUI>();
        }

        if (_eventText == null)
        {
            _eventText = GameObject.Find("Canvas").transform.Find("Event").GetComponent<TextMeshProUGUI>();
        }

        _directing = GameObject.Find("Canvas").transform.Find("DirectingObj").GetComponent<RoleDirectingScript>();

        _maker = Instantiate(makerPrefab);
        _maker.SetActive(false);
    }

    private void Update()
    {
        IsGrounded();
        FallActionMaker();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        DecreaseSatiety();
        GrowStab();

        //仮でここに
        _timeText.text = "残り時間：" + (int)_satiety + "秒";

    }

    private void InitDangos()
    {
        if (_dangos == null) return;

        //初期化
        _dangos.Clear();
    }

    private void FallActionMaker()
    {
        if (IsGround) return;
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            _maker.transform.position = hit.point;
            _maker.SetActive(true);
        }
    }

    private void IsGrounded()
    {
        var ray = new Ray(transform.position, Vector3.down);
        IsGround = Physics.Raycast(ray, 1f);
    }

    /// <summary>
    /// Playerをカメラの方向に合わせて動かす関数。
    /// </summary>
    private void PlayerMove()
    {
        //カメラの向きを確認、Cameraforwardに代入
        var Cameraforward = Vector3.Scale(playerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        //カメラの向きを元にベクトルの作成
        MoveVec = _moveAxis.y * Cameraforward * _moveSpeed + _moveAxis.x * playerCamera.transform.right * _moveSpeed;

        if (_rigidbody.velocity.magnitude < 8f)
            _rigidbody.AddForce(MoveVec * _moveSpeed);
    }

    /// <summary>
    /// 満腹度をへらす関数、fixedUpdateに配置。
    /// </summary>
    private void DecreaseSatiety()
    {
        //満腹度を0.02秒(fixedUpdateの呼ばれる秒数)減らす
        _satiety -= Time.fixedDeltaTime;

        //ゲームマネージャー管理のほうがいいと思うけど
        //とりあえずここに置いておきます。
        FinishGame();

        //[debug]10秒おきにデバッグログを表示
        //if ((int)_satiety % 10 == 0) Logger.Log(_satiety);
    }

    /// <summary>
    /// 串が一定時間で伸びる処理
    /// </summary>
    private void GrowStab()
    {
        //刺せる団子の数が7だったら実行しない。
        if (_Maxdango == 7) return;

        float growTime = 10f;

        time += Time.fixedDeltaTime;

        if (time >= growTime)
        {
            _Maxdango++;
            Logger.Log("させる団子の数が増えた！" + _Maxdango);
            _eventText.text = "させる団子の数が増えた！(" + _Maxdango + "個)";
            time = 0;
        }
    }

    private void FinishGame()
    {
        int madeCount = 0;
        if (_satiety <= 0)
        {
            var posRoles = dangoRole.GetPosRoles();
            foreach (var posRole in posRoles)
            {
                if (posRole.GetMadeCount() > 0)
                {
                    madeCount++;
                }
            }
            Logger.Log("満足度：" + GameManager.GameScore * madeCount);
        }
    }

    public Vector2 GetRoteAxis() => _roteAxis;
    public List<DangoColor> GetDangoType() => _dangos;
    public DangoColor GetDangoType(int value)
    {
        try
        {
            return _dangos[value];
        }
        catch (IndexOutOfRangeException e)
        {
            Logger.Error(e);
            Logger.Error("代わりに先頭（配列の0番）を返します。");
            return _dangos[0];
        }
    }
    public int GetMaxDango() => _Maxdango;
    public List<DangoColor> GetDangos() => _dangos;
    public void AddDangos(DangoColor d) => _dangos.Add(d);

}
