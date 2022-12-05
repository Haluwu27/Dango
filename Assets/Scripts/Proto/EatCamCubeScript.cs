using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCamCubeScript : MonoBehaviour
{
    [SerializeField] int mapLayer = 7;
    [SerializeField] int noneLayer = 10;

    private List<GameObject> _obj = new();

    private void OnDisable()
    {
        SetLayer(true);
    }

    private void OnTriggerEnter(Collider col)
    {
        //if (((1<<col.gameObject.layer) & Mask.value) != 0)
        if (col.gameObject.layer == mapLayer)
        {
            col.gameObject.layer = noneLayer;
            _obj.Add(col.gameObject);
        }
    }

    private void SetLayer(bool enable)
    {
        foreach (GameObject obj in _obj)
        {
            obj.layer = mapLayer;
        }
        if (enable) _obj.Clear();
    }
    //private void SetRendEnable(bool enable)
    //{
    //    foreach (GameObject obj in _obj)
    //    {
    //        obj.enabled = enable;
    //    }
    //    if (enable) _obj.Clear();
    //}
}
