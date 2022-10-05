using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Expantion
{
    static Vector3 v = new();

    public static Vector3 SetX(this Vector3 vec, float x)
    {
        v.Set(x, vec.y, vec.z);

        return v;
    }
    public static Vector3 SetY(this Vector3 vec, float y)
    {
        v.Set(vec.x, y, vec.z);

        return v;
    }
    public static Vector3 SetZ(this Vector3 vec, float z)
    {
        v.Set(vec.x, vec.x, z);

        return v;
    }

}
