using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettingManager : MonoBehaviour
{
    //表示・非表示切り替え用に管理するもの
    [SerializeField] Canvas _canvas = default!;

    /// <summary>
    /// Canvasの表示・非表示を設定する関数
    /// </summary>
    public void SetCanvasEnable(bool enable)
    {
        _canvas.enabled = enable;
    }
}
