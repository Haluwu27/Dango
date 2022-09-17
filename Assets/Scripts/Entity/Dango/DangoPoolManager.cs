using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DangoPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject dango = default!;
    [SerializeField] private Transform parent = default!;

    public ObjectPool<DangoManager> DangoPool { get; private set; }
    private int _poolCount = 0;

    private void Awake()
    {
        DangoPool = new(OnCreateDango, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 7 * 30, 7 * 150);
    }

    private DangoManager OnCreateDango()
    {
        //’cŽq‚ðŽæ“¾
        var dangoObj = Instantiate(dango);

        //Žæ“¾‚µ‚½’cŽq‚©‚çDangoManager‚ðŽæ“¾
        var dangoManager = dangoObj.GetComponent<DangoManager>();

        //Žæ“¾‚µ‚½’iŠK‚Å’cŽq‚ÌF‚ðÝ’è
        SetDangoColor(dangoManager);

        dangoManager.transform.parent = parent;

        return dangoManager;
    }

    private void OnTakeFromPool(DangoManager dango)
    {
        SetDangoColor(dango);
        dango.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(DangoManager dango)
    {
        dango.gameObject.SetActive(false);
        dango.gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        dango.gameObject.transform.localScale = Vector3.one;
        dango.SetDangoColor(DangoColor.None);
    }

    void OnDestroyPoolObject(DangoManager dango)
    {
        Destroy(dango.gameObject);
    }

    private void SetDangoColor(DangoManager dangoManager)
    {
        dangoManager.SetDangoColor((DangoColor)_poolCount + 1);
        _poolCount++;
        if (_poolCount > 6) _poolCount = 0;

        dangoManager.Rend.material.color = dangoManager.GetDangoColor() switch
        {
            DangoColor.Red => Color.red,
            DangoColor.Orange => new Color32(255, 155, 0, 255),
            DangoColor.Yellow => Color.yellow,
            DangoColor.Green => Color.green,
            DangoColor.Cyan => Color.cyan,
            DangoColor.Blue => Color.blue,
            DangoColor.Purple => new Color32(200, 0, 255, 255),
            DangoColor.Other => Color.gray,
            _ => Color.white,
        };
    }
}
