using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imageScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(setfalse(3));
    }
    IEnumerator setfalse(float a)
    {
        yield return new WaitForSeconds(a);
        gameObject.SetActive(false);
    }
}
