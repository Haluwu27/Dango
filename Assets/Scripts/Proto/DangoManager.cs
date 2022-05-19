using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 団子に関するマネージャークラス
/// </summary>
public class DangoManager : MonoBehaviour
{
    DangoColor dango=DangoColor.None;

    public DangoColor GetDangoType() => dango;
    public void SetDangoType(DangoColor type) => dango = type;
}
