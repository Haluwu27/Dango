using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoleDirectingScript : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI _role;
    //public static RoleDirectingScript instance = new RoleDirectingScript();
    [SerializeField] GameObject[] imageobj;
    private Image[] images;
    void Start()
    {
        if (_role == null)
        {
            _role = GameObject.Find("Canvas").transform.Find("Role").GetComponent<TextMeshProUGUI>();
        }
        images = new Image[imageobj.Length];
        for (int i = 0; i < imageobj.Length; i++)
        {
            images[i] = imageobj[i].GetComponent<Image>();
            imageobj[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _role.text = GameManager.NowRoleList;
    }

    public void Dirrecting(List<DangoColor> dangos)
    {
        float i = 0;
        //色は多分確定
        ColorDirecting(dangos);

        //役が存在するかどうかだけ知りたいからスコアには適当な関数入れてます
        if (DangoRole.instance.CheckPosRole(dangos, ref i))
        {
            PosRoleDirecting();
        }
        if (DangoRole.instance.CheckSpecialRole(dangos, ref i))
        {
            SpecialRoleDirecting();
        }
        if (DangoRole.instance.CheckColorRole(ref i))
        {
            ColorRoleDirecting();
        }
    }

    private void PosRoleDirecting()
    {
        //位置役の演出
        switch (GameManager.NowRoleList)
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
    private void ColorDirecting(List<DangoColor> color)
    {
        for (int i = 6; i > -1; i--)
        {
            if (color.Count > i) {
                imageobj[i].SetActive(true);
                switch (color[i])
                {
                    case DangoColor.Red:
                        Logger.Log("赤");
                        images[i].color = Color.red;
                        break;
                    case DangoColor.Orange:
                        Logger.Log("橙");
                        images[i].color = new Color32(255, 155, 0, 255);
                        break;
                    case DangoColor.Yellow:
                        Logger.Log("黄色");
                        images[i].color = Color.yellow;
                        break;
                    case DangoColor.Green:
                        Logger.Log("緑");
                        images[i].color = Color.green;
                        break;
                    case DangoColor.Blue:
                        Logger.Log("青");
                        images[i].color = Color.blue;
                        break;
                    case DangoColor.Purple:
                        Logger.Log("紫");
                        images[i].color = new Color32(200, 0, 255, 255);
                        break;
                    case DangoColor.Cyan:
                        Logger.Log("水色");
                        images[i].color = Color.cyan;
                        break;
                }
            }
            else
            {
                imageobj[i].SetActive(false);
            }
        }
    }
    private void ColorRoleDirecting()
    {
        switch (GameManager.NowRoleList)
        {
            case "temp":
                break;
            case "test2":
                break;
            case "test3":
                break;
        }
    }

    //スペシャル役の演出
    private void SpecialRoleDirecting()
    {
        switch (GameManager.NowRoleList)
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
