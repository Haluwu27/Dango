using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DangoPoolManager : MonoBehaviour
{
    [SerializeField] private DangoData[] dangoDatas;
    [SerializeField] private Transform parent = default!;

    public ObjectPool<DangoData>[] DangoPool { get; private set; } = new ObjectPool<DangoData>[(int)DangoColor.Other - 1];

    int index;

    private void Awake()
    {
        for (int poolIndex = 0; poolIndex < DangoPool.Length; poolIndex++)
        {
            DangoPool[poolIndex] = new(OnCreateDango, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 7 * 30, 7 * 150);
        }
    }

    private DangoData OnCreateDango()
    {
        //’cŽq‚ðŽæ“¾
        var dangoObj = Instantiate(dangoDatas[index]);

        //Žæ“¾‚µ‚½’cŽq‚©‚çDangoManager‚ðŽæ“¾
        var dangoData = dangoObj.GetComponent<DangoData>();

        dangoData.transform.parent = parent;

        return dangoData;
    }

    private void OnTakeFromPool(DangoData dango)
    {
        dango.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(DangoData dango)
    {
        dango.gameObject.SetActive(false);
        dango.gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        dango.gameObject.transform.localScale = Vector3.one;
        dango.SetDangoColor(DangoColor.None);
    }

    void OnDestroyPoolObject(DangoData dango)
    {
        Destroy(dango.gameObject);
    }

    public void SetCreateColor(DangoColor color)
    {
        index = (int)color - 1;
    }
}
