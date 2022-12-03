using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Easing;
using TM.Easing.Management;
using System;
using static FloorManager;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DangoInjection : MonoBehaviour
{
    [Flags]
    private enum DangoColorChoice
    {
        [InspectorName("全取り消し")]
        None = 0,

        An = 1 << 1,
        Beni = 1 << 2,
        Mitarashi = 1 << 3,
        Nori = 1 << 4,
        Shiratama = 1 << 5,
        Yomogi = 1 << 6,

        [InspectorName("")]
        All = 7,
        [InspectorName("全選択")]
        SET_ALL = ~0,
    }

    private enum ShotMode
    {
        Random,
        Fixed
    }

    [SerializeField, Tooltip("発射モード")] ShotMode shotMode;
    [SerializeField, Tooltip("出す色")] DangoColorChoice colorChoice;
    [SerializeField, Tooltip("団子の射出ポイント")] GameObject spawner = default!;
    [SerializeField, Tooltip("全体")] Transform baseRotate;
    [SerializeField, Tooltip("砲身")] Transform cannonAngle;

    [SerializeField, Tooltip("一度に発射する数"), Min(1)] private int defalutInjectionCount = 1;
    [SerializeField, Tooltip("2つ以上発射するときの間隔"), Min(0)] private int defalutContinueFrame = 0;
    [SerializeField, Tooltip("アニメーション時間"), Min(0)] private int animationFrame = 40;
    [SerializeField, Tooltip("アニメーション終わりから発射までの待機時間"), Min(0)] private int shotWaitFrame = 10;
    [SerializeField, Tooltip("アニメーションまでのクールタイム"), Min(1)] private int animationWaitFrame = 10;

    [SerializeField, Tooltip("射出力"), Min(0.1f)] private float shotPower = 10f;

    [SerializeField, Tooltip("縦方向の可動域")] Vector2 verticalRot;
    [SerializeField, Tooltip("横方向の可動域")] Vector2 horizontalRot;

    [SerializeField] DangoInjectionFixed[] fixedAngles;
    [SerializeField] FloorManager floorManager;

    [SerializeField] bool _canShot = true;

    private DangoPoolManager _poolManager = default!;

    private DangoInjectionFixed _nextLookAngle = new();
    private DangoInjectionFixed _lookedAngle = new();
    private DangoInjectionFixed _firstLookAngle = new();
    private DangoInjectionFixed _interpolatedVec = new();

    private int _injectionCount = default;
    private int _continueFrame = default;
    private int _currentAnimFrame = default;
    private int _shotWaitFrame = default;
    private int _animWaitFrame = default;

    private List<DangoColor> dangoColors = new();

    private Floor _floor;

    interface IState
    {
        public enum E_State
        {
            Injection,
            Animation,
            WaitInjection,
            WaitAnimation,

            Max,

            Unchanged,
        }

        public E_State Init(DangoInjection parent);
        public E_State Update(DangoInjection parent);
    }

    //状態管理
    private IState.E_State _currentState = IState.E_State.Injection;
    private static readonly IState[] states = new IState[(int)IState.E_State.Max]
     {
         new InjectionState(),
         new AnimationState(),
         new WaitInjectionState(),
         new WaitAnimationState(),
     };

    class InjectionState : IState
    {
        public IState.E_State Init(DangoInjection parent)
        {
            if (!parent._canShot) return IState.E_State.WaitInjection;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(DangoInjection parent)
        {
            return parent.Injection() ? IState.E_State.WaitAnimation : IState.E_State.Unchanged;
        }
    }
    class AnimationState : IState
    {
        public IState.E_State Init(DangoInjection parent)
        {
            parent._currentAnimFrame = 0;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(DangoInjection parent)
        {
            return parent.IsSmoothLookRotation() ? IState.E_State.WaitInjection : IState.E_State.Unchanged;
        }
    }
    class WaitInjectionState : IState
    {
        public IState.E_State Init(DangoInjection parent)
        {
            parent._shotWaitFrame = parent.shotWaitFrame;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(DangoInjection parent)
        {
            return --parent._shotWaitFrame == 0 ? IState.E_State.Injection : IState.E_State.Unchanged;
        }
    }
    class WaitAnimationState : IState
    {
        public IState.E_State Init(DangoInjection parent)
        {
            parent._animWaitFrame = parent.animationWaitFrame;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(DangoInjection parent)
        {
            return --parent._animWaitFrame == 0 ? IState.E_State.Animation : IState.E_State.Unchanged;
        }
    }

    private void InitState()
    {
        Logger.Assert(_currentState is >= 0 and < IState.E_State.Max);

        var nextState = states[(int)_currentState].Init(this);

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

    private void Awake()
    {
        Logger.Assert(verticalRot.x < verticalRot.y);
        Logger.Assert(horizontalRot.x < horizontalRot.y);
        Logger.Assert(colorChoice != DangoColorChoice.None);

        //初期化
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
        _firstLookAngle.ba = baseRotate.localEulerAngles.y;
        _firstLookAngle.ca = cannonAngle.localEulerAngles.y;
        NextLook();

        for (int i = 1; i < (int)DangoColorChoice.All; i++)
        {
            if (colorChoice.HasFlag((DangoColorChoice)(1 << i)))
            {
                dangoColors.Add((DangoColor)i);
            }
        }

        InitState();
    }

    private void FixedUpdate()
    {
        UpdateState();
    }

    private bool Injection()
    {
        //方向確定
        baseRotate.localRotation = Quaternion.Euler(baseRotate.localEulerAngles.SetY(_nextLookAngle.ba));
        cannonAngle.localRotation = Quaternion.Euler(cannonAngle.localEulerAngles.SetY(_nextLookAngle.ca));

        //連続打ちのFRAMEを管理
        if (--_continueFrame > 0) return false;

        //団子発射
        if (!ShotDango(dangoColors[UnityEngine.Random.Range(0, dangoColors.Count)])) return false;

        //次の発射地点を決定
        NextLook();

        return true;
    }

    private bool ShotDango(DangoColor color)
    {
        if (dangoColors.Count == 0) return false;

        EnforcementShot(color);

        floorManager.FloorArrays[(int)_floor].AddDangoCount(color);

        return --_injectionCount <= 0;
    }

    public DangoData EnforcementShot(DangoColor color)
    {
        //オブジェクトプールから団子を取り出して設定。
        _poolManager.SetCreateColor(color);
        var dango = _poolManager.DangoPool[(int)color - 1].Get();

        dango.SetDangoColor(color);
        dango.transform.position = spawner.transform.position;
        dango.Rb.AddForce(transform.forward.normalized * shotPower, ForceMode.Impulse);
        dango.SetFloor(_floor);
        dango.SetFloorManager(floorManager);

        _continueFrame = defalutContinueFrame;

        return dango;
    }

    private bool IsSmoothLookRotation()
    {
        var progress = EasingManager.EaseProgress(EaseType.OutBack, ++_currentAnimFrame, animationFrame, 1f, 0);

        cannonAngle.localRotation = Quaternion.Euler(cannonAngle.localEulerAngles.SetY(_lookedAngle.ca + (_interpolatedVec.ca * progress)));
        baseRotate.localRotation = Quaternion.Euler(baseRotate.localEulerAngles.SetY(_lookedAngle.ba + (_interpolatedVec.ba * progress)));

        return _currentAnimFrame == animationFrame;
    }

    private void NextLook()
    {
        //現在の位置を入力
        _lookedAngle.ba = baseRotate.localEulerAngles.y;
        _lookedAngle.ca = cannonAngle.localEulerAngles.y;

        //初期化
        _continueFrame = 0;
        _injectionCount = defalutInjectionCount;

        //次の位置の抽選
        if (shotMode == ShotMode.Random)
        {
            _nextLookAngle.ba = _firstLookAngle.ba + UnityEngine.Random.Range(horizontalRot.x, horizontalRot.y);
            _nextLookAngle.ca = _firstLookAngle.ca + UnityEngine.Random.Range(verticalRot.x, verticalRot.y);
        }
        else
        {
            if (fixedAngles.Length == 0) return;
            int index = UnityEngine.Random.Range(0, fixedAngles.Length);
            _nextLookAngle.ba = _firstLookAngle.ba + fixedAngles[index].BaseAngle;
            _nextLookAngle.ca = _firstLookAngle.ca + fixedAngles[index].CannonAngle;
        }

        //補間値を求める
        _interpolatedVec.ba = _nextLookAngle.ba - _lookedAngle.ba;
        _interpolatedVec.ca = _nextLookAngle.ca - _lookedAngle.ca;
    }

    private float Around(float val)
    {
        if (val > 180f) return val - 360f;
        if (val < -180f) return val + 360f;

        return val;
    }

    public void SalvationShot(DangoColor color)
    {
        EnforcementShot(color);
    }

    public void SetFloor(Floor floor) => _floor = floor;

    public void SetCanShot(bool canShot) => _canShot = canShot;
    public bool CanShot => _canShot;

    [Serializable]
    class DangoInjectionFixed
    {
        [SerializeField, Range(0, 360f)] float _baseAngle;
        [SerializeField, Range(0, 360f)] float _cannonAngle;

        [NonSerialized] public float ba = 0;
        [NonSerialized] public float ca = 0;

        public float BaseAngle => _baseAngle;
        public float CannonAngle => _cannonAngle;
        public void SetBaseAngle(float value) => _baseAngle = value;
        public void SetCannonAngle(float value) => _cannonAngle = value;

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DangoInjection))]
    [CanEditMultipleObjects]
    public class DangoInjectionEditor : Editor
    {
        private DangoInjection dangoInjection;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            dangoInjection = target as DangoInjection;
            base.OnInspectorGUI();

            if (dangoInjection.fixedAngles.Length == 0) return;

            if (GUILayout.Button("Copy"))
            {
                dangoInjection.fixedAngles[^1].ba = dangoInjection.fixedAngles[^1].BaseAngle;
                dangoInjection.fixedAngles[^1].ca = dangoInjection.fixedAngles[^1].CannonAngle;
                dangoInjection.fixedAngles[^1].SetBaseAngle(dangoInjection.transform.localEulerAngles.y);
                dangoInjection.fixedAngles[^1].SetCannonAngle(dangoInjection.transform.localEulerAngles.x);
            }
            if (GUILayout.Button("Redo"))
            {
                dangoInjection.fixedAngles[^1].SetBaseAngle(dangoInjection.fixedAngles[^1].ba);
                dangoInjection.fixedAngles[^1].SetCannonAngle(dangoInjection.fixedAngles[^1].ca);
            }
        }

    }
#endif
}