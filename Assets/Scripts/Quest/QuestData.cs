using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class QuestData
{
    protected QuestType questType;
    protected string questName;
    protected float returnScore;
    protected bool isKeyQuest;

    public QuestType QuestType => questType;
    public string QuestName => questName;
    public float ReturnScore => returnScore;
    public bool IsKeyQuest => isKeyQuest;
}
