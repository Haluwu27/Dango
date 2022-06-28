using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 団子に関するマネージャークラス
/// </summary>
public class DangoManager : MonoBehaviour
{
    //消去時間（10秒）
    const int DELETE_FRAME = 500;
    const float DELETE_MIN_SPEED = 0.1f;

    //消去時間
    int _frameCount = DELETE_FRAME;

    //団子が持つ色データ
    DangoColor _color = DangoColor.None;

    //団子のコンポーネント
    [SerializeField] Renderer rend = default!;
    public Rigidbody Rb { get; private set; }

    //オブジェクトプールマネージャー
    DangoPoolManager _poolManager;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
    }

    private void FixedUpdate()
    {
        ReleaseDango();
    }

    private void ReleaseDango()
    {
        //もし団子がカメラ外で、速度が一定値以下であれば
        if (!rend.isVisible && Rb.velocity.magnitude < DELETE_MIN_SPEED)
        {
            if (--_frameCount <= 0) _poolManager.DangoPool.Release(this);
        }
        else
        {
            _frameCount = DELETE_FRAME;
        }
    }

    public DangoColor GetDangoColor() => _color;
    public void SetDangoColor(DangoColor type) => _color = type;
}
