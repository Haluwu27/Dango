using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoleDirectingScript : MonoBehaviour
{
    // Start is called before the first frame update
    //private TextMeshProUGUI _role;
    public static RoleDirectingScript instance = new RoleDirectingScript();
    [SerializeField] GameObject[] imageobj;
    private Image[] images;
    void Start()
    {
        //if (_role == null)
        //{
        //    _role = GameObject.Find("Canvas").transform.Find("Role").GetComponent<TextMeshProUGUI>();
        //}
        images = new Image[imageobj.Length];
        for (int i = 0; i < imageobj.Length; i++)
        {
            images[i] = imageobj[i].GetComponent<Image>();
            imageobj[i].SetActive(false);
        }
        instance.imageobj = imageobj;
        instance.images = images;
    }

    // Update is called once per frame
    void Update()
    {
        //_role.text = GameManager.NowRoleList;
    }

    public void PosDirecting(Role<int> dangos)
    {
        //位置役の演出
        switch (dangos.GetRolename())
        {
            case "単色役":
                Logger.Log("単色役");
                break;
            case "線対称":
                Logger.Log("線対象");
                break;
            case "ループ":
                Logger.Log("ループ");
                break;
            case "二分割":
                Logger.Log("二分割");
                break;
            default:
                break;
        }
    }

    //色の演出
    public void ColorDirecting(List<DangoColor> color)
    {
        for (int i = 6; i > -1; i--)
        {
            if (color.Count > i) {
                instance.imageobj[i].SetActive(true);
                switch (color[i])
                {
                    case DangoColor.Red:
                        Logger.Log("赤");
                        instance.images[i].color = Color.red;
                        break;
                    case DangoColor.Orange:
                        Logger.Log("橙");
                        instance.images[i].color = new Color32(255, 155, 0, 255);
                        break;
                    case DangoColor.Yellow:
                        Logger.Log("黄色");
                        instance.images[i].color = Color.yellow;
                        break;
                    case DangoColor.Green:
                        Logger.Log("緑");
                        instance.images[i].color = Color.green;
                        break;
                    case DangoColor.Blue:
                        Logger.Log("青");
                        instance.images[i].color = Color.blue;
                        break;
                    case DangoColor.Purple:
                        Logger.Log("紫");
                        instance.images[i].color = new Color32(200, 0, 255, 255);
                        break;
                    case DangoColor.Cyan:
                        Logger.Log("水色");
                        instance.images[i].color = Color.cyan;
                        break;
                }
            }
            else
            {
                    Logger.Log("なし");
                instance.imageobj[i].SetActive(false);
            }
        }
    }

    //スペシャル役の演出
    private void SpecialDirecting(Role<DangoColor> dangos)
    {
        //スペシャル役がどの基準で判断するかわからないから仮でRolenameで取ってます
        switch (dangos.GetRolename())
        {
            case "temp":
                break;
            case "test2":
                break;
            case "test3":
                break;
        }
    }
}
