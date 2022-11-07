using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootObjScript : MonoBehaviour
{
    private bool isGround;
    enum Layer
    {
        Map =7,
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == (int)Layer.Map)
            isGround = true;
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == (int)Layer.Map)
            isGround = false;
    }

    public bool GetIsGround() => isGround;
}
