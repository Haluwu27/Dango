using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TM.Entity.Player;
using UnityEngine;

/// 
/// ＜実装すること＞
/// ・接地判定をまともに
/// 

[RequireComponent(typeof(Rigidbody), typeof(RideMoveObj))]
class PlayerData : MonoBehaviour
{
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
     };

    class ControlState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            //串の状態をリセット
            parent.ResetSpit();
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            parent._animator.SetFloat(parent._velocityYHash, parent.rb.velocity.y);

            if (parent._playerJump.IsJumping) return IState.E_State.Unchanged;

            //プレイヤーを動かす処理
            parent._playerMove.Update(parent.rb, parent.playerCamera.transform, parent._playerRemoveDango.IsCoolDown, parent._playerJump.IsStayJump);

            //満腹度（制限時間）減らす処理
            parent.DecreaseSatiety();

            //ジャンプ
            parent._playerJump.SetIsGround(parent._isGround);
            parent._playerJump.SetMaxStabCount(parent._currentStabCount);

            //ステートに移行。
            if (parent._hasStayedEat) return IState.E_State.StayEatDango;
            if (parent._hasAttacked) return IState.E_State.AttackAction;
            if (parent._hasFalled) return IState.E_State.FallAction;

            return IState.E_State.Unchanged;
        }
    }
    class FallActionState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._animator.SetTrigger(parent._fallActionTriggerHash);
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
            parent._playerMove.Update(parent.rb, parent.playerCamera.transform, parent._playerRemoveDango.IsCoolDown,parent._playerJump.IsStayJump);

            //制限時間減少
            parent.DecreaseSatiety();

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
            SoundManager.Instance.PlaySE(Random.Range((int)SoundSource.VOISE_PRINCE_ATTACK01, (int)SoundSource.VOISE_PRINCE_ATTACK02 + 1));

            //急降下アクション中なら団子の数の有無は無視して続行
            if (parent._playerFall.IsFallAction) return IState.E_State.Unchanged;

            if (!parent.CanStab()) return IState.E_State.Control;

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            parent.DecreaseSatiety();

            if (parent._playerAttack.FixedUpdate())
            {
                parent._animator.SetBool(parent._isEndWalkHash, InputSystemManager.Instance.MoveAxis.magnitude > 0);
                return IState.E_State.Control;
            }

            return IState.E_State.Unchanged;
        }
    }
    class StayEatDangoState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._playerUIManager.EventText.TextData.SetText("食べチャージ中！");
            parent._playerStayEat.ResetCount();
            //SE推奨
            parent._animator.SetBool("IsEatingCharge", true);

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            parent.OnChargeCameraMoving();

            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            parent.DecreaseSatiety();

            //食べる待機が終わったら食べるステートに移行
            if (parent._playerStayEat.CanEat())
            {
                parent.StartCoroutine(parent.ResetCameraView());
                return IState.E_State.EatDango;
            }
            //待機をやめたらコントロールに戻る
            if (!parent._hasStayedEat)
            {
                parent.StartCoroutine(parent.ResetCameraView());
                parent._animator.SetBool("IsEatingCharge", false);
                return IState.E_State.Control;
            }
            return IState.E_State.Unchanged;
        }
    }
    class EatDangoState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._animator.SetTrigger("EatingTrigger");
            parent.EatDango();
            return IState.E_State.Control;
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
    //スコアと満腹度のレート
    const float SCORE_TIME_RATE = 0.2f;

    //StatePatternで各ステートに移動するフラグに使用
    private bool _hasAttacked = false;
    private bool _hasFalled = false;
    private bool _hasStayedEat = false;

    [SerializeField] Rigidbody rb = default!;
    [SerializeField] CapsuleCollider capsuleCollider = default!;
    [SerializeField] Camera playerCamera = default!;

    //プレイヤーの能力
    [SerializeField] SpitManager spitManager = default!;
    [SerializeField] GameObject makerUI = default!;
    [SerializeField] GameObject rangeUI = default!;

    //UI関連
    [SerializeField] RoleDirectingScript _directing;
    [SerializeField] PlayerUIManager _playerUIManager;
    [SerializeField] DangoUIScript _dangoUISC = default!;

    [SerializeField] Animator _animator = default!;

    [SerializeField] PhysicMaterial _ice = default!;
    [SerializeField] PhysicMaterial _normal = default!;

    [SerializeField] ImageUIData _attackRangeImage = default!;

    //串を伸ばす処理
    readonly PlayerGrowStab _playerGrowStab = new();

    const float DEFAULT_CAMERA_VIEW = 60f;
    const float CAMERA_REMOVETIME = 0.3f;

    const float EVENTTEXT_FLASH_TIME = 0.4f;
    const float EVENTTEXT_PRINT_TIME = 2.4f;

    readonly DangoRole dangoRole = DangoRole.instance;

    readonly PlayerStayEat _playerStayEat = new();

    //生成はAwakeで行っています。
    PlayerMove _playerMove;
    PlayerJump _playerJump;
    PlayerRemoveDango _playerRemoveDango;
    PlayerFallAction _playerFall;
    PlayerAttackAction _playerAttack;

    //映像やアニメーションのイベントフラグ
    public static bool Event = false;

    //animatorのハッシュ値を取得（最適化の処理）
    int _isGroundHash = Animator.StringToHash("IsGround");
    int _attackTriggerHash = Animator.StringToHash("AttackTrigger");
    int _velocityYHash = Animator.StringToHash("VelocityY");
    int _fallActionTriggerHash = Animator.StringToHash("FallActionTrigger");
    int _fallActionLandingTriggerHash = Animator.StringToHash("FallActionLandingTrigger");
    int _isEndWalkHash = Animator.StringToHash("IsEndWalk");

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
    readonly List<DangoColor> _dangos = new();

    /// <summary>
    /// 刺せる数、徐々に増える
    /// </summary>    
    private int _currentStabCount = 3;

    private bool _isGround = false;

    public bool IsGround
    {
        get => _isGround;
        private set
        {
            if (value && _playerFall.IsFallAction)
            {
                //AN7Bの再生もここ
                _animator.SetTrigger(_fallActionLandingTriggerHash);

                SoundManager.Instance.PlaySE(SoundSource.SE11_FALLACTION_LANDING);

                _playerFall.IsFallAction = false;
            }

            //空中時は摩擦をカット
            capsuleCollider.sharedMaterial = value ? _normal : _ice;
            _isGround = value;
        }
    }

    public Rigidbody Rb => rb;
    public PlayerFallAction PlayerFall => _playerFall;

    public Vector3 MoveVec { get; private set; }
    #endregion

    private void Awake()
    {
        _playerAttack = new(_attackRangeImage, this, _animator);
        _playerFall = new(capsuleCollider, OnJump, OnJumpExit);
        _playerRemoveDango = new(_dangos, _dangoUISC, this, _animator);
        _playerMove = new(_animator);
        _playerJump = new(rb, OnJump, OnJumpExit, _animator);
    }

    private void OnEnable()
    {
        InitDangos();
    }

    private void Start()
    {
        InputSystemManager.Instance.onFirePerformed += _playerRemoveDango.Remove;
        InputSystemManager.Instance.onAttackPerformed += OnAttack;
        InputSystemManager.Instance.onEatDangoPerformed += OnEatDango;
        InputSystemManager.Instance.onEatDangoCanceled += () => _hasStayedEat = false;
        InputSystemManager.Instance.onJumpPerformed += _playerJump.OnStayJumping;
        InputSystemManager.Instance.onJumpCanceled += _playerJump.Jump;
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
        FixedUpdateState();
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onFirePerformed -= _playerRemoveDango.Remove;
        InputSystemManager.Instance.onAttackPerformed -= OnAttack;
        InputSystemManager.Instance.onEatDangoPerformed -= OnEatDango;
        InputSystemManager.Instance.onEatDangoCanceled -= () => _hasStayedEat = false;
        InputSystemManager.Instance.onJumpPerformed -= _playerJump.OnStayJumping;
        InputSystemManager.Instance.onJumpCanceled -= _playerJump.Jump;
    }

    //突き刺しボタン降下時処理
    private async void OnAttack()
    {
        //落下アクション中受け付けない。
        if (_playerFall.IsFallAction) return;

        //地上なら確実に通常攻撃
        if (_isGround)
        {
            _hasAttacked = true;
            return;
        }

        //Yの加速度が負になるまで待機
        await WaitVelocityYLessZero();

        //急降下するか判定、しないなら通常刺しに移行
        _hasFalled = _playerFall.ToFallAction(transform.position, _isGround);
        _hasAttacked = !_hasFalled;
    }

    private async UniTask WaitVelocityYLessZero()
    {
        while (rb.velocity.y > 0)
        {
            await UniTask.Yield();
            if (!InputSystemManager.Instance.IsPressAttack) break;
        }
    }

    //食べる
    private void OnEatDango()
    {
        //串に刺さってなかったら実行しない。
        if (_dangos.Count == 0) return;

        //何らかの動作中で食べようとしても実行しない。
        if (_currentState is not (IState.E_State.Control or IState.E_State.StayEatDango))
        {
            _playerUIManager.EventText.TextData.SetText("食べられないよ！");
            _playerUIManager.EventText.TextData.FlashAlpha(EVENTTEXT_PRINT_TIME, EVENTTEXT_FLASH_TIME, 0);
            return;
        }

        //限界まで団子が刺さっていなかったら実行しない。
        if (_dangos.Count != _currentStabCount)
        {
            _playerUIManager.EventText.TextData.SetText("食べられないよ！");
            _playerUIManager.EventText.TextData.FlashAlpha(EVENTTEXT_PRINT_TIME, EVENTTEXT_FLASH_TIME, 0);
            return;
        }

        SoundManager.Instance.PlaySE(Random.Range((int)SoundSource.VOISE_PRINCE_STAYEAT01, (int)SoundSource.VOISE_PRINCE_STAYEAT02 + 1));
        _hasStayedEat = true;
    }

    private void EatDango()
    {
        //SE
        //GameManager.SoundManager.PlaySE(SoundSource.SE_PLAYER_EATDANGO);

        _hasStayedEat = false;

        //食べた団子の点数を取得
        var score = dangoRole.CheckRole(_dangos, _currentStabCount);

        //演出関数の呼び出し
        _directing.Dirrecting(_dangos);

        _playerUIManager.EventText.TextData.SetText("食べた！" + (int)score + "点！");


        //満腹度を上昇
        //_satiety += score * SCORE_TIME_RATE;

        //スコアを上昇
        GameManager.GameScore += score * 100f;

        //串をクリア。
        _dangos.Clear();

        //UI更新
        _dangoUISC.DangoUISet(_dangos);
        //一部UIの非表示
        _playerUIManager.EatDangoUI_False();
    }

    private void ResetSpit()
    {
        spitManager.IsSticking = false;
    }

    private void OnJump()
    {
        InputSystemManager.Instance.onFirePerformed -= _playerRemoveDango.Remove;
        InputSystemManager.Instance.onEatDangoPerformed -= OnEatDango;
        InputSystemManager.Instance.onEatDangoCanceled -= () => _hasStayedEat = false;
    }

    private void OnJumpExit()
    {
        InputSystemManager.Instance.onFirePerformed += _playerRemoveDango.Remove;
        InputSystemManager.Instance.onEatDangoPerformed += OnEatDango;
        InputSystemManager.Instance.onEatDangoCanceled += () => _hasStayedEat = false;
    }

#if UNITY_EDITOR
    //デバッグ終わりに削除
    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 100, 50), "" + _currentState);
    }
#endif

    private void InitDangos()
    {
        if (_dangos == null) return;

        //初期化
        _dangos.Clear();
    }

    private void FallActionMaker()
    {
        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            makerUI.transform.position = hit.point + new Vector3(0, 0.01f, 0);

            //突き刺しできるようになったら有効化
            makerUI.SetActive(!Physics.Raycast(ray, capsuleCollider.height + capsuleCollider.height / 2f));
        }

    }

    private void RangeUI()
    {
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            rangeUI.transform.position = new Vector3(rangeUI.transform.position.x, hit.point.y + 0.01f, rangeUI.transform.position.z);
        }

        //空中でも出てると違和感あったので消します
        rangeUI.SetActive(_isGround);
        rangeUI.SetActive(!Event);
    }

    private bool CanStab()
    {
        //団子がこれ以上させないなら実行しない
        if (_dangos.Count >= _currentStabCount)
        {
            _playerUIManager.EventText.TextData.SetText("それ以上させないよ！");

            //すでに再生されているのを止めてSEを再生
            SoundManager.Instance.PlaySE(SoundSource.SE7_CANT_STAB_DANGO, true);

            EraseEventText(2f).Forget();

            return false;
        }

        //刺す時間の内部でカウントしているTimeをリセット
        _playerAttack.ResetTime();

        //突き刺せる状態にして
        spitManager.IsSticking = true;

        //串の位置を変更（アニメーション推奨）
        _animator.SetTrigger(_attackTriggerHash);

        return true;
    }

    private async UniTask EraseEventText(float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            await UniTask.Yield();
            currentTime += Time.deltaTime;
            if (InputSystemManager.Instance.WasPressedThisFrameAttack) return;
        }
        _playerUIManager.EventText.TextData.SetText();
    }

    private void IsGrounded()
    {
        var ray = new Ray(new(transform.position.x, transform.position.y + capsuleCollider.height / 2f, transform.position.z), Vector3.down);
        IsGround = Physics.Raycast(ray, capsuleCollider.height / 1.5f);
        _animator.SetBool(_isGroundHash, IsGround);

        Debug.DrawRay(new Vector3(transform.position.x,transform.position.y + capsuleCollider.height / 2f,transform.position.z), Vector3.down, Color.red);
    }

    /// <summary>
    /// 満腹度をへらす関数、fixedUpdateに配置。
    /// </summary>
    private void DecreaseSatiety()
    {
        //満腹度を0.02秒(fixedUpdateの呼ばれる秒数)減らす
        _satiety -= Time.fixedDeltaTime;
    }

    private void OnChargeCameraMoving()
    {
        playerCamera.fieldOfView -= 10f * Time.deltaTime;
    }

    private IEnumerator ResetCameraView()
    {
        float view = playerCamera.fieldOfView;
        float hokann = DEFAULT_CAMERA_VIEW - view;
        while (playerCamera.fieldOfView <= DEFAULT_CAMERA_VIEW)
        {
            playerCamera.fieldOfView += (hokann / CAMERA_REMOVETIME) * Time.deltaTime;
            yield return null;
        }
        playerCamera.fieldOfView = DEFAULT_CAMERA_VIEW;
    }

    public void GrowStab(bool enable)
    {
        if (!enable) return;

        _currentStabCount = _playerGrowStab.GrowStab(_currentStabCount);
        _playerUIManager.EventText.TextData.SetText("させる団子の数が増えた！(" + _currentStabCount + "個)");

        //刺せる範囲表示の拡大。今串が伸びないのでコメントアウトしてます。
        //parent.rangeUI.transform.localScale = new Vector3(parent.rangeUI.transform.localScale.x, parent.rangeUI.transform.localScale.y + 0.01f, parent.rangeUI.transform.localScale.z);
    }

    public int GetCurrentStabCount()
    {
        return _currentStabCount;
    }

    public void EatAnima()
    {
        _animator.SetBool("IsEatingCharge", false);
        _playerUIManager.EatDangoUI_True();
    }

    #region GetterSetter
    public int GetMaxDango() => _currentStabCount;

    public int SetMaxDango(int i) => _currentStabCount = i;
    public List<DangoColor> GetDangos() => _dangos;
    public void AddDangos(DangoColor d) => _dangos.Add(d);
    public void ResetDangos()=>_dangos.Clear();
    public float GetSatiety() => _satiety;
    public void AddSatiety(float value) => _satiety += value;
    public DangoUIScript GetDangoUIScript() => _dangoUISC;

    #endregion
}