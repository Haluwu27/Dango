using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("Camera").GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().planeDistance = 3;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
