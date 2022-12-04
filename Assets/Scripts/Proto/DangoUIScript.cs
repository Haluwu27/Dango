using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DangoUIScript : MonoBehaviour
{
    [SerializeField] GameObject[] Objs;
    [SerializeField] GameObject reachObjs;
    [SerializeField] Sprite[] DangoImags;
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] PlayerData PlayerData;
    private Image[] DangoImagObjs;
    private Image reachImag;
    private PlayerDangoAnima[] dangoAnimas;

    private void Start()
    {
        DangoImagObjs = new Image[Objs.Length];
        dangoAnimas = new PlayerDangoAnima[Objs.Length];
        for (int i = 0; i < Objs.Length; i++)
        {
            DangoImagObjs[i] = Objs[i].GetComponent<Image>();
            dangoAnimas[i] = Objs[i].GetComponent<PlayerDangoAnima>();
        }
        reachImag = reachObjs.GetComponent<Image>();
    }
    private void Update()
    {
        Text.text = PlayerData.GetMaxDango().ToString();
    }
    public void DangoUISet(List<DangoColor> dangos)
    {
        for (int i = 0; i < dangos.Count; i++)
        {
            //団子の種類をみてマテリアルに色を付ける、画像が出来たらimagを切り替える。
            //団子が刺さっていないものがあれば非アクティブに
            Objs[i].SetActive(true);
            //DangoImagObjs[i].sprite = DangoImags[(int)dangos[i] - 1];
            DangoImagObjs[i].sprite = dangos[i] switch
            {
                DangoColor.Red => DangoImags[(int)DangoColor.Red],
                DangoColor.Orange => DangoImags[(int)DangoColor.Orange],
                DangoColor.Yellow => DangoImags[(int)DangoColor.Yellow],
                DangoColor.Green => DangoImags[(int)DangoColor.Green],
                DangoColor.Cyan => DangoImags[(int)DangoColor.Cyan],
                DangoColor.Blue => DangoImags[(int)DangoColor.Blue],
                DangoColor.Purple => DangoImags[(int)DangoColor.Purple],
                DangoColor.Other => DangoImags[(int)DangoColor.Other],
                _ => DangoImags[(int)DangoColor.Other],
            };
        }
        //for (int i = dangos.Count; i < Objs.Length; i++)
        //{
        //    Objs[i].SetActive(false);
        //}

    }
    public void AddDango(List<DangoColor> dangos)
    {
        if (dangos.Count - 1 >= 0)
            dangoAnimas[dangos.Count - 1].AddDango();
    }

    public void RemoveDango(List<DangoColor> dangos)
    {
        dangoAnimas[dangos.Count].RemoveDango();
    }

    public void ALLRemoveDango(List<DangoColor> dangos)
    {
        for (int i = dangos.Count; i < Objs.Length; i++)
        {
            Objs[i].SetActive(false);
        }
    }

    public void ReachSet(DangoColor color,int current)
    {
        reachObjs.SetActive(true);
        reachImag.sprite = color switch
        {
            DangoColor.Red => DangoImags[(int)DangoColor.Red],
            DangoColor.Orange => DangoImags[(int)DangoColor.Orange],
            DangoColor.Yellow => DangoImags[(int)DangoColor.Yellow],
            DangoColor.Green => DangoImags[(int)DangoColor.Green],
            DangoColor.Cyan => DangoImags[(int)DangoColor.Cyan],
            DangoColor.Blue => DangoImags[(int)DangoColor.Blue],
            DangoColor.Purple => DangoImags[(int)DangoColor.Purple],
            DangoColor.Other => DangoImags[(int)DangoColor.Other],
            _ => DangoImags[(int)DangoColor.Other],
        };
        reachObjs.GetComponent<RectTransform>().anchoredPosition = Objs[current-1].GetComponent<RectTransform>().anchoredPosition;
    }
    public void ReachClose()
    {
        if(reachObjs.activeSelf)
        reachObjs.SetActive(false);
    }
}
