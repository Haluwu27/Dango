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
            Control,
            Falling,
            FallAction,
            AttackAction,
            StayEatDango,
            EatDango,
            StayJumping,
            Jumping,
            Landing,
            StayRemoveDango,
            RemoveDango,

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
        new FallingState(),
        new FallActionState(),
        new AttackActionState(),
        new StayEatDangoState(),
        new EatDangoState(),
        new StayJumpingState(),
        new JumpingState(),
        new LandingState(),
        new StayRemoveDangoState(),
        new RemoveDangoState(),
     };

    class ControlState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._faceAnimationController.ChangeFaceType(FaceAnimationController.FaceTypes.Default);

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
            if (parent._playerJump.IsJumping) return IState.E_State.Unchanged;

            //MoveAnimation
            parent._playerMove.Animation();

            //プレイヤーを動かす処理
            parent._playerMove.Update(parent.rb, parent.playerCamera.transform, false);

            //満腹度（制限時間）減らす処理
            parent.DecreaseSatiety();

            //ステートに移行。
            if (parent._playerRemoveDango.HasRemoveDango) return IState.E_State.StayRemoveDango;
            if (parent._hasStayedEat) return IState.E_State.StayEatDango;
            if (parent._hasAttacked) return IState.E_State.AttackAction;
            if (parent._hasFalled) return IState.E_State.FallAction;
            if (parent._playerJump.IsStayJump) return IState.E_State.StayJumping;
            if (parent.rb.velocity.y < -0.1f && !parent.IsGround) return IState.E_State.Falling;

            return IState.E_State.Unchanged;
        }
    }
    class FallingState : IState
    {
        private async void PlayAnimation(PlayerData parent)
        {
            parent._animationManager.ChangeAnimation(AnimationManager.E_Animation.AN3Start, 0.1f);

            try
            {
                while (parent._animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                {
                    await UniTask.Yield();
                }
            }
            catch (MissingReferenceException)
            {
                return;
            }

            if (parent._currentState != IState.E_State.Falling) return;

            parent._animationManager.ChangeAnimation(AnimationManager.E_Animation.An3_FreeFall, 0.1f);
        }

        public IState.E_State Initialize(PlayerData parent)
        {
            PlayAnimation(parent);

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            //プレイヤーを動かす処理
            parent._playerMove.Update(parent.rb, parent.playerCamera.transform, true);

            //満腹度（制限時間）減らす処理
            parent.DecreaseSatiety();

            //ステートに移行
            if (parent._playerRemoveDango.HasRemoveDango) return IState.E_State.StayRemoveDango;
            if (parent._hasStayedEat) return IState.E_State.StayEatDango;
            if (parent.IsGround) return IState.E_State.Landing;
            if (parent._hasFalled) return IState.E_State.FallAction;
            if (parent._hasAttacked) return IState.E_State.AttackAction;

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
        PlayerAttackAction.AttackPattern pattern;

        public IState.E_State Initialize(PlayerData parent)
        {
            //移行フラグを折る
            parent._hasAttacked = false;

            //急降下アクション中なら団子の数の有無は無視して続行
            if (parent._playerFall.IsFallAction)
            {
                pattern = PlayerAttackAction.AttackPattern.FallAttack;
                return IState.E_State.Unchanged;
            }

            if (!parent.CanStab()) return IState.E_State.Control;

            pattern = PlayerAttackAction.AttackPattern.NormalAttack;

            parent._animationManager.ChangeAnimationEnforcement(AnimationManager.E_Animation.An5_Thrust, 0);
            parent._playerAttack.WasPressedAttackButton(parent.rb);

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            parent.DecreaseSatiety();

            if (parent._playerAttack.ChangeState(pattern))
            {
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
            parent._playerUIManager.StartDangoHighlight();

            SoundManager.Instance.PlaySE(Random.Range((int)SoundSource.VOISE_PRINCE_STAYEAT01, (int)SoundSource.VOISE_PRINCE_STAYEAT02 + 1));
            SoundManager.Instance.PlaySE(SoundSource.SE5_PLAYER_STAY_EATDANGO);

            parent._animationManager.ChangeAnimationEnforcement(AnimationManager.E_Animation.An4A_EatCharge, 0.1f);

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
                parent._playerUIManager.CancelHighlight();
                return IState.E_State.EatDango;
            }
            //待機をやめたらコントロールに戻る
            if (!parent._hasStayedEat)
            {
                parent.StartCoroutine(parent.ResetCameraView());
                parent._playerUIManager.CancelHighlight();
                return IState.E_State.Control;
            }
            return IState.E_State.Unchanged;
        }
    }
    class EatDangoState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._faceAnimationController.ChangeFaceType(FaceAnimationController.FaceTypes.Smile);
            parent._animationManager.ChangeAnimationEnforcement(AnimationManager.E_Animation.An4B_Eat, 0);
            parent._playerEat.EatDango(parent);
            parent._hasStayedEat = false;

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            return parent._playerEat.WaitAnimationComplete(parent._animator) ? IState.E_State.Control : IState.E_State.Unchanged;
        }

    }
    class StayJumpingState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._animationManager.ChangeAnimation(AnimationManager.E_Animation.An11Start, 0.2f);

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            var stateInfo = parent._animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("AN11Start") && stateInfo.normalizedTime >= 1f)
            {
                if (InputSystemManager.Instance.MoveAxis.magnitude > 0) parent._animationManager.ChangeAnimation(AnimationManager.E_Animation.An11A_JumpCharge_Walk, 0.2f);
                else parent._animationManager.ChangeAnimation(AnimationManager.E_Animation.An11B_JumpCharge, 0.2f);
            }
            //プレイヤーを動かす処理
            parent._playerMove.Update(parent.rb, parent.playerCamera.transform, true);

            //満腹度（制限時間）減らす処理
            parent.DecreaseSatiety();

            //ジャンプ
            parent._playerJump.SetIsGround(parent._isGround);
            parent._playerJump.SetMaxStabCount(parent._currentStabCount);

            if (parent._playerJump.IsJumping && !parent._isGround) return IState.E_State.Jumping;
            if (!parent._isGround) return IState.E_State.Falling;

            return IState.E_State.Unchanged;
        }

    }
    class JumpingState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._animationManager.ChangeAnimation(AnimationManager.E_Animation.An6_Jump, 0.06f);

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            //プレイヤーを動かす処理
            parent._playerMove.Update(parent.rb, parent.playerCamera.transform, true);

            //満腹度（制限時間）減らす処理
            parent.DecreaseSatiety();

            if (parent.IsGround) return IState.E_State.Landing;
            if (parent.rb.velocity.y < -0.1f && !parent.IsGround) return IState.E_State.Falling;

            return IState.E_State.Unchanged;
        }
    }
    class LandingState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._animationManager.ChangeAnimation(AnimationManager.E_Animation.An9_Landing, 0.2f);
            SoundManager.Instance.PlaySE(SoundSource.SE3_LANDING_HARD);

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            //満腹度（制限時間）減らす処理
            parent.DecreaseSatiety();

            if (parent._hasStayedEat) return IState.E_State.StayEatDango;
            if (InputSystemManager.Instance.MoveAxis.magnitude > 0) return IState.E_State.Control;
            if (parent._animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) return IState.E_State.Control;
            if (parent._hasAttacked) return IState.E_State.AttackAction;
            if (parent._playerJump.IsStayJump) return IState.E_State.StayJumping;

            return IState.E_State.Unchanged;
        }

    }
    class StayRemoveDangoState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            if (!parent.IsGround) return IState.E_State.RemoveDango;

            parent._animationManager.ChangeAnimation(AnimationManager.E_Animation.An8A_DangoRemoveCharge, 0.2f);

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            //プレイヤーを動かす処理
            parent._playerMove.Update(parent.rb, parent.playerCamera.transform, true);

            //満腹度（制限時間）減らす処理
            parent.DecreaseSatiety();

            if (!parent._playerRemoveDango.HasRemoveDango) return IState.E_State.Control;

            return parent._playerRemoveDango.IsStayCoolTime() ? IState.E_State.Unchanged : IState.E_State.RemoveDango;
        }
    }
    class RemoveDangoState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._playerRemoveDango.Remove();

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            //プレイヤーを動かす処理
            parent._playerMove.Update(parent.rb, parent.playerCamera.transform, true);

            //満腹度（制限時間）減らす処理
            parent.DecreaseSatiety();
            parent._animationManager.ChangeAnimation(AnimationManager.E_Animation.An8B_DangoRemove, 0);

            return parent._playerRemoveDango.IsRemoveCoolTime() ? IState.E_State.Unchanged : IState.E_State.Control;
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
    //StatePatternで各ステートに移動するフラグに使用
    private bool _hasAttacked = false;
    private bool _hasFalled = false;
    private bool _hasStayedEat = false;

    [SerializeField] Rigidbody rb = default!;
    [SerializeField] CapsuleCollider capsuleCollider = default!;
    [SerializeField] Camera playerCamera = default!;

    //プレイヤーの能力
    SpitManager spitManager = default!;
    [SerializeField] Canvas makerUI = default!;
    [SerializeField] FootObjScript footObj = default!;
    [SerializeField] PlayerKusiScript kusiObj = default!;

    //UI関連
    [SerializeField] RoleDirectingScript _directing;
    [SerializeField] PlayerUIManager _playerUIManager;
    [SerializeField] DangoUIScript _dangoUISC = default!;

    [SerializeField] Animator _animator = default!;

    [SerializeField] PhysicMaterial _ice = default!;
    [SerializeField] PhysicMaterial _normal = default!;

    [SerializeField] FaceAnimationController _faceAnimationController = default!;

    [SerializeField] PlayerSpitColliderManager _playerSpitCollider = default!;

    [SerializeField] SpitManager[] _swords = new SpitManager[5];

    [SerializeField] bool _allowReducedTimeLimit = true;

    const float DEFAULT_CAMERA_VIEW = 60f;
    const float CAMERA_REMOVETIME = 0.3f;

    int _mapLayer;

    //生成はAwakeで行っています。
    PlayerMove _playerMove;
    PlayerJump _playerJump;
    PlayerRemoveDango _playerRemoveDango;
    PlayerFallAction _playerFall;
    PlayerAttackAction _playerAttack;
    PlayerStayEat _playerStayEat;
    PlayerEat _playerEat;
    PlayerGrowStab _playerGrowStab;

    AnimationManager _animationManager;

    //映像やアニメーションのイベントフラグ
    public static bool Event = false;

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
                _animationManager.ChangeAnimation(AnimationManager.E_Animation.An7B_FallLanding, 0.2f);

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
        spitManager = _swords[0];

        _mapLayer = LayerMask.NameToLayer("Map");
        _animationManager = new(_animator);

        _playerAttack = new(_animator, spitManager);
        _playerFall = new(capsuleCollider, OnJump, OnJumpExit, _animationManager, _mapLayer);
        _playerRemoveDango = new(_dangos, _dangoUISC, this, _animator, kusiObj, spitManager, _playerUIManager);
        _playerMove = new(_animationManager);
        _playerJump = new(rb, OnJump, OnJumpExit, spitManager);
        _playerStayEat = new(this);
        _playerEat = new(_directing, _playerUIManager, kusiObj);
        _playerGrowStab = new();

        _playerSpitCollider.SetPlayerAttack(_playerAttack);

        makerUI.enabled = false;
        InitDangos();
    }

    private void Start()
    {
        InputSystemManager.Instance.onFirePerformed += _playerRemoveDango.OnPerformed;
        InputSystemManager.Instance.onFireCanceled += _playerRemoveDango.OnCanceled;
        InputSystemManager.Instance.onAttackPerformed += OnAttack;
        InputSystemManager.Instance.onEatDangoPerformed += OnEatDango;
        InputSystemManager.Instance.onEatDangoCanceled += OnEatDangoCanceled;
        InputSystemManager.Instance.onJumpPerformed += _playerJump.OnStayJumping;
        InputSystemManager.Instance.onJumpCanceled += _playerJump.Jump;
    }

    private void Update()
    {
        IsGrounded();
        UpdateState();
        FallActionMaker();
        _playerSpitCollider.SetRangeUIEnable(IsGround && !Event);
    }

    private void FixedUpdate()
    {
        FixedUpdateState();
        _playerAttack.FixedUpdate(transform);
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onFirePerformed -= _playerRemoveDango.OnPerformed;
        InputSystemManager.Instance.onFireCanceled -= _playerRemoveDango.OnCanceled;
        InputSystemManager.Instance.onAttackPerformed -= OnAttack;
        InputSystemManager.Instance.onEatDangoPerformed -= OnEatDango;
        InputSystemManager.Instance.onEatDangoCanceled -= OnEatDangoCanceled;
        InputSystemManager.Instance.onJumpPerformed -= _playerJump.OnStayJumping;
        InputSystemManager.Instance.onJumpCanceled -= _playerJump.Jump;
    }

    //突き刺しボタン降下時処理
    private async void OnAttack()
    {
        //ヒットストップ中受け付けない
        if (spitManager.IsHitStop) return;

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
            _playerUIManager.DontEat();
            return;
        }

        //限界まで団子が刺さっていなかったら実行しない。
        if (_dangos.Count != _currentStabCount)
        {
            _playerUIManager.DontEat();
            return;
        }

        _hasStayedEat = true;
    }

    private void OnEatDangoCanceled()
    {
        _hasStayedEat = false;
        SoundManager.Instance.StopSE(SoundSource.SE5_PLAYER_STAY_EATDANGO);
    }

    private void ResetSpit()
    {
        spitManager.IsSticking = false;
    }

    private void OnJump()
    {
        InputSystemManager.Instance.onEatDangoPerformed -= OnEatDango;
        InputSystemManager.Instance.onEatDangoCanceled -= OnEatDangoCanceled;
    }

    private void OnJumpExit()
    {
        InputSystemManager.Instance.onEatDangoPerformed += OnEatDango;
        InputSystemManager.Instance.onEatDangoCanceled += OnEatDangoCanceled;
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
        //地上なら確実に動作しない
        if (IsGround)
        {
            makerUI.enabled = false;
            return;
        }

        Ray ray = new(transform.position, Vector3.down);

        //描画しなくていいなら弾く
        if (Physics.Raycast(ray, capsuleCollider.height + capsuleCollider.height / 2f, 1 << _mapLayer))
        {
            makerUI.enabled = false;
            return;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << _mapLayer))
        {
            makerUI.transform.position = (hit.point + transform.forward.normalized * 0.313f).SetY(hit.point.y + 0.01f);

            //突き刺しできるようになったら有効化
            makerUI.enabled = true;
        }
    }

    private bool CanStab()
    {
        if (Event) return false;
        //団子がこれ以上させないなら実行しない
        if (_dangos.Count >= _currentStabCount)
        {
            _playerUIManager.EventText.TextData.SetText("それ以上させないよ！");

            //すでに再生されているのを止めてSEを再生
            SoundManager.Instance.PlaySE(SoundSource.SE7_CANT_STAB_DANGO, true);

            EraseEventText(2f).Forget();

            return false;
        }

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
        //var ray = new Ray(new(transform.position.x, transform.position.y + capsuleCollider.height / 2f, transform.position.z), Vector3.down);
        IsGround = footObj.GetIsGround(); /*Physics.Raycast(ray, capsuleCollider.height / 1.5f);*/

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + capsuleCollider.height / 2f, transform.position.z), Vector3.down, Color.red);
    }

    /// <summary>
    /// 満腹度をへらす関数、fixedUpdateに配置。
    /// </summary>
    private void DecreaseSatiety()
    {
        if (Event) return;

        //空腹度減少の許可
        if (!_allowReducedTimeLimit) return;

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
        _swords[_currentStabCount - 3].gameObject.SetActive(true);
        _swords[_currentStabCount - 4].gameObject.SetActive(false);
        spitManager = _swords[_currentStabCount - 3];
        _playerAttack.SetSpitManager(spitManager);
        _playerUIManager.EventText.TextData.SetText("させる団子の数が増えた！(" + _currentStabCount + "個)");

        //刺せる範囲表示の拡大。今串が伸びないのでコメントアウトしてます。
        //parent.rangeUI.transform.localScale = new Vector3(parent.rangeUI.transform.localScale.x, parent.rangeUI.transform.localScale.y + 0.01f, parent.rangeUI.transform.localScale.z);
    }

    public int GetCurrentStabCount()
    {
        return _currentStabCount;
    }

    public async void EatAnima()
    {
        _playerUIManager.EatDangoUI_True();
        await UniTask.Delay(5000);
        DangoRoleUI.OnGUIReset();
    }

    #region GetterSetter
    public int GetMaxDango() => _currentStabCount;

    public int SetMaxDango(int i) => _currentStabCount = i;
    public List<DangoColor> GetDangos() => _dangos;
    public void AddDangos(DangoColor d)
    {
        Logger.Warn("団子を強制的に追加しました");

        //団子が刺さったタイミングを通知
        _playerAttack.SetHasStabDango();

        _dangos.Add(d);
    }
    public void AddDangos(DangoData dango)
    {
        //団子が刺さったタイミングを通知
        _playerAttack.SetHasStabDango();
        _playerAttack.RemoveTargetDango(dango);

        _dangos.Add(dango.GetDangoColor());
    }
    public void ResetDangos() => _dangos.Clear();
    public float GetSatiety() => _satiety;
    public void AddSatiety(float value) => _satiety += value;
    public DangoUIScript GetDangoUIScript() => _dangoUISC;
    public PlayerUIManager GetPlayerUIManager() => _playerUIManager;
    public Animator GetAnimator() => _animator;
    public void SetIsMoveable(bool enable) => _playerMove.SetIsMoveable(enable);

    #endregion
}