using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIsStaying
{
    interface IState
    {
        public enum E_State
        {
            Control,
            Stay,

            Max,

            Unchanged,
        }
        E_State Initialize(CameraIsStaying parent);
        E_State Update(CameraIsStaying parent);
    }

    //状態管理
    private IState.E_State _currentState = IState.E_State.Stay;
    private static readonly IState[] states = new IState[(int)IState.E_State.Max]
    {
        new ControlState(),
        new StayState(),
    };

    class ControlState : IState
    {
        public IState.E_State Initialize(CameraIsStaying parent)
        {
            parent._currentTime = 0;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(CameraIsStaying parent)
        {
            return parent.LookPlayerBack() ? IState.E_State.Stay : IState.E_State.Unchanged;
        }
    }
    class StayState : IState
    {
        public IState.E_State Initialize(CameraIsStaying parent)
        {
            parent.Reset();
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(CameraIsStaying parent)
        {
            return parent.IsStaying() ? IState.E_State.Unchanged : IState.E_State.Control;
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

    //待機秒数
    private const float MAX_STAY_TIME = 2f;

    float _currentTime = MAX_STAY_TIME;

    GameObject _playerCamera;
    GameObject _terminus;
    Transform _player;

    public CameraIsStaying(GameObject playerCamera, GameObject terminus, Transform player)
    {
        _playerCamera = playerCamera;
        _terminus = terminus;
        _player = player;
    }

    public void Update()
    {
        UpdateState();
    }

    public void Reset()
    {
        _currentTime = MAX_STAY_TIME;
    }

    private bool IsStaying()
    {
        _currentTime -= Time.deltaTime;

        return _currentTime > 0;
    }

    private bool LookPlayerBack()
    {
        float x = _currentTime += Time.deltaTime;

        //ここで移動量と時間の計算すべき？
        x = Mathf.Clamp01(x);

        float y = x < 0.5f ? 12f * x * x : 12f * Mathf.Abs(x - 1f) * Mathf.Abs(x - 1f);
        _playerCamera.transform.RotateAround(_player.position, Vector3.up, y);
        _terminus.transform.RotateAround(_player.position, Vector3.up, y);
        return (_player.forward.x - _playerCamera.transform.forward.x) * (_player.forward.x - _playerCamera.transform.forward.x) + (_player.forward.z - _playerCamera.transform.forward.z) * (_player.forward.z - _playerCamera.transform.forward.z) < 0.01f;
    }
}
