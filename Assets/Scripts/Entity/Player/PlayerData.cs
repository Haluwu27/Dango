using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// 
/// ＜実装すること＞
/// ・接地判定をまともに
/// 

[RequireComponent(typeof(Rigidbody), typeof(RideMoveObj))]
class PlayerData : MonoBehaviour
{
    #region inputSystem
    //スコアと満腹度のレート
    const float SCORE_TIME_RATE = 0.2f;

    //入力値
    private Vector2 _moveAxis;
    private Vector2 _roteAxis;

    private bool _hasAttacked = false;
    private bool _hasFalled = false;
    private bool _hasStayedEat = false;
    private bool _hasJumped = false;

    [SerializeField] private Rigidbody rb = default!;
    [SerializeField] private CapsuleCollider capsuleCollider = default!;
    [SerializeField] private GameObject playerCamera = default!;

    private DangoRole dangoRole = DangoRole.instance;

    private PlayerFallAction _playerFall = new();
    private PlayerAttackAction _playerAttack = new();

    public PlayerFallAction PlayerFall => _playerFall;

    //移動処理
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _moveAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _moveAxis = Vector2.zero;
        }
    }

    //ジャンプ処理
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsGround) return;

        if (context.phase == InputActionPhase.Performed)
        {
            _hasJumped = true;
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

            //SE
            GameManager.SoundManager.PlaySE(SoundSource.SE_REMOVE_DANGO);

            //消す処理。
            _dangos.RemoveAt(_dangos.Count - 1);
            _dangoUISC.DangoUISet(_dangos);
        }
    }

    //突き刺しアニメーション
    public void OnAttack(InputAction.CallbackContext context)
    {
        //落下アクション中受け付けない。
        if (_playerFall.IsFallAction) return;

        if (context.phase == InputActionPhase.Performed)
        {
            //空中かつ地面に近すぎなければ落下刺しに移行
            if (!_isGround)
            {
                Ray ray = new(transform.position, Vector3.down);

                //近くに地面があるか(playerの半分の大きさ)判定
                if (Physics.Raycast(ray, capsuleCollider.height + capsuleCollider.height / 2f)) _hasAttacked = true;
                else _hasFalled = true;
            }
            //地面なら普通に突き刺しに移行
            else
            {
                _hasAttacked = true;
            }
        }
    }

    //食べる
    public void OnEatDango(InputAction.CallbackContext context)
    {
        //串に刺さってなかったら実行しない。
        if (_dangos.Count == 0) return;

        if (context.phase == InputActionPhase.Performed) _hasStayedEat = true;
        else if (context.phase == InputActionPhase.Canceled) _hasStayedEat = false;
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

    private void EatDango()
    {
        //SE
        GameManager.SoundManager.PlaySE(SoundSource.SE_PLAYER_EATDANGO);

        _hasStayedEat = false;

        //食べた団子の点数を取得
        var score = dangoRole.CheckRole(_dangos);

        //演出関数の呼び出し
        _directing.Dirrecting(_dangos);

        _playerUIManager.SetEventText("食べた！" + (int)score + "点！");

        //残り時間の増加
        PlayerUIManager.time += score;

        //満腹度を上昇
        _satiety += score * SCORE_TIME_RATE;

        //スコアを上昇
        GameManager.GameScore += score * 100f;

        //串をクリア。
        _dangos.Clear();
        //UI更新
        _dangoUISC.DangoUISet(_dangos);
    }

    private void ResetSpit()
    {
        spitManager.isSticking = false;
        spitManager.gameObject.transform.localRotation = Quaternion.identity;
        spitManager.gameObject.transform.localPosition = new Vector3(0, 0.4f, 1.1f);
    }
    #endregion

    #region statePattern
    interface IState
    {
        public enum E_State
        {
            Control = 0,
            FallAction = 1,
            AttackAction = 2,
            StayEatDango = 3,
            EatDango = 4,
            GrowStab = 5,

            Max,

            Unchanged,
        }

        E_State Initialize(PlayerData parent);
        E_State Update(PlayerData parent);
        E_State FixedUpdate(PlayerData parent);
    }
    //状態管理
    private IState.E_State _currentState = IState.E_State.Control;
    private static readonly IState[] states = new IState[(int)IState.E_State.Max]
     {
        new ControlState(),
        new FallActionState(),
        new AttackActionState(),
        new StayEatDangoState(),
        new EatDangoState(),
        new GrowStabState(),
     };

    class ControlState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            //串の位置をリセット
            parent.ResetSpit();
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            //プレイヤーを動かす処理
            parent.PlayerMove(parent._cameraForward);

            //ステートに移行。
            if (parent._hasAttacked) return IState.E_State.AttackAction;
            if (parent._hasStayedEat) return IState.E_State.StayEatDango;
            if (parent._hasFalled) return IState.E_State.FallAction;

            return IState.E_State.Unchanged;
        }
    }
    class FallActionState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._hasFalled = false;
            parent._playerFall.IsFallAction = true;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            //移動
            parent.PlayerMove(parent._cameraForward);

            //途中で接地したらコントロールに戻る
            if (parent.IsGround) return IState.E_State.Control;

            //待機時間が終わったらアタックステートに移行
            return parent._playerFall.FixedUpdate(parent.rb, parent.spitManager)
                ? IState.E_State.AttackAction : IState.E_State.Unchanged;
        }
    }
    class AttackActionState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._hasAttacked = false;

            if (!parent.CanStab()) return IState.E_State.Control;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            return parent._playerAttack.FixedUpdate() ? IState.E_State.Control : IState.E_State.Unchanged;
        }

    }
    class StayEatDangoState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._playerUIManager.SetEventText("食べチャージ中！");
            parent._playerStayEat.ResetCount();
            //SE推奨

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            //チャージしてる感じのアニメーションとかはここ

            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            parent.PlayerMove(parent._cameraForward);

            //食べる待機が終わったら食べるステートに移行
            if (parent._playerStayEat.CanEat()) return IState.E_State.EatDango;

            //待機をやめたらコントロールに戻る
            if (!parent._hasStayedEat) return IState.E_State.Control;

            return IState.E_State.Unchanged;
        }
    }
    class EatDangoState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            if (parent._canGrowStab) return IState.E_State.GrowStab;

            parent.EatDango();
            return IState.E_State.Control;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            parent.PlayerMove(parent._cameraForward);

            return IState.E_State.Unchanged;
        }

    }
    class GrowStabState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._maxStabCount = parent._playerGrowStab.GrowStab(parent._maxStabCount);
            parent._playerUIManager.SetEventText("させる団子の数が増えた！(" + parent._maxStabCount + "個)");
            parent._canGrowStab = false;
            //刺せる範囲表示の拡大。今串が伸びないのでコメントアウトしてます。
            //parent.rangeUI.transform.localScale = new Vector3(parent.rangeUI.transform.localScale.x, parent.rangeUI.transform.localScale.y + 0.01f, parent.rangeUI.transform.localScale.z);
            return IState.E_State.EatDango;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }

    }

    private void InitState()
    {
        Logger.Assert(_currentState is >= 0 and < IState.E_State.Max);

        var nextState = states[(int)_currentState].Initialize(this);

        if (nextState != IState.E_State.Unchanged)
        {
            _currentState = nextState;
            InitState();//初期化で状態が変わるなら再帰的に初期化する。
        }
    }
    private void UpdateState()
    {
        Logger.Assert(_currentState is >= 0 and < IState.E_State.Max);

        var nextState = states[(int)_currentState].Update(this);

        if (nextState != IState.E_State.Unchanged)
        {
            //次に遷移
            _currentState = nextState;
            InitState();
        }
    }
    private void FixedUpdateState()
    {
        Logger.Assert(_currentState is >= 0 and < IState.E_State.Max);

        var nextState = states[(int)_currentState].FixedUpdate(this);

        if (nextState != IState.E_State.Unchanged)
        {
            //次に遷移
            _currentState = nextState;
            InitState();
        }
    }
    #endregion

    #region メンバ変数
    //プレイヤーの能力
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _jumpPower = 10f;

    [SerializeField] private SpitManager spitManager = default!;
    [SerializeField] private GameObject makerUI = default!;
    [SerializeField] private GameObject rangeUI = default;

    //UI関連
    RoleDirectingScript _directing;
    PlayerUIManager _playerUIManager;
    DangoUIScript _dangoUISC;

    //串を伸ばす処理
    const int MAX_DANGO = 7;
    const int GROW_STAB_FRAME = 500;
    PlayerGrowStab _playerGrowStab = new(MAX_DANGO, GROW_STAB_FRAME);
    bool _canGrowStab = false;

    //食べる処理
    const int STAY_FRAME = 100;
    PlayerStayEat _playerStayEat = new(STAY_FRAME);

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
    private int _maxStabCount = 3;

    private bool _isGround = false;

    public bool IsGround
    {
        get => _isGround;
        private set
        {
            if (value)
            {
                _playerFall.IsFallAction = false;
            }

            _isGround = value;
        }
    }

    //移動速度定数
    const float MAX_VELOCITY_MAG = 8f;
    const float RUN_SPEED_MAG = 7f;

    public Vector3 MoveVec { get; private set; }
    private Vector3 _cameraForward;

    public void SetCameraForward(Vector3 camForward) => _cameraForward = camForward;
    #endregion

    private void Awake()
    {
        _playerUIManager = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUIManager>();
        _dangoUISC = GameObject.Find("PlayerUICanvas").transform.Find("DangoBackScreen").GetComponent<DangoUIScript>();
        _directing = GameObject.Find("PlayerUICanvas").transform.Find("DirectingObj").GetComponent<RoleDirectingScript>();
    }

    private void OnEnable()
    {
        InitDangos();
    }

    private void Start()
    {
        makerUI.SetActive(false);
    }

    private void Update()
    {
        IsGrounded();
        UpdateState();
        FallActionMaker();
        RangeUI();
    }

    private void FixedUpdate()
    {
        DecreaseSatiety();
        _canGrowStab = _playerGrowStab.CanGrowStab(_maxStabCount);
        FixedUpdateState();
        if (_hasJumped)
        {
            rb.AddForce(Vector3.up * (_jumpPower + _maxStabCount), ForceMode.Impulse);
            _hasJumped = false;
        }
    }

    //デバッグ終わりに削除
    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 100, 50), "" + _currentState);
    }

    private void InitDangos()
    {
        if (_dangos == null) return;

        //初期化
        _dangos.Clear();
    }

    private void FallActionMaker()
    {
        var ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit,Mathf.Infinity))
        {
            makerUI.transform.position = hit.point + new Vector3(0,0.01f,0);
            //突き刺しできるようになったら有効化
            if (!Physics.Raycast(ray, capsuleCollider.height + capsuleCollider.height / 2f))
                makerUI.SetActive(true);
            else
                makerUI.SetActive(false);
        }
        
    }

    private void RangeUI()
    {
        var ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            rangeUI.transform.position = new Vector3(rangeUI.transform.position.x, hit.point.y + 0.01f, rangeUI.transform.position.z);
        }

        //空中でも出てると違和感あったので消します
        if (_isGround)
        {
            rangeUI.SetActive(true);
        }
        else
        {
            rangeUI.SetActive(false);
        }
    }

    private bool CanStab()
    {
        //団子がこれ以上させないなら実行しない
        if (_dangos.Count >= _maxStabCount)
        {
            Logger.Warn("突き刺せる数を超えています");
            _playerUIManager.SetEventText("それ以上させないよ！");

            return false;
        }

        //刺す時間の内部でカウントしているTimeをリセット
        _playerAttack.ResetTime();

        //突き刺せる状態にして
        spitManager.isSticking = true;

        //串の位置を変更（アニメーション推奨）
        if (_playerFall.IsFallAction)
        {
            spitManager.gameObject.transform.localPosition = new Vector3(0, -2f, 0);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);
        }

        return true;
    }

    private void IsGrounded()
    {
        var ray = new Ray(transform.position, Vector3.down);
        IsGround = Physics.Raycast(ray, capsuleCollider.height / 1.999f);
    }

    /// <summary>
    /// Playerをカメラの方向に合わせて動かす関数。
    /// </summary>
    private void PlayerMove(Vector3 camForward)
    {
        //カメラの向きを元にベクトルの作成
        MoveVec = _moveAxis.y * _moveSpeed * _cameraForward + _moveAxis.x * _moveSpeed * playerCamera.transform.right;

        if (rb.velocity.magnitude < MAX_VELOCITY_MAG)
        {
            float mag = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
            float speedMag = RUN_SPEED_MAG - mag;
            rb.AddForce(MoveVec * speedMag);
        }
    }

    /// <summary>
    /// 満腹度をへらす関数、fixedUpdateに配置。
    /// </summary>
    private void DecreaseSatiety()
    {
        //満腹度を0.02秒(fixedUpdateの呼ばれる秒数)減らす
        _satiety -= Time.fixedDeltaTime;
    }

    #region GetterSetter
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
    public void PlayerRotate(Quaternion rot)
    {
        if (_currentState != IState.E_State.Control) return;

        transform.rotation = rot;
    }

    public int GetMaxDango() => _maxStabCount;
    public List<DangoColor> GetDangos() => _dangos;
    public void AddDangos(DangoColor d) => _dangos.Add(d);
    public float GetSatiety() => _satiety;
    public DangoUIScript GetDangoUIScript() => _dangoUISC;

    #endregion
}