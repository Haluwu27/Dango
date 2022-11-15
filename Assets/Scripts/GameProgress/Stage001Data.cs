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

    public List<QuestData> QuestData = new();

    public void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false), 1, 0, "‰½‚ç‚©‚Ì–ğ‚ğ¬—§‚³‚¹‚é", 30f, false, false, 2, 3),
            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false, false), 1, 0, "–ğ‚ğ¬—§‚³‚¹‚¸‚É’cq‚ğH‚×‚é", 15f, false, false, 2, 3),

            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(2), 1, 0, "2F‚Å‚Å‚«‚é–ğ‚ğì‚é", 15f, false, false, 4, 5),
            questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.UseColorCount(1), 1, 0, "1F‚Å‚Å‚«‚é–ğ‚ğì‚é", 45f, false, false, 4, 5),

            //D5ã¸
            questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.FallAttack, 3, "‹}~‰ºh‚µ‚Å3‰ñh‚·", 0f, true, false, 6),
            questManager.Creater.CreateQuestEatDango(5, DangoColor.Red, 3, 0, true, true, "ÔF‚Ì’cq‚ğ3‚ÂH‚×‚é", 15f, true, false, 6),

            questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.floor10, false, "é“à‚Ì’†‘w‚ÉŒü‚©‚¦", 30f, false, false, 7, 8),

            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.SpecifyTheRole("—×F‹¾–Ê"), 2,0, "—×F‹¾–Ê‚ğ2‰ñì‚ê", 60f, true, false, 9, 10),
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.SpecifyTheRole("—Ö‰ô“]¶"), 2, 0, "—Ö‰ô“]¶‚ğ2‰ñì‚ê", 60f, true, false, 9, 10),

            //D5ã¸
            questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Orange), 2, 0, "òF‚Ì’cq‚ğŠÜ‚ñ‚Å–ğ‚ğ2‰ñì‚ê", 30f, true, false, 11),
            questManager.Creater.CreateQuestCreateRole(10, new QuestCreateRole.EstablishRole(true,false,DangoColor.Green), 2, 0, "—ÎF‚Ì’cq‚ğŠÜ‚ñ‚Å–ğ‚ğ2‰ñì‚ê", 30f, true, false, 11),

            questManager.Creater.CreateQuestDestination(11, FloorManager.Floor.floor11, false, "é“à‚ÌÅã‘w‚ÉŒü‚©‚¦", 0f, false, true, 0),
        };

        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0], quest[1]);
    }
}