using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGageAnima : MonoBehaviour
{
    TextUIData uIData;
    [SerializeField] TimeGageAnima TimeGage;

    public void Start()
    {
        uIData = GetComponent<TextUIData>();
    }

    public void SetText(float score)
    {
        uIData.TextData.SetText("+"+score.ToString());
    }
    public void SetText()
    {
        if (uIData != null)
        uIData.TextData.SetText("");
    }

    public void EndAnima()
    {
        uIData.TextData.SetText("");
    }
    private void OnDisable()
    {
        if (uIData != null)
        {
            uIData.TextData.SetText("");
            TimeGage.SetText();
        }
    }
}
