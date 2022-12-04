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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false, DangoColor.Mitarashi), 2, 0, "みたらし団子を含んで団結を2回作れ", 30f, false, false, new(
                new PortraitTextData.PTextData()),
                1),

            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.UseColorCount(2), 1, 0, "2種類でできる団結を作る", 15f, false, false,new(
                new PortraitTextData.PTextData()),
                3,4),
            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(1), 1, 0, "1種類でできる団結を作る", 45f, false, false,new(
                new PortraitTextData.PTextData()),
                3,4),

            //D5上昇
            questManager.Creater.CreateQuestPlayAction(3, QuestPlayAction.PlayerAction.FallAttack, 3, "急降下刺しで3回刺す", 0f, true, false,new(
                new PortraitTextData.PTextData()),
                5),
            questManager.Creater.CreateQuestEatDango(4, DangoColor.Beni, 3, 0, true, true, "紅色の団子を3つ食べる", 15f, true, false,new(
                new PortraitTextData.PTextData()),
                5),

            questManager.Creater.CreateQuestDestination(5, FloorManager.Floor.floor10, false, "城内の中層に向かえ", 30f, false, false,new(
                new PortraitTextData.PTextData()),
                6, 7),

            questManager.Creater.CreateQuestCreateRole(6, new QuestCreateRole.SpecifyTheRole("隣色鏡面"), 2,0, "二分割で団結を2回作れ", 60f, true, false,new(
                new PortraitTextData.PTextData()),
                8, 9),
            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.SpecifyTheRole("輪廻転生"), 2, 0, "ループで団結を2回作れ", 60f, true, false,new(
                new PortraitTextData.PTextData()),
                8, 9),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.EstablishRole(true,false,DangoColor.Mitarashi), 2, 0, "みたらし団子を含んで団結を2回作れ", 30f, true, false,new(
                new PortraitTextData.PTextData()),
                10),
            questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Yomogi), 2, 0, "緑色の団子を含んで団結を2回作れ", 30f, true, false,new(
                new PortraitTextData.PTextData()),
                10),

            questManager.Creater.CreateQuestDestination(10, FloorManager.Floor.floor11, false, "城内の上層に向かえ", 0f, false, true,new(
                new PortraitTextData.PTextData()),
                11),
            questManager.Creater.CreateQuestDestination(11, FloorManager.Floor.Max, false, "クリア！", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0], quest[1]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(
            new PortraitTextData.PTextData(0, "団子が増えているみたいだな……", 2f, PortraitTextData.FacePatturn.Normal),
            new(1, "食うのが楽しみだぜ！", 2f, PortraitTextData.FacePatturn.Normal));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }
}