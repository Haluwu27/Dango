using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] Player1 player;
    private DangoColor dangoType = DangoColor.None;
    public bool canStab = false;
    DangoColor temp;

    public DangoColor GetDangoType()
    {
        temp = dangoType;
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
