using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider>().isTrigger = true;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<OneWayScript>().OnWayTriggerEnter();
    }
    private void OnTriggerExit(Collider other)
    {
        transform.parent.GetComponent<OneWayScript>().OnWayTriggerExit();
    }

}
