using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    private static FadeManager _fadeManager = default!;
    public static FadeManager FadeManager => _fadeManager;

    private void Awake()
    {
        _fadeManager = GameObject.Find("FadeCanvas").GetComponentInChildren<FadeManager>();
        _fadeManager.StartFade(TM.Easing.EaseType.Linear, FadeStyle.Fadeout, 5f);

#if UNITY_EDITOR
        //マウスカーソルのやつ。
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }
}
