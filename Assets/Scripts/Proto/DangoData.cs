using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DangoColor
{
    None,

    Red,
    Orange,
    Yellow,
    Green,
    Cyan,
    Blue,
    Purple,

    Other,
}

/// <summary>
/// 団子が持つデータを列挙したもの。団子にアタッチ禁止。無限ループします。
/// </summary>
public class DangoData : MonoBehaviour
{
    [SerializeField] GameObject dango;

    public DangoColor DangoColor { get; private set; }

    private void Awake()
    {
        
        //for (int i = 0; i < 100; i++)
        //{
        //    float x = Random.Range(-99.0f, 99.0f);
        //    float y = -4f;
        //    float z = Random.Range(-99.0f, 99.0f);

        //    dangoColor = (DangoColor)Random.Range(1, 8);

        //    //オブジェクトを生産
        //    GameObject a = Instantiate(dango, new Vector3(x, y, z), Quaternion.identity);
        //    a.name = "Dango" + i;
        //    a.GetComponent<DangoManager>().SetDangoType(dangoColor);
        //    a.GetComponent<Renderer>().material.color = dangoColor switch
        //    {
        //        DangoColor.Red => Color.red,
        //        DangoColor.Orange => new Color32(255, 155, 0, 255),
        //        DangoColor.Yellow => Color.yellow,
        //        DangoColor.Green => Color.green,
        //        DangoColor.Cyan => Color.cyan,
        //        DangoColor.Blue => Color.blue,
        //        DangoColor.Purple => new Color32(200, 0, 255, 255),
        //        DangoColor.Other => Color.gray,
        //        _ => Color.white,
        //    };

        //}
    }

    private void Start()
    {
        for(int i = 0; i < 100; i++)
        GameManager.DangoPool.Get();
    }
}
