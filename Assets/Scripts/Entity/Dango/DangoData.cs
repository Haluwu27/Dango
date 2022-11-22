using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 団子に関するマネージャークラス
/// </summary>
public class DangoData : MonoBehaviour
{
    //消去時間（10秒）
    const int DELETE_FRAME = 500;
    const float DELETE_MIN_SPEED = 0.1f;

    //消去時間
    int _frameCount = DELETE_FRAME;

    //団子が持つ色データ
    DangoColor _color = DangoColor.None;

    FloorManager.Floor _floor;
    FloorManager _floorManager;

    [SerializeField] Renderer _rend;
    [SerializeField] Rigidbody _rigidbody;

    public Renderer Rend => _rend;
    public Rigidbody Rb => _rigidbody;

    //オブジェクトプールマネージャー
    DangoPoolManager _poolManager;

    private void Awake()
    {
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
    }

    private void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude < 10)
        {
            //指定したスピードから現在の速度を引いて加速力を求める
            float currentSpeed = 10 - _rigidbody.velocity.magnitude;
            //調整された加速力で力を加える
            _rigidbody.AddForce(transform.forward * currentSpeed);
        }

        //ここで回転処理でも作る
        if (_rigidbody.velocity.magnitude < 0.01f) transform.Rotate(0, Random.Range(90f, 270f), 0);

        ReleaseDango();
    }

    private void ReleaseDango()
    {
        if (Rend == null || Rb == null) return;

        //もし団子がカメラ外で、速度が一定値以下であれば
        if (!Rend.isVisible && Rb.velocity.magnitude < DELETE_MIN_SPEED)
        {
            if (--_frameCount <= 0) ReleaseDangoPool();
        }
        else
        {
            _frameCount = DELETE_FRAME;
        }
    }

    public void ReleaseDangoPool()
    {
        //部屋の団子総数をへらす
        _floorManager.FloorArrays[(int)_floor].RemoveDangoCount(1);

        _poolManager.DangoPool[(int)_color - 1].Release(this);
    }

    public DangoColor GetDangoColor() => _color;

    public void SetDangoColor(DangoColor type)
    {
        _color = type;
    }

    public FloorManager.Floor Floor => _floor;
    public void SetFloor(FloorManager.Floor floor) => _floor = floor;
    public void SetFloorManager(FloorManager floorManager) => _floorManager = floorManager;
}
