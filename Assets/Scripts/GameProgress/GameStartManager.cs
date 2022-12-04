using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] FadeManager _fadeManager = default!;
    [SerializeField] bool cursor = false;

    private void Awake()
    {
        if (_fadeManager != null) _fadeManager.StartFade(TM.Easing.EaseType.Linear, FadeStyle.Fadeout, 5f);
    }

    private void Update()
    {
        if (cursor)
        {
            //マウスカーソルのやつ。
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
