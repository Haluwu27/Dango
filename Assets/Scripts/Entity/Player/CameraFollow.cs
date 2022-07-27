using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    enum State
    {
        normal,//プレイヤーの後をラグなくついていきます
        Lerp,//プレイヤーの回転、移動に滑らかについていきます
        LerpMove,//プレイヤーの移動のみに滑らかについていきます
    }

    #region メンバ
    [SerializeField, Tooltip("追従したいターゲット")] Transform target = default!;
    [SerializeField] float MinAngle;
    [SerializeField] float MaxAngle;
    [SerializeField, Tooltip("どのくらいの時間で追いつくか"), Min(0)] float ratio = 1;

    //[SerializeField, Range(0.01f, 1f), Tooltip("カメラの追従度")]
    //private float smoothSpeed = 0.125f;

    [SerializeField] LayerMask wallLayer;//マップのレイヤーマスク

    private Vector3 _prebTargetPos = Vector3.zero;
    private GameObject _terminus = null;

    private PlayerData _playerData;

    private float _roteYSpeed = -100f;

    //壁にぶつかったときのもの
    Vector3 _wallHitPos;//壁にぶつかった際の座標
    RaycastHit _hit;//壁を所得するRay

    //カメラ静止中に行う処理
    CameraIsStaying _camIsStaying = null;

    [SerializeField] State state;

    Camera _cam;
    [SerializeField] GameObject eatCamPos;

    const float DEFAULT_CAMERA_VIEW = 60f;
    const float CAMERA_REMOVETIME = 0.3f;

    Vector3 _firstCameraPos;
    Quaternion _firstCameraRot;

    #endregion

    private void Start()
    {
        _playerData = target.GetComponent<PlayerData>();
        transform.parent = null;//動くものに乗るとそれに追従しだすから親子関係を無くす
        _terminus = new GameObject("cameraTermiusObject");
        _terminus.transform.position = transform.position;
        _camIsStaying = new(gameObject, _terminus, target);
        _cam = GetComponent<Camera>();

        _prebTargetPos = target.position;
    }

    private void Update()
    {
        CameraRotate();
        RotateToLookRot();
    }

    //Playerが動いたあとに実行するため、LateUpdateで行う。
    private void LateUpdate()
    {
        CameraSmoothMove();

        _playerData.SetCameraForward(transform.forward);
    }

    private void CameraSmoothMove()
    {
        Vector3 currentTargetPos = target.position;

        //カメラの目標地点を変更する
        _terminus.transform.position += currentTargetPos - _prebTargetPos;

        if (WallHitCheck())
        {
            //当たった場所に飛ばすとカメラが壁の中に埋まるので調整。
            _wallHitPos = _hit.point + (currentTargetPos - _terminus.transform.position).normalized;
            transform.position = _wallHitPos;
        }
        else//カメラの移動
        {
            transform.position = state switch
            {
                State.normal => _terminus.transform.position,
                _ => Vector3.Lerp(transform.position, _terminus.transform.position, Time.deltaTime * ratio),
            };
        }

        _prebTargetPos = currentTargetPos;
    }

    private void RotateToLookRot()
    {
        if (_playerData.GetRoteAxis().magnitude > 0.1f || _playerData.GetMoveAxis().magnitude > 0.1f)
        {
            _camIsStaying.Reset();
            return;
        }

        _camIsStaying.Update();
    }

    private void CameraRotate()
    {
        //X軸回転の角度を所得
        float currentYAngle = transform.eulerAngles.x;

        //X軸が0～360の値しか返さないので調整
        if (currentYAngle > 180)
        {
            currentYAngle -= 360;
        }

        switch (state)
        {
            case State.normal:
                Rote(_terminus, currentYAngle);
                transform.rotation = _terminus.transform.rotation;
                break;
            case State.Lerp:
                Rote(_terminus, currentYAngle);
                transform.rotation = _terminus.transform.rotation;
                break;
            case State.LerpMove:
                Rote(gameObject, currentYAngle);
                Rote(_terminus, currentYAngle);
                break;
        }

    }

    private void Rote(GameObject obj, float a)
    {
        //カメラをroteAxis.xに合わせて回転させる。
        obj.transform.RotateAround(target.position, Vector3.up, _playerData.GetRoteAxis().x * Time.deltaTime);

        //縦軸の制限
        if ((a >= MinAngle && _playerData.GetRoteAxis().y > 0) || (a <= MaxAngle && _playerData.GetRoteAxis().y < 0))
        {
            obj.transform.RotateAround(target.position, obj.transform.right, _playerData.GetRoteAxis().y * Time.deltaTime * _roteYSpeed);
        }
    }

    private bool WallHitCheck()
    {
        return Physics.Raycast(target.position, _terminus.transform.position - target.position, out _hit, Vector3.Distance(_prebTargetPos, _terminus.transform.position), wallLayer, QueryTriggerInteraction.Ignore);
    }

    public void OnChargeCameraMoving()
    {
        _cam.fieldOfView -= 10f * Time.deltaTime;
    }

    public IEnumerator ResetCameraView()
    {
        float view = _cam.fieldOfView;
        float hokann = DEFAULT_CAMERA_VIEW - view;
        while (_cam.fieldOfView <= DEFAULT_CAMERA_VIEW)
        {
            _cam.fieldOfView += (hokann / CAMERA_REMOVETIME) * Time.deltaTime;
            yield return null;
        }
        _cam.fieldOfView = DEFAULT_CAMERA_VIEW;
    }

    public void EatStateCamera()
    {
        //移動開始前の初期位置を保存する
        _firstCameraPos = transform.position;
        _firstCameraRot = transform.rotation;

        //player to camera vec
        Vector3 pToC = transform.position - _playerData.transform.position;

        //player to target vec
        Vector3 pToT = eatCamPos.transform.position - _playerData.transform.position;

        //カメラの瞬間移動
        transform.position = eatCamPos.transform.position;
        transform.rotation = Quaternion.Euler(new(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + RotateAngle(pToC, pToT), transform.rotation.eulerAngles.z));
        _terminus.transform.position = eatCamPos.transform.position;
        _terminus.transform.rotation = Quaternion.Euler(new(_terminus.transform.rotation.eulerAngles.x, _terminus.transform.rotation.eulerAngles.y + RotateAngle(pToC, pToT), _terminus.transform.rotation.eulerAngles.z));

        //カメラをもとに戻す処理（修正必要）
        Invoke("RemoveCamera", 0.3f);
    }

    private float RotateAngle(Vector3 from, Vector3 to)
    {
        //法線N
        Vector3 n = Vector3.up;

        Vector3 planeFrom = Vector3.ProjectOnPlane(from, n);
        Vector3 planeTo = Vector3.ProjectOnPlane(to, n);

        return Vector3.SignedAngle(planeFrom, planeTo, n);
    }

    private void RemoveCamera()
    {
        transform.position = _firstCameraPos;
        transform.rotation = _firstCameraRot;
    }
}