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
    void Start()
    {
        //if (_role == null)
        //{
        //    _role = GameObject.Find("Canvas").transform.Find("Role").GetComponent<TextMeshProUGUI>();
        //}
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
        for (int i = color.Count-1; i > -1; i--)
        {
            switch (color[i])
            {
                case DangoColor.Red:
                    Logger.Log("赤");
                    break;
                case DangoColor.Orange:
                    Logger.Log("橙");
                    break;
                case DangoColor.Yellow:
                    Logger.Log("黄色");
                    break;
                case DangoColor.Green:
                    Logger.Log("緑");
                    break;
                case DangoColor.Blue:
                    Logger.Log("青");
                    break;
                case DangoColor.Purple:
                    Logger.Log("紫");
                    break;
                case DangoColor.Cyan:
                    Logger.Log("水色");
                    break;
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
