using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("追従したいターゲット")]
    public Transform target = default!;

    ////[SerializeField, Range(0.01f, 1f),Tooltip("カメラの追従度")]
    //private float smoothSpeed = 0.125f;

    //[SerializeField,Tooltip("ターゲットからのカメラの位置")]
    //private Vector3 offset = new Vector3(0f, 1f, -10f);

    //private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;
    private Player1 P1;

    private void Start()
    {
        P1 = target.GetComponent<Player1>();
    }

    //Playerが動いたあとに実行するため、LateUpdateで行う。
    private void LateUpdate()
    {
        //カメラの位置を変更する
        transform.position += target.position - targetPos;
        targetPos = target.position;

        //カメラをroteAxis.xに合わせて回転させる。
        transform.RotateAround(target.position, Vector3.up, P1.GetRoteAxis().x * Time.deltaTime);
        
        //カメラの位置が決定してからプレイヤーの向きを決めることで、カクつきをなくす。
        target.transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
    }
}
