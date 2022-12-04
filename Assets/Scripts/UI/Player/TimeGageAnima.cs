using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGageAnima : MonoBehaviour
{
    [SerializeField] TextUIData uIData;
    [SerializeField] TimeGageAnima TimeGage;

    public void SetText(float score)
    {
        if (uIData != null)
            uIData.TextData.SetText("+" + score.ToString());
    }
    public void SetText()
    {
        if (uIData != null)
            uIData.TextData.SetText("");
    }

    public void EndAnima()
    {
        if (uIData != null)
            uIData.TextData.SetText("");
    }
    private void OnDisable()
    {
        if (uIData != null&&TimeGage!=null)
        {
            uIData.TextData.SetText("");
            TimeGage.SetText();
        }
    }
}
