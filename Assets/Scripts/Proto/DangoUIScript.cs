using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangoUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image[] DangoImagObjs;
    private List<DangoColor> PlayerDangos;
    void Start()
    {
        for (int i = 0; i < DangoImagObjs.Length; i++)
            DangoImagObjs[i].gameObject.SetActive(false);
    }

    public void DangoUISet(List<DangoColor> dangos)
    {
        PlayerDangos = dangos;
        for(int i = 0; i < PlayerDangos.Count; i++)
        {
            DangoImagObjs[i].gameObject.SetActive(true);
            
            //団子の種類をみてマテリアルに色を付ける、画像が出来たらimagを切り替える。
            //団子が刺さっていないものがあれば非アクティブに
            switch (PlayerDangos[i])
            {
                case DangoColor.Red:
                    DangoImagObjs[i].color = Color.red;
                    break;
                case DangoColor.Orange:
                    DangoImagObjs[i].color = new Color(1,0.4f,0);
                    break;
                case DangoColor.Yellow:
                    DangoImagObjs[i].color = Color.yellow;
                    break;
                case DangoColor.Green:
                    DangoImagObjs[i].color = Color.green;
                    break;
                case DangoColor.Cyan:
                    DangoImagObjs[i].color = Color.cyan;
                    break;
                case DangoColor.Blue:
                    DangoImagObjs[i].color = Color.blue;
                    break;
                case DangoColor.Purple:
                    DangoImagObjs[i].color = new Color(0.6f,0,0.6f);
                    break;
                case DangoColor.Other:
                    DangoImagObjs[i].color = Color.gray;
                    break;
                default:
                    DangoImagObjs[i].gameObject.SetActive(false);
                    break;
            }
        }
    }
}
