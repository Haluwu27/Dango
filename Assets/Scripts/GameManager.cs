using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// メインゲームの進行に関わるもののみ定義
/// </summary>
[RequireComponent(typeof(GameStartManager))]
internal class GameManager : MonoBehaviour
{
    public static float GameScore { get; set; } = 0;

    [SerializeField] PlayerData _playerData;

    #region statePattern
    interface IState
    {
        public enum E_State
        {
            Control = 0,
            GameOver = 1,

            Max,

            Unchanged,
        }

        E_State Initialize(GameManager parent);
        E_State Update(GameManager parent);
        E_State FixedUpdate(GameManager parent);
    }

    //状態管理
    private IState.E_State _currentState = IState.E_State.Control;
    private static readonly IState[] states = new IState[(int)IState.E_State.Max]
     {
        new ControlState(),
        new GameOverState(),
     };

    class ControlState : IState
    {
        public IState.E_State Initialize(GameManager parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(GameManager parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(GameManager parent)
        {
            if (parent._playerData != null)
            {
                if (parent._playerData.GetSatiety() <= 0) return IState.E_State.GameOver;
            }

            return IState.E_State.Unchanged;
        }
    }
    class GameOverState : IState
    {
        public IState.E_State Initialize(GameManager parent)
        {
            parent.FinishGame();
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(GameManager parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(GameManager parent)
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

    private void Start()
    {
        InputSystemManager.Instance.onPausePerformed += OnPause;
    }

    private void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        FixedUpdateState();
    }

    private void OnPause()
    {
        SceneSystem.Instance.Load(SceneSystem.Scenes.InGamePause);
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");
    }

    private void FinishGame()
    {
        //int madeCount = 0;
        //var posRoles = DangoRole.instance.GetPosRoles();

        //foreach (var posRole in posRoles)
        //{
        //    if (posRole.GetMadeCount() > 0)
        //    {
        //        madeCount++;
        //    }
        //}

        //Logger.Log("満足度：" + GameScore * madeCount);

        InputSystemManager.Instance.onPausePerformed -= OnPause;
        SceneSystem.Instance.Load(SceneSystem.Scenes.GameOver);
    }
}
