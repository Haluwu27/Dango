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
    [SerializeField, Tooltip("追従したいターゲット")] Transform target = default!;
    [SerializeField] float MinAngle;
    [SerializeField] float MaxAngle;
    [SerializeField, Tooltip("どのくらいの時間で追いつくか")] float ratio = 1;

    [SerializeField, Range(0.01f, 1f), Tooltip("カメラの追従度")]
    private float smoothSpeed = 0.125f;

    [SerializeField] GameObject TerminusObjct;

    [SerializeField] LayerMask wallLayer;//マップのレイヤーマスク

    //[SerializeField, Tooltip("ターゲットからのカメラの位置")]
    //private Vector3 offset = new Vector3(0f, 1f, -10f);

    //private Vector3 velocity = Vector3.zero;


    private Vector3 targetPos = Vector3.zero;
    private GameObject terminus;

    private PlayerData _playerData;

    private float roteYSpeed = -10f;

    Vector3 wallHitPos;//壁にぶつかった際の座標
    RaycastHit hit;//壁を所得するRay

    [SerializeField] State state;
    private void Start()
    {
        _playerData = target.GetComponent<PlayerData>();
        transform.parent = null;//動くものに乗るとそれに追従しだすから親子関係を無くす
        terminus = Instantiate(TerminusObjct);
        terminus.transform.position = transform.position;
    }

    //Playerが動いたあとに実行するため、LateUpdateで行う。
    private void LateUpdate()
    {

        _playerData = target.GetComponent<PlayerData>();

        //カメラの位置を変更する
        if(target.position - targetPos != target.position)
        terminus.transform.position += target.position - targetPos;

        //X軸回転の角度を所得
        float currentYAngle = transform.eulerAngles.x;

        //X軸が0～360の値しか返さないので調整
        if (currentYAngle > 180)
        {
            currentYAngle -= 360;
        }

        if (WallHitCheck())
        {
            //switch (state)
            //{

            //    case State.normal:
                    transform.position = wallHitPos;//Lerpｗｐ使うと移動の際に壁の中が見えるため固定
            //        break;
            //    case State.Lerp:
            //        transform.position = Vector3.Lerp(transform.position, wallHitPos, ratio*Time.deltaTime);
            //        break;
            //    case State.LerpMove:
            //        transform.position = Vector3.Lerp(transform.position, wallHitPos, ratio * Time.deltaTime);
            //        break;
            //}
        }
        else//カメラの移動
        {
            switch(state)
            {
                case State.normal:
                    transform.position = terminus.transform.position;
                    break;
                case State.Lerp:
                    transform.position = Vector3.Lerp(transform.position, terminus.transform.position, ratio * Time.deltaTime);
                    break;
                case State.LerpMove:
                    transform.position = Vector3.Lerp(transform.position, terminus.transform.position, ratio * Time.deltaTime);
                    break;
            }
        }

        switch (state)
        {

            case State.normal:
                Rote(terminus, currentYAngle);
                //transform.position = terminus.transform.position;
                transform.rotation = terminus.transform.rotation;
                break;
            case State.Lerp:
                Rote(terminus, currentYAngle);
                transform.rotation = terminus.transform.rotation;
                //transform.position = Vector3.Lerp(transform.position, terminus.transform.position, ratio);
                break;
            case State.LerpMove:
                //transform.position = Vector3.Lerp(transform.position, terminus.transform.position, ratio);
                Rote(gameObject, currentYAngle);
                Rote(terminus, currentYAngle);
                break;
        }

        //カメラの位置が決定してからプレイヤーの向きを決めることで、カクつきをなくす。
        target.transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        targetPos = target.position;

    }
    private void Rote(GameObject obj, float a)
    {
        //カメラをroteAxis.xに合わせて回転させる。
        obj.transform.RotateAround(target.position, Vector3.up, _playerData.GetRoteAxis().x * Time.deltaTime);

        //縦軸の制限
        if ((a >= MinAngle && _playerData.GetRoteAxis().y > 0) || (a <= MaxAngle && _playerData.GetRoteAxis().y < 0))
        {
            obj.transform.RotateAround(target.position, obj.transform.right, _playerData.GetRoteAxis().y * Time.deltaTime * roteYSpeed);
        }

    }
    protected bool WallHitCheck()
    {
        if (Physics.Raycast(targetPos, terminus.transform.position - targetPos, out hit, Vector3.Distance(targetPos, terminus.transform.position), wallLayer, QueryTriggerInteraction.Ignore))
        {
            wallHitPos = hit.point * 0.95f;//当たった場所に飛ばすとカメラが壁の中に埋まるので調整。
            Logger.Log(hit.point);
            return true;
        }
        else
        {
            return false;
        }
    }
}

