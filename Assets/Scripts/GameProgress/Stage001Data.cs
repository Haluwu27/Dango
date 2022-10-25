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

    static readonly DangoColor[] dangoColors = { DangoColor.Red, DangoColor.Orange, DangoColor.Yellow, DangoColor.Green, DangoColor.Cyan, DangoColor.Blue, DangoColor.Purple };

    public void AddQuest()
    {
        List<QuestData> quest = new()
        {
            _questManager.Creater.CreateQuestCreateRole(0, dangoColors, true, false, 1, 0, 0, "‰½‚ç‚©‚Ì–ğ‚ğ¬—§‚³‚¹‚é", 30f, false, false, new int[] { 2, 3 }),
            _questManager.Creater.CreateQuestCreateRole(1, dangoColors, false, false, 1, 0, 0, "–ğ‚ğ¬—§‚³‚¹‚¸‚É’cq‚ğH‚×‚é", 15f, false, false, new int[] { 2, 3 }),

            _questManager.Creater.CreateQuestCreateRole(2, dangoColors, true, false, 1, 0, 2, "2F‚Å‚Å‚«‚é–ğ‚ğì‚é", 30f, false, false, new int[] { 5 }),
            _questManager.Creater.CreateQuestCreateRole(3, dangoColors, true, false, 1, 0, 1, "1F‚Å‚Å‚«‚é–ğ‚ğì‚é", 0f, true, false, new int[] { 4 }),

            _questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.FallAttack, 3, "‹}~‰ºh‚µ‚Å3‰ñh‚·", 0f, true, false, new int[] { 6, 7 }),
            _questManager.Creater.CreateQuestEatDango(5, DangoColor.Red, 3, 0, true, true, "ÔF‚Ì’cq‚ğ3‚ÂH‚×‚é", 15f, false, false, new int[] { 6, 7 }),

            //Cube001-20•t‹ß
            _questManager.Creater.CreateQuestDestination(6, false, "é‚Ì“ì¼‚Ì’†’ë‚ÉŒü‚©‚¦", 30f, true, false, new int[] { 8, 9 }),

            //Cube001-13•t‹ß
            _questManager.Creater.CreateQuestDestination(7, false, "é‚Ì–k‘¤‚Ì’†’ë‚ÉŒü‚©‚¦", 30f, true, false, new int[] { 8, 9 }),

            _questManager.Creater.CreateQuestCreateRole(8, DangoColor.Orange, true, true, 3, 0, 0, "òF‚Ì’cq‚ğŠÜ‚ñ‚Å–ğ‚ğ3‰ñì‚ê", 30f, false, false, new int[] { 10 }),
            _questManager.Creater.CreateQuestCreateRole(9, DangoColor.Green, true, true, 3, 0, 0, "—ÎF‚Ì’cq‚ğŠÜ‚ñ‚Å–ğ‚ğ3‰ñì‚ê", 30f, false, false, new int[] { 10 }),

            _questManager.Creater.CreateQuestDestination(10, false, "é‚Ì•ó•¨ŒÉ‚ÖŒü‚©‚¦", 0f, false, true, 0),




        };

        QuestData.AddRange(quest);
        _questManager.ChangeQuest(quest[0], quest[1]);
    }
}