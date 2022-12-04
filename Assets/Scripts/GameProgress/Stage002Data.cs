using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage002Data : StageData
{
    static readonly List<DangoColor> stageDangoColors = new() { DangoColor.An, DangoColor.Beni, DangoColor.Mitarashi, DangoColor.Nori, DangoColor.Shiratama, DangoColor.Yomogi };

    protected override void Start()
    {
        base.Start();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundSource.BGM1A_STAGE1);
    }

    protected override void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false, DangoColor.Mitarashi), 2, 0, "‚İ‚½‚ç‚µ’cq‚ğŠÜ‚ñ‚Å–ğ‚ğ2‰ñì‚ê", 30f, true, false, new(
                new PortraitTextData.PTextData()),
                1),

            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.UseColorCount(2), 1, 0, "2F‚Å‚Å‚«‚é–ğ‚ğì‚é", 15f, false, false,new(
                new PortraitTextData.PTextData()),
                3,4),
            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(1), 1, 0, "1F‚Å‚Å‚«‚é–ğ‚ğì‚é", 45f, false, false,new(
                new PortraitTextData.PTextData()),
                3,4),

            //D5ã¸
            questManager.Creater.CreateQuestPlayAction(3, QuestPlayAction.PlayerAction.FallAttack, 3, "‹}~‰ºh‚µ‚Å3‰ñh‚·", 0f, true, false,new(
                new PortraitTextData.PTextData()),
                5),
            questManager.Creater.CreateQuestEatDango(4, DangoColor.Beni, 3, 0, true, true, "gF‚Ì’cq‚ğ3‚ÂH‚×‚é", 15f, true, false,new(
                new PortraitTextData.PTextData()),
                5),

            questManager.Creater.CreateQuestDestination(5, FloorManager.Floor.floor10, false, "é“à‚Ì’†‘w‚ÉŒü‚©‚¦", 30f, false, false,new(
                new PortraitTextData.PTextData()),
                6, 7),

            questManager.Creater.CreateQuestCreateRole(6, new QuestCreateRole.SpecifyTheRole("—×F‹¾–Ê"), 2,0, "—×F‹¾–Ê‚ğ2‰ñì‚ê", 60f, true, false,new(
                new PortraitTextData.PTextData()),
                8, 9),
            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.SpecifyTheRole("—Ö‰ô“]¶"), 2, 0, "—Ö‰ô“]¶‚ğ2‰ñì‚ê", 60f, true, false,new(
                new PortraitTextData.PTextData()),
                8, 9),

            //D5ã¸
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.EstablishRole(true,false,DangoColor.Mitarashi), 2, 0, "‚İ‚½‚ç‚µ’cq‚ğŠÜ‚ñ‚Å–ğ‚ğ2‰ñì‚ê", 30f, true, false,new(
                new PortraitTextData.PTextData()),
                10),
            questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Yomogi), 2, 0, "—ÎF‚Ì’cq‚ğŠÜ‚ñ‚Å–ğ‚ğ2‰ñì‚ê", 30f, true, false,new(
                new PortraitTextData.PTextData()),
                10),

            questManager.Creater.CreateQuestDestination(10, FloorManager.Floor.floor11, false, "é“à‚ÌÅã‘w‚ÉŒü‚©‚¦", 0f, false, true,new(
                new PortraitTextData.PTextData()),
                11),
            questManager.Creater.CreateQuestDestination(11, FloorManager.Floor.Max, false, "ƒNƒŠƒAI", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0], quest[1]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(
            new PortraitTextData.PTextData(0, "’cq‚ª‘‚¦‚Ä‚¢‚é‚İ‚½‚¢‚¾‚Ècc", 2f, PortraitTextData.FacePatturn.Normal),
            new(1, "H‚¤‚Ì‚ªŠy‚µ‚İ‚¾‚ºI", 2f, PortraitTextData.FacePatturn.Normal));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }
}