using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangoInjection : MonoBehaviour
{
    [SerializeField] private DangoPoolManager _poolManager = default!;
    [SerializeField] private int injectionCount = 20;
    [SerializeField] private int waitFrame = 500;

    private void Start()
    {
        StartCoroutine(Injection());
    }

    private IEnumerator Injection()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < waitFrame; j++)
                yield return new WaitForFixedUpdate();

            for (int j = 0; j < injectionCount; j++)
            {
                _poolManager.DangoPool.Get();
            }
        }
    }
}
