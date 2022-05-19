using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] Player1 player;
    private DangoType dangoType=DangoType.None, oldDangoType=DangoType.None;
    public bool canStab = false;
    DangoType temp;

    public DangoType GetDangoType()
    {
        temp = dangoType;
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
