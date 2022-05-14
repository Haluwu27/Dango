using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangoManager : MonoBehaviour
{
    DangoType dango=DangoType.None;

    public DangoType GetDangoType() => dango;
    public void SetDangoType(DangoType type) => dango = type;
}
