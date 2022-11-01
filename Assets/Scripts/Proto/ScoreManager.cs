using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager 
{
    public static ScoreManager Instance =new ScoreManager();
    float time = 0;
    List<QuestData> clearQuestData = new List<QuestData>();
    List<float> clearQuestTime = new List<float>();

    public float GetTime() => time;
    public void SetTime(int a) => time = a;
    public void AddClearQuest(QuestData quest) => clearQuestData.Add(quest);
    public void AddClearTime(float time) => clearQuestTime.Add(time);
    public List<float> GetClearTime() => clearQuestTime;
    public List<QuestData> GetClearQuest() => clearQuestData;
}
