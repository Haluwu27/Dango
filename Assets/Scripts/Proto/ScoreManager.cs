using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager 
{
    public static ScoreManager Instance =new ScoreManager();
    float time = 0;
    float questTime = 0;
    List<QuestData> clearQuestData = new List<QuestData>();
    List<float> clearQuestTime = new List<float>();

    public float SetQuestTime()
    {
        float temp = 0;
        for(int i=0;i<clearQuestTime.Count;i++)
            temp+=clearQuestTime[i];
        
       return questTime = time - temp;
    }
    
    public float GetTime() => time;
    public float ResetTime() => time;

    public void ResetQuestTime() => questTime = 0;
    public void AddTime() => time += Time.deltaTime;
    public void AddClearQuest(QuestData quest) => clearQuestData.Add(quest);
    public void AddClearTime(float time) => clearQuestTime.Add(time);
    public List<float> GetClearTime() => clearQuestTime;
    public List<QuestData> GetClearQuest() => clearQuestData;

    public void ResetClearQuest()=>clearQuestData.Clear();
    public void ResetClearTime()=>clearQuestTime.Clear();
}
