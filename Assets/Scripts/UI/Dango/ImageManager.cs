using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour
{ 
    private void OnEnable()
    {
        Invoke("SetFalse", 3);
    }

    void SetFalse()
    {
        gameObject.SetActive(false);
    }
}
