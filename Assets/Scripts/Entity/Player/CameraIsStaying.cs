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
            parent._timeX = parent._timeY = 0;

            return (parent.InitControlX() && parent.InitControlY()) ? IState.E_State.Unchanged : IState.E_State.Stay;
        }
        public IState.E_State Update(CameraIsStaying parent)
        {
            parent.LookPlayerBackH();
            

            return parent.LookPlayerBackV() ? IState.E_State.Stay : IState.E_State.Unchanged;
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

    static readonly Vector3 OFFSET = new(0, 2f, -10f);

    //1sに移動する角度の定数
    const float MOVE_ANGLE = 45f;

    //待機秒数定数
    const float MAX_STAY_TIME = 2f;

    float _currentTime = MAX_STAY_TIME;

    GameObject _playerCamera;
    GameObject _terminus;
    Transform _playerTrans;

    float _arrivalTime;
    float _arrivalX;
    float _arrivalY;

    float _angleX;
    float _angleY;
    bool _isLeft;
    bool _isUp;

    float _timeX;
    float _timeY;

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

    private bool InitControlX()
    {
        //Y座標を無視したプレイヤーの座標
        Vector3 playerPos = new(_playerTrans.position.x, _playerCamera.transform.position.y, _playerTrans.position.z);

        //カメラ座標
        Vector3 cameraPos = _playerCamera.transform.position;

        //playerPosとカメラ座標の距離（ベクトルの大きさ）
        float distancePtoC = Vector3.Distance(cameraPos, playerPos);

        //Player->Cameraのベクトル
        Vector3 pcVec = cameraPos - playerPos;

        //Playerの左側にカメラがあるか外積のyを取って判定
        _isLeft = Vector3.Cross(_playerTrans.forward, pcVec).y < 0 ? true : false;

        //正面の逆ベクトルに距離をかけてプレイヤーの座標（Y無視）を足したもの
        //これが目標地点
        Vector3 targetPos = playerPos + (-_playerTrans.forward) * distancePtoC;

        //余弦定理から角度を求める
        float distanceCtoT = Vector3.Distance(targetPos, cameraPos);
        float angleX = Mathf.Acos((distancePtoC * distancePtoC + distancePtoC * distancePtoC - distanceCtoT * distanceCtoT) / (2 * distancePtoC * distancePtoC));

        //弧度法から度数法に変換
        _angleX = angleX * 180f / Mathf.PI;

        //ゼロ除算ケア（そもそも到達時間が0なら移動しないということ）
        if (_angleX == 0) return false;

        float aTime = _angleX / MOVE_ANGLE;
        _arrivalTime = Mathf.Max(aTime, _arrivalTime);
        _arrivalX = aTime / _arrivalTime;

        return true;
    }
    private bool InitControlY()
    {
        //プレイヤーの座標
        Vector3 playerPos = _playerTrans.position;

        //カメラ座標
        Vector3 cameraPos = _playerCamera.transform.position;

        //カメラ方向に地面と並行（プレイヤーの角度）なベクトル
        Vector3 cameraVec = new(_playerCamera.transform.position.x, _playerTrans.position.y, _playerCamera.transform.position.z);

        //Player->Cameraのベクトル
        Vector3 pcVec = cameraPos - playerPos;
        Vector3 pcVecAtGround = cameraVec - playerPos;

        //Playerの左側にカメラがあるか外積のyを取って判定
        _isUp = Vector3.Cross(_playerTrans.forward, pcVec).x < 0 ? true : false;

        float currentAngle = Mathf.Acos(Vector3.Dot(pcVec, pcVecAtGround) / (pcVec.magnitude * pcVecAtGround.magnitude));
        float cAngle = currentAngle * 180f / Mathf.PI;

        Vector3 v = new(playerPos.x, playerPos.y, playerPos.z + OFFSET.z);
        float targetAngle = Mathf.Acos(Vector3.Dot(v, OFFSET) / (v.magnitude * OFFSET.magnitude));
        float tAngle = targetAngle * 180f / Mathf.PI;

        _angleY = (cAngle - tAngle);

        //ゼロ除算ケア（そもそも到達時間が0なら移動しないということ）
        if (_angleY == 0) return false;

        float aTime = _angleY / MOVE_ANGLE;

        _arrivalTime = Mathf.Max(aTime, _arrivalTime);
        _arrivalY = aTime / _arrivalTime;

        return true;
    }

    private bool LookPlayerBackV()
    {
        //現在の経過時間を足す
        _timeY += Time.deltaTime;

        //経過時間を0-1で表したいから、さっき求めた何秒で到達するかで割る
        float x = _timeY;
        x /= _arrivalTime;

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

        //積分した値（上の関数の面積）→これが平均の速度になる。→平均の速度がMOVE_ANGLEになればよい。
        float dx = (0.5f * 0.5f * 0.5f * 0.5f) * 2f;

        _playerCamera.transform.RotateAround(_playerTrans.position, _playerCamera.transform.right.normalized, (_isUp ? -y : y) * MOVE_ANGLE * _arrivalY / dx * Time.deltaTime);
        _terminus.transform.RotateAround(_playerTrans.position, _playerCamera.transform.right.normalized, (_isUp ? -y : y) * MOVE_ANGLE * _arrivalY / dx * Time.deltaTime);

        return _timeY >= _arrivalTime;
    }

    private bool LookPlayerBackH()
    {
        //現在の経過時間を足す
        _timeX += Time.deltaTime;

        //経過時間を0-1で表したいから、さっき求めた何秒で到達するかで割る
        float x = _timeX;
        x /= _arrivalTime;

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

        //積分した値（上の関数の面積）→これが平均の速度になる。→平均の速度がMOVE_ANGLEになればよい。
        float dx = (0.5f * 0.5f * 0.5f * 0.5f) * 2f;

        _playerCamera.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -y : y) * MOVE_ANGLE * _arrivalX / dx * Time.deltaTime);
        _terminus.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -y : y) * MOVE_ANGLE * _arrivalX / dx * Time.deltaTime);

        return _timeX >= _arrivalTime;
    }
}
