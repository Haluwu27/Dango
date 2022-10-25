using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    static FadeManager _fadeManager = default!;
    public static FadeManager FadeManager => _fadeManager;
    [SerializeField] bool cursor = false;

    private void Awake()
    {
        _fadeManager = GameObject.Find("FadeCanvas").GetComponentInChildren<FadeManager>();
        _fadeManager.StartFade(TM.Easing.EaseType.Linear, FadeStyle.Fadeout, 5f);



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
