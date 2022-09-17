using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PortraitScript : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI text;
    public void changeText(string message)
    {
        text.text = message;
    }

    public void changeImg(Sprite image)
    {
        img.sprite = image;
    }
}
