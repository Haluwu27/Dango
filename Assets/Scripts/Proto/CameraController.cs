using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera MainCam;
    [SerializeField] Camera EatCam;
    [SerializeField] LayerMask LayerMask;

    void Start()
    {
        MainCam.gameObject.SetActive(true);
        EatCam.gameObject.SetActive(false);
    }

    public void OnEat()
    {
        EatCam.gameObject.SetActive(true);
        MainCam.gameObject.SetActive(false);

        PlayerData.Event = true;
    }

    public void OffEat()
    {
        MainCam.gameObject.SetActive(true);
        EatCam.gameObject.SetActive(false);

        PlayerData.Event = false;
    }
}