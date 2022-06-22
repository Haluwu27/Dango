using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class QuestData
{
    protected QuestType type;
    protected string name;
    protected float score;
    protected bool isKeyQuest;

    public QuestType QuestType => type;
    public string Name => name;
    public float Score => score;
    public bool IsKeyQuest => isKeyQuest;
}
