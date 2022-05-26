using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideMoveObj : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision col)
    {
        //ìÆÇ≠ï®ëÃÇêeÇ…Ç∑ÇÈ
        if (transform.parent.parent == null && col.gameObject.tag == "MoveObj")
        {
            GameObject emptyObject = new GameObject();
            emptyObject.transform.parent = col.gameObject.transform;
            transform.parent.parent = emptyObject.transform;
        }
    }

    void OnCollisionExit(Collision col)
    {
        //êeÇÃâèú
        if (transform.parent.parent != null && col.gameObject.tag == "MoveObj")
        {
            GameObject o = transform.parent.parent.gameObject;
            transform.parent.parent = null;
            Destroy(o);
        }
    }
}
