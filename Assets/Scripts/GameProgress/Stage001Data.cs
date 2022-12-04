using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage001Data : StageData
{
    static readonly List<DangoColor> stageDangoColors = new() {  DangoColor.Beni, DangoColor.Mitarashi, DangoColor.Nori, DangoColor.Shiratama, DangoColor.Yomogi };

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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false), 1, 0, "何らかの団結を成立させる", 30f, false, false,new(
                new PortraitTextData.PTextData(0,"団結を作ったな！",2f,PortraitTextData.FacePatturn.Normal),
                new(0,"お腹も膨れて一石二鳥！この調子でいこう！",2f,PortraitTextData.FacePatturn.Normal)),
                2),
            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false, false), 1, 0, "団結を成立させずに団子を食べる", 15f, false, false,new(
                new PortraitTextData.PTextData(0,"美味い！やっぱ栄都の団子は違うねぇ",2f,PortraitTextData.FacePatturn.Normal),
                new(0,"折角なら次は団結を作って食べてみるか……",2f,PortraitTextData.FacePatturn.Normal)),
                2),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.SpecifyTheRole("全天鏡面"), 0, 2, "線対称で団結を2回作れ", 30f, true, false, new(
                new PortraitTextData.PTextData()),
                3),

            questManager.Creater.CreateQuestDestination(3, FloorManager.Floor.floor8, false, "上に向かえ", 30f, false, false, new(
                new PortraitTextData.PTextData()),
                4,5),

            questManager.Creater.CreateQuestCreateRole(4, new QuestCreateRole.EstablishRole(true, false, DangoColor.Mitarashi), 2, 0, "みたらし団子を含んで団結を2回作れ", 30f, false, false, new(
                new PortraitTextData.PTextData()),
                6),
            questManager.Creater.CreateQuestCreateRole(5, new QuestCreateRole.EstablishRole(true, false, DangoColor.Nori), 2, 0, "海苔団子を含んで団結を2回作れ", 30f, false, false, new(
                new PortraitTextData.PTextData()),
                6),

            questManager.Creater.CreateQuestCreateRole(6, new QuestCreateRole.CreateSameRole(false),0,2,"異なる団結を2回連続で作る", 30f,true,false,new(
                new PortraitTextData.PTextData()),
                7),

            questManager.Creater.CreateQuestDestination(7, FloorManager.Floor.floor9, false, "上に向かえ", 0f, false, true,new(new PortraitTextData.PTextData()), 8),
            questManager.Creater.CreateQuestDestination(8, FloorManager.Floor.Max, false, "クリア！", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0], quest[1]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(new PortraitTextData.PTextData(0, "団道を始めるぜ！", 2f, PortraitTextData.FacePatturn.Normal), new(0, "まずはクエストの確認からだな…", 2f, PortraitTextData.FacePatturn.Normal), new(0, "Lボタンを押してみようぜ！", 10f, PortraitTextData.FacePatturn.Normal));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }
}