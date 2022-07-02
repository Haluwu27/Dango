using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの進行に関わるもののみ定義
/// </summary>
internal class GameManager : MonoBehaviour
{
    public static float GameScore { get; set; } = 0;

    QuestManager _questManager = new();
    PlayerData _playerData;

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


    private void Awake()
    {
        //マウスカーソルのやつ。
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //最初のクエストを仮置き。
        _questManager.ChangeQuest(_questManager.Creater.CreateQuestCreateRole(DangoRole.POSROLE_DIVIDED_INTO_TWO, 1, "役「二分割」を1個作れ！"),
                               _questManager.Creater.CreateQuestIncludeColor(DangoColor.Red, 3, "赤色を含めて役を3個作れ！"));
    }

    private void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        FixedUpdateState();
    }

    private void FinishGame()
    {
        int madeCount = 0;
        var posRoles = DangoRole.instance.GetPosRoles();

        foreach (var posRole in posRoles)
        {
            if (posRole.GetMadeCount() > 0)
            {
                madeCount++;
            }
        }

        Logger.Log("満足度：" + GameScore * madeCount);
    }

    public void OnJoinPlayer()
    {
        _playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    //複数プレイヤーの処理（棄却されたもの）
    //static private int PlayerNum = 1;
    //static public Player1[] player { get; set; } = new Player1[2];
    //public static void SetPlayer(Player1 obj)
    //{

    //    for (int i = 0; i < player.Length; i++)
    //    {
    //        if (player[i] == null)
    //        {
    //            player[i] = obj;
    //            break;
    //        }
    //    }
    //}

    //public void AddPlayer(GameObject obj)
    //{
    //    //Camera cam = obj.GetComponentInChildren<Camera>();
    //    //if (PlayerNum == 1)
    //    //{
    //    //    cam.rect = new Rect(0, 0, 1f, 0.5f);
    //    //    Logger.Log("cam.rectを変更");

    //    //}
    //    //else if (PlayerNum == 2)
    //    //{
    //    //    cam.rect = new Rect(0, 0.5f, 1f, 0.5f);
    //    //    Logger.Log("cam.rectを変更");
    //    //    return;
    //    //}
    //    //    PlayerNum++;
    //}
}
