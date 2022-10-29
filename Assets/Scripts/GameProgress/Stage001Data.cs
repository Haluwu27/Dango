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
            _questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true,false), 1, 0, "‰½‚ç‚©‚Ì–ğ‚ğ¬—§‚³‚¹‚é", 30f, false, false, 2, 3 ),
            _questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false,false), 1, 0, "–ğ‚ğ¬—§‚³‚¹‚¸‚É’cq‚ğH‚×‚é", 15f, false, false,2, 3 ),

            _questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(2), 1, 0, "2F‚Å‚Å‚«‚é–ğ‚ğì‚é", 30f, false, false, 5),
            _questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.UseColorCount(1), 1, 0, "1F‚Å‚Å‚«‚é–ğ‚ğì‚é", 0f, true, false, 4),

            _questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.FallAttack, 3, "‹}~‰ºh‚µ‚Å3‰ñh‚·", 0f, true, false, 6, 7),
            _questManager.Creater.CreateQuestEatDango(5, DangoColor.Red, 3, 0, true, true, "ÔF‚Ì’cq‚ğ3‚ÂH‚×‚é", 15f, false, false, 6, 7),

            //Cube001-20•t‹ß
            _questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.floor1,false, "é‚Ì“ì¼‚Ì’†’ë‚ÉŒü‚©‚¦", 30f, true, false, 8, 9),
            //Cube001-13•t‹ß
            _questManager.Creater.CreateQuestDestination(7, FloorManager.Floor.floor1, false, "é‚Ì–k‘¤‚Ì’†’ë‚ÉŒü‚©‚¦", 30f, true, false, 8, 9),

            _questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.EstablishRole(true,false,DangoColor.Orange), 3, 0, "òF‚Ì’cq‚ğŠÜ‚ñ‚Å–ğ‚ğ3‰ñì‚ê", 30f, false, false,10),
            _questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Green), 3, 0, "—ÎF‚Ì’cq‚ğŠÜ‚ñ‚Å–ğ‚ğ3‰ñì‚ê", 30f, false, false,10),

            _questManager.Creater.CreateQuestDestination(10, FloorManager.Floor.floor1, false, "é‚Ì•ó•¨ŒÉ‚ÖŒü‚©‚¦", 0f, false, true, 0),
        };

        QuestData.AddRange(quest);
        _questManager.ChangeQuest(quest[0], quest[1]);
    }
}