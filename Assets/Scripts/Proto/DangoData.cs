using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DangoType
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

public class DangoData : MonoBehaviour
{
    [SerializeField] GameObject dango;

    DangoType dangoType;

    Renderer rend;

    private void Awake()
    {
        rend = dango.GetComponent<Renderer>();
        for (int i = 0; i < 100; i++)
        {
            float x = Random.Range(-99.0f, 99.0f);
            float y = -4f;
            float z = Random.Range(-99.0f, 99.0f);

            dangoType = (DangoType)Random.Range(1, 8);

            //オブジェクトを生産
            GameObject a= Instantiate(dango, new Vector3(x, y, z), Quaternion.identity);
            a.name = "Dango" + i;
            a.GetComponent<Renderer>().material.color = dangoType switch
            {
                DangoType.Red => Color.red,
                DangoType.Orange => new Color32(255, 155, 0, 255),
                DangoType.Yellow => Color.yellow,
                DangoType.Green => Color.green,
                DangoType.Cyan => Color.cyan,
                DangoType.Blue => Color.blue,
                DangoType.Purple => new Color32(200, 0, 255, 255),
                DangoType.Other => Color.gray,
                _ => Color.white,
            };

        }
    }

}
