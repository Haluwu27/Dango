using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] BoxCollider bloc;

    public void OnWayTriggerEnter()
    {
        bloc.isTrigger = true;
    }
    public void OnWayTriggerExit()
    {
        bloc.isTrigger = false;
    }
}
