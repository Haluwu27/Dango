using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera MainCam;
    [SerializeField] Camera EatCam;
    [SerializeField] GameObject EatCamCube;
    [SerializeField] LayerMask LayerMask;

    void Start()
    {
        MainCam.enabled = true;

        EatCam.enabled = false;
        EatCamCube.SetActive(false);
    }

    public void OnEat()
    {
        EatCam.enabled = true;
        EatCamCube.SetActive(true);

        MainCam.enabled = false;

        PlayerData.Event = true;
    }

    public void OffEat()
    {
        MainCam.enabled = true;

        EatCam.enabled = false;
        EatCamCube.SetActive(false);

        PlayerData.Event = false;
    }
}