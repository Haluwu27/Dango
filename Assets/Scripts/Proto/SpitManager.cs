using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] Player1 player;
    private DangoType dangoType;
    public bool canStab = false;

    public DangoType GetDangoType()
    {
        var temp = dangoType;
        dangoType = DangoType.None;
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
