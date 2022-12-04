using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoleDirectingScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roleText = default!;
    [SerializeField] GameObject[] imageObj;
    [SerializeField] RoleEffect roleEffect;
    private Image[] _images;

    void Start()
    {
        _images = new Image[imageObj.Length];

        for (int i = 0; i < imageObj.Length; i++)
        {
            _images[i] = imageObj[i].GetComponent<Image>();
            imageObj[i].SetActive(false);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        roleText.text = DangoRoleUI.CurrentRoleName;
    }

    public void Dirrecting(List<DangoColor> dangos)
    {
        //色は多分確定
        //ColorDirecting(dangos);
        roleEffect.RoleSetEffect(dangos[0]);

    }

    //色の演出
    private void ColorDirecting(List<DangoColor> color)
    {
        for (int i = 6; i > -1; i--)
        {
            if (color.Count > i)
            {
                imageObj[i].SetActive(true);
                _images[i].color = color[i] switch
                {
                    //この演出が必要か見直し
                    //DangoColor.None => throw new System.NotImplementedException(),
                    //DangoColor.An => throw new System.NotImplementedException(),
                    //DangoColor.Beni => throw new System.NotImplementedException(),
                    //DangoColor.Mitarashi => throw new System.NotImplementedException(),
                    //DangoColor.Nori => throw new System.NotImplementedException(),
                    //DangoColor.Shiratama => throw new System.NotImplementedException(),
                    //DangoColor.Yomogi => throw new System.NotImplementedException(),
                    DangoColor.Other => Color.gray,
                    _ => Color.white,
                };
            }
            else
            {
                imageObj[i].SetActive(false);
            }
        }
    }
}
