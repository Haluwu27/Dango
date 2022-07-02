using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class QuestData
{
    QuestType questType;
    string questName;
    float returnScore;
    bool isKeyQuest;

    public QuestData(QuestType questType, string questName, float returnScore, bool isKeyQuest)
    {
        this.questType = questType;
        this.questName = questName;
        this.returnScore = returnScore;
        this.isKeyQuest = isKeyQuest;
    }

    //これはスコアとかをまだ設定していないときに使用するのでそのうち消します
    public QuestData(QuestType questType, string questName)
    {
        this.questType = questType;
        this.questName = questName;
    }

    public QuestType QuestType => questType;
    public string QuestName => questName;
    public float ReturnScore => returnScore;
    public bool IsKeyQuest => isKeyQuest;
}
