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

    DangoColor _dango = DangoColor.None;
    [SerializeField] Renderer rend = default!;
    [SerializeField] Rigidbody rb = default!;

    public void FixedUpdate()
    {
        if (!rend.isVisible && rb.velocity.magnitude < DELETE_MIN_SPEED)
        {
            _frameCount--;
        }
        else
        {
            _frameCount = DELETE_FRAME;
        }

        if (_frameCount <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public DangoColor GetDangoColor() => _dango;
    public void SetDangoType(DangoColor type) => _dango = type;
}
