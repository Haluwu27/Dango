using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage001Data
{
    public static Stage001Data Instance = new();

    private Stage001Data()
    {
        AddQuest();
    }

    QuestManager _questManager = new();

    public List<QuestData> QuestData = new();

    static readonly DangoColor[] dangoColors = { DangoColor.Red, DangoColor.Orange, DangoColor.Yellow, DangoColor.Green, DangoColor.Cyan, DangoColor.Blue, DangoColor.Purple };

    private void AddQuest()
    {
        List<QuestData> quest = new()
        {
            _questManager.Creater.CreateQuestCreateRole(0,dangoColors,true,false,1,0,0,"何らかの役を成立させる",30f,false,false,new int[]{2,3 }),
            _questManager.Creater.CreateQuestCreateRole(1,dangoColors,false,false,1,0,0,"役を成立させずに団子を食べる",10f,false,false,new int[]{2,3}),
            _questManager.Creater.CreateQuestCreateRole(2,dangoColors,true,false,1,0,0,"何らかの役を成立させる",30f,false,false,new int[]{0,1 }),
            _questManager.Creater.CreateQuestCreateRole(3,dangoColors,false,false,1,0,0,"役を成立させずに団子を食べる",10f,false,false,0),
        };

        QuestData.AddRange(quest);
    }
}
