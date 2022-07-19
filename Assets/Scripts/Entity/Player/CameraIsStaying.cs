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

            return parent.InitControl() ? IState.E_State.Unchanged : IState.E_State.Stay;
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
            parent._currentTime = MAX_STAY_TIME;
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

        Logger.Log(_currentState);

        if (nextState != IState.E_State.Unchanged)
        {
            //次に遷移
            _currentState = nextState;
            InitState();
        }
    }


    //1sに移動する角度の定数
    const float MOVE_ANGLE = 45f;

    //待機秒数定数
    const float MAX_STAY_TIME = 2f;

    float _currentTime = MAX_STAY_TIME;

    GameObject _playerCamera;
    GameObject _terminus;
    Transform _playerTrans;

    Vector3 _playerPos;
    Vector3 _cameraPos;
    Vector3 _targetPos;
    float _distancePtoC;
    float _distanceCtoT;
    float _angle;
    bool _isLeft;

    public CameraIsStaying(GameObject playerCamera, GameObject terminus, Transform playerTrans)
    {
        _playerCamera = playerCamera;
        _terminus = terminus;
        _playerTrans = playerTrans;
    }

    public void Update()
    {
        UpdateState();
    }

    public void Reset()
    {
        _currentTime = MAX_STAY_TIME;
        _currentState = IState.E_State.Stay;
    }

    private bool IsStaying()
    {
        _currentTime -= Time.deltaTime;

        return _currentTime > 0;
    }

    private bool InitControl()
    {
        //Y座標を無視したプレイヤーの座標
        _playerPos = new(_playerTrans.position.x, _playerCamera.transform.position.y, _playerTrans.position.z);

        //カメラ座標
        _cameraPos = _playerCamera.transform.position;

        //playerPosとカメラ座標の距離（ベクトルの大きさ）
        _distancePtoC = Vector3.Distance(_cameraPos, _playerPos);

        //Playerの左側にカメラがあるか外積のyを取って判定
        _isLeft = Vector3.Cross(_playerTrans.forward, _cameraPos - _playerPos).y < 0 ? true : false;

        //正面の逆ベクトルに距離をかけてプレイヤーの座標（Y無視）を足したもの
        //これが目標地点
        _targetPos = _playerPos + (-_playerTrans.forward) * _distancePtoC;

        //余弦定理で角度を求める
        _distanceCtoT = Vector3.Distance(_targetPos, _cameraPos);
        float angle = Mathf.Acos((_distancePtoC * _distancePtoC + _distancePtoC * _distancePtoC - _distanceCtoT * _distanceCtoT) / (2 * _distancePtoC * _distancePtoC));

        //弧度法から度数法に変換
        _angle = angle * 180f / Mathf.PI;

        //ゼロ除算ケア（そもそも到達時間が0なら移動しないということ）
        if (_angle == 0) return false;

        return true;
    }

    private bool LookPlayerBack()
    {
        //角度を定数で割る。定数値を1としたとき何秒で目標地点に到達するかって話
        float arrivalTime = _angle / MOVE_ANGLE;
        _currentTime += Time.deltaTime;

        //現在の経過時間を足す
        //経過時間を0-1で表したいから、さっき求めた何秒で到達するかで割る
        float x = _currentTime;

        x /= arrivalTime;

        float y = 0;
        if (x < 0.5f)
        {
            y = 4f * x * x * x;
        }
        else if (x is >= 0.5f and <= 1f)
        {
            x = Mathf.Abs(x - 1);
            y = 4f * x * x * x;
        }

        _playerCamera.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -y : y) * 360f * Time.deltaTime);
        _terminus.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -y : y) * 360f * Time.deltaTime);
        //_playerCamera.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -MOVE_ANGLE : MOVE_ANGLE) * Time.deltaTime);
        //_terminus.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -MOVE_ANGLE : MOVE_ANGLE) * Time.deltaTime);

        return _currentTime >= arrivalTime;
    }
}
