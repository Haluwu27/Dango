using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] FusumaManager _fusumaManager;

    private void Start()
    {
        _fusumaManager.Open(1f);
    }
}
