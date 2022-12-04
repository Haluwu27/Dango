using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage003Data : StageData
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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.CreateSameRole(false), 0, 4, "異なる団結を4回連続で作る", 0f, false, false, new(
                new PortraitTextData.PTextData()),
                1,2),

            questManager.Creater.CreateQuestPlayAction(1, QuestPlayAction.PlayerAction.FallAttack, 3, "急降下刺しで2回刺す", 0f, false, false, new(
                new PortraitTextData.PTextData()),
                3),
            questManager.Creater.CreateQuestDestination(2, FloorManager.Floor.floor8, false, "上に向かえ", 0f, false, false, new(
                new PortraitTextData.PTextData()),
                3),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.SpecifyTheRole("一統団結"), 1, 0, "一種類だけで団結を作れ", 0f, true, false, new(
                new PortraitTextData.PTextData()),
                4),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(4, new QuestCreateRole.EstablishRole(true, false, DangoColor.Mitarashi), 3, 0, "あん団子を含んで団結を3回作れ", 0f, true, false, new(
                new PortraitTextData.PTextData()),
                5),

            questManager.Creater.CreateQuestDestination(5, FloorManager.Floor.floor12, false, "城内の上層に向かえ", 0f, false, false, new(
                new PortraitTextData.PTextData()),
                6,7),

            questManager.Creater.CreateQuestCreateRole(6, new QuestCreateRole.CreateSameRole(true), 0, 2, "同じ団結を2回連続で作る", 0f, false, false, new(
                new PortraitTextData.PTextData()),
                8),
            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.CreateSameRole(false), 0, 2, "異なる団結を2回連続で作る", 0f, false, false, new(
                new PortraitTextData.PTextData()),
                8),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.SpecifyTheRole("三面華鏡"), 2, 0, "三分割で団結を2回作れ", 0f, true, false, new(
                new PortraitTextData.PTextData()),
                9),

            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false), 1, 0, "何らかの団結を成立させる", 0f, false, false, new(
                new PortraitTextData.PTextData()),
                10),

            questManager.Creater.CreateQuestDestination(10, FloorManager.Floor.floor15, false, "城内の最上層に向かえ", 0f, false, true, new(
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
            new PortraitTextData.PTextData(0, "今回は刀の長さが団子4つからのスタートみたいだな", 2f, PortraitTextData.FacePatturn.Normal),
            new(1, "流儀を以て、団子を食す……いざ！", 2f, PortraitTextData.FacePatturn.Normal));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }
}