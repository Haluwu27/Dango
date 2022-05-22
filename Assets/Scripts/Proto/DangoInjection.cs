using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangoInjection : MonoBehaviour
{
    [SerializeField] private DangoManager dango;

    private void Start()
    {
        StartCoroutine(Injection());
    }

    private IEnumerator Injection()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 500; j++)
                yield return null;


            for (int j = 0; j < 20; j++)
            {
                GameManager.dangoPool.Get();
            }
        }
    }
}
