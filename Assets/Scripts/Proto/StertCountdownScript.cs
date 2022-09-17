using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StertCountdownScript : MonoBehaviour
{
    int a = 3;
    int i=0;
    TextMeshProUGUI text;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "3";
    }
    public void countDown()//アニメーションから呼び出し
    {
        i++;
        text.text = (a - i).ToString("0");

        if (i == a)
            text.text = "始め！";
        
        if (i > a)
            Destroy(gameObject);
    }
}
