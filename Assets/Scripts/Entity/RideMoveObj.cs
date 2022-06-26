using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideMoveObj : MonoBehaviour
{
    Transform _parant = null;

    private void OnEnable()
    {
        if (transform.parent != null) _parant = transform.parent;
    }

    void OnCollisionEnter(Collision col)
    {
        //“®‚­•¨‘Ì‚ğe‚É‚·‚é
        if (col.gameObject.CompareTag("MoveObj"))
        {
            transform.parent = col.transform;
        }
    }

    void OnCollisionExit(Collision col)
    {
        //e‚Ì‰ğœ
        if (col.gameObject.CompareTag("MoveObj"))
        {
            transform.parent = _parant;
        }
    }
}
