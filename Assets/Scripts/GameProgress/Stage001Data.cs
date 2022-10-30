using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage001Data
{
    public static Stage001Data Instance = new();

    private Stage001Data()
    {
    }

    QuestManager _questManager = QuestManager.Instance;

    public List<QuestData> QuestData = new();

    public void AddQuest()
    {
        List<QuestData> quest = new()
        {
            _questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true,false), 1, 0, "何らかの役を成立させる", 30f, false, false, 2, 3 ),
            _questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false,false), 1, 0, "役を成立させずに団子を食べる", 15f, false, false,2, 3 ),

            _questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(2), 1, 0, "2色でできる役を作る", 30f, false, false, 5),
            _questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.UseColorCount(1), 1, 0, "1色でできる役を作る", 0f, true, false, 4),

            _questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.FallAttack, 3, "急降下刺しで3回刺す", 0f, true, false, 6, 7),
            _questManager.Creater.CreateQuestEatDango(5, DangoColor.Red, 3, 0, true, true, "赤色の団子を3つ食べる", 15f, false, false, 6, 7),

            //Cube001-20付近
            _questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.floor1,false, "城の南西の中庭に向かえ", 30f, true, false, 8, 9),
            //Cube001-13付近
            _questManager.Creater.CreateQuestDestination(7, FloorManager.Floor.floor1, false, "城の北側の中庭に向かえ", 30f, true, false, 8, 9),

            _questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.EstablishRole(true,false,DangoColor.Orange), 3, 0, "橙色の団子を含んで役を3回作れ", 30f, false, false,10),
            _questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Green), 3, 0, "緑色の団子を含んで役を3回作れ", 30f, false, false,10),

            _questManager.Creater.CreateQuestDestination(10, FloorManager.Floor.floor1, false, "城の宝物庫へ向かえ", 0f, false, true, 0),

            _questManager.Creater.CreateQuestDestination(11,FloorManager.Floor.floor1,true,"デバッグモード",0f,true,false,11),
        };

        QuestData.AddRange(quest);
        _questManager.ChangeQuest(quest[11]);
    }
}