using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaEventScript : MonoBehaviour
{
    private Player1 p1;
    public CameraController eatCamera;
    // Start is called before the first frame update
    void Start()
    {
        p1 = transform.parent.GetComponent<Player1>();
        if (eatCamera == null)
        eatCamera =transform.root.Find("CameraController").GetComponent<CameraController>();
    }

    public void EatAnima()
    {
        p1.EatAnima();
    }

    public void EatCameraOn()
    {
        eatCamera.OnEat();
    }
    public void EatCameraOff()
    {
        eatCamera.OffEat();
    }
}
