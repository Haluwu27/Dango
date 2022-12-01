using Cysharp.Threading.Tasks;
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
    [SerializeField] IngameUIManager _ingameUIManager;
    [SerializeField] FusumaManager _fusumaManager;
    [SerializeField] PlayerData _playerData;
    bool _gameSucceed;

    #region statePattern
    interface IState
    {
        public enum E_State
        {
            Control = 0,
            GameOver = 1,
            Succeed = 2,

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
        new GameSucceedState(),
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
                if (parent._gameSucceed) return IState.E_State.Succeed;
            }

            return IState.E_State.Unchanged;
        }
    }
    class GameOverState : IState
    {
        public IState.E_State Initialize(GameManager parent)
        {
            parent.GameOver();
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
    class GameSucceedState : IState
    {
        public IState.E_State Initialize(GameManager parent)
        {
            parent.OnSucceed();
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

    private void GameOver()
    {
        InputSystemManager.Instance.onPausePerformed -= OnPause;
        SceneSystem.Instance.Load(SceneSystem.Scenes.GameOver);
    }

    private async void OnSucceed()
    {
        await _ingameUIManager.EraseUIs();
        await UniTask.Delay(2000);
        await _ingameUIManager.TextAnimation();
        await UniTask.Delay(2500);

        InputSystemManager.Instance.onPausePerformed -= OnPause;

        SceneSystem.Scenes nextScene = SceneSystem.Scenes.Success;

        //チュートリアルをクリアした際は別処理
        if (SceneSystem.Instance.CurrentIngameScene == SceneSystem.Scenes.Tutorial)
        {
            //セーブデータにチュートリアルをクリアしたことを記録
            DataManager.saveData.completedTutorial = true;

            Logger.Assert(_fusumaManager != null);

            await _fusumaManager.UniTaskClose();

            SceneSystem.Instance.UnLoad(SceneSystem.Instance.CurrentIngameScene, true);
            InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");

            //クリア画面には移行せず、メニューに戻る
            nextScene = SceneSystem.Scenes.Menu;
        }

        SceneSystem.Instance.Load(nextScene);
    }

    public void SetGameSucceed() => _gameSucceed = true;
}
