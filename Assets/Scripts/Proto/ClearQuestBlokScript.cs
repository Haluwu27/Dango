using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ClearQuestBlokScript : MonoBehaviour
{
    [SerializeField] TextUIData nameText;
    [SerializeField] TextUIData timeText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(QuestData data,float time)
    {
        nameText.TextData.SetText(data.QuestName);
        timeText.TextData.SetText(time.ToString("f1"));
    }
}
