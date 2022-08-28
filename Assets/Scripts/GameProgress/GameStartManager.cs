using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    static FadeManager _fadeManager = default!;
    public static FadeManager FadeManager => _fadeManager;

    [SerializeField] private float startTime;
    private void Awake()
    {
        _fadeManager = GameObject.Find("FadeCanvas").GetComponentInChildren<FadeManager>();
        _fadeManager.StartFade(TM.Easing.EaseType.Linear, FadeStyle.Fadeout, 5f);

        PlayerUIManager.time = startTime;

#if UNITY_EDITOR
        //マウスカーソルのやつ。
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }
}
