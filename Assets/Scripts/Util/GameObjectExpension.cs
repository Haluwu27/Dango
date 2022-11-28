using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameObjectExpension
{
    public static void SetLayerIncludeChildren(this GameObject parent,int layer)
    {
        parent.layer = layer;

        foreach(Transform child in parent.transform)
        {
            SetLayerIncludeChildren(child.gameObject, layer);
        }
    }
}