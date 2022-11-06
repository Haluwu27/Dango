using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoleDirectingScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roleText = default!;
    [SerializeField] GameObject[] imageObj;
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
        ColorDirecting(dangos);
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
            else
            {
                imageObj[i].SetActive(false);
            }
        }
    }
}
