using System;
using System.Collections;
using System.Collections.Generic;
using TM.Input.KeyConfig;
using UnityEngine;

[Serializable]
public class PreservationKeyConfigData
{
    public int[] keys;

    public PreservationKeyConfigData(int[] ints)
    {
        keys = ints;
    }
    public PreservationKeyConfigData()
    {
    }
}