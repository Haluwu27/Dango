using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Tooltip("追従したいターゲット")] Transform target = default!;
    [SerializeField] float MinAngle;
    [SerializeField] float MaxAngle;

    ////[SerializeField, Range(0.01f, 1f),Tooltip("カメラの追従度")]
    //private float smoothSpeed = 0.125f;

    //[SerializeField,Tooltip("ターゲットからのカメラの位置")]
    //private Vector3 offset = new Vector3(0f, 1f, -10f);

    //private Vector3 velocity = Vector3.zero;


    private Vector3 targetPos = Vector3.zero;

    private PlayerData _playerData;

    private float roteYSpeed = -10f;
    private void Start()
    {
        _playerData = target.GetComponent<PlayerData>();
        transform.parent = null;//動くものに乗るとそれに追従しだすから親子関係を無くす
    }

    //Playerが動いたあとに実行するため、LateUpdateで行う。
    private void LateUpdate()
    {
        //カメラの位置を変更する
        transform.position += target.position - targetPos;
        targetPos = target.position;

        //カメラをroteAxis.xに合わせて回転させる。
        transform.RotateAround(target.position, Vector3.up, _playerData.GetRoteAxis().x * Time.deltaTime);

        //X軸回転の角度を所得
        float currentYAngle = transform.eulerAngles.x;
        //X軸が0～360の値しか返さないので調整
        if (currentYAngle > 180)
        {
            currentYAngle -= 360;
        }

        //縦軸の制限
        if ((currentYAngle >= MinAngle && _playerData.GetRoteAxis().y > 0) || (currentYAngle <= MaxAngle && _playerData.GetRoteAxis().y < 0))
        {
            transform.RotateAround(target.position, transform.right, _playerData.GetRoteAxis().y * Time.deltaTime * roteYSpeed);
        }

        //カメラの位置が決定してからプレイヤーの向きを決めることで、カクつきをなくす。
        target.transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

    }
}
