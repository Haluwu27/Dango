using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] Plater1 player;
    private DangoColor dangoType;
    public bool canStab = false;

    public DangoColor GetDangoType()
    {
        var temp = dangoType;
        dangoType = DangoColor.None;
        return temp;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!canStab) return;
        if (other.gameObject.TryGetComponent(out DangoManager dango))
        {
            dangoType = dango.GetDangoType();
            other.gameObject.SetActive(false);
        }
    }
}
