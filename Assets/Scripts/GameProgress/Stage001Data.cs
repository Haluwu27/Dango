using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage001Data : StageData
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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false), 1, 0, "何らかの役を成立させる", 30f, false, false,new(new PortraitTextData.PTextData(0,"おぉ、役が揃ったな！",2f,PortraitTextData.FacePatturn.Normal),new(0,"この調子で頼むぜ！",2f,PortraitTextData.FacePatturn.Normal)), 2, 3),
            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false, false), 1, 0, "役を成立させずに団子を食べる", 15f, false, false,new(new PortraitTextData.PTextData(0,"やっぱ団子は美味いな！",2f,PortraitTextData.FacePatturn.Normal),new(0,"次はキレイな順番で食ってみるか…",2f,PortraitTextData.FacePatturn.Normal)), 2, 3),

            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(2), 1, 0, "2色でできる役を作る", 15f, false, false,new(new PortraitTextData.PTextData()), 4, 5),
            questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.UseColorCount(1), 1, 0, "1色でできる役を作る", 45f, false, false,new(new PortraitTextData.PTextData()), 4, 5),

            //D5上昇
            questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.FallAttack, 3, "急降下刺しで3回刺す", 0f, true, false,new(new PortraitTextData.PTextData()), 6),
            questManager.Creater.CreateQuestEatDango(5, DangoColor.Beni, 3, 0, true, true, "紅色の団子を3つ食べる", 15f, true, false,new(new PortraitTextData.PTextData()), 6),

            questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.floor10, false, "城内の中層に向かえ", 30f, false, false,new(new PortraitTextData.PTextData()), 7, 8),

            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.SpecifyTheRole("隣色鏡面"), 2,0, "隣色鏡面を2回作れ", 60f, true, false,new(new PortraitTextData.PTextData()), 9, 10),
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.SpecifyTheRole("輪廻転生"), 2, 0, "輪廻転生を2回作れ", 60f, true, false,new(new PortraitTextData.PTextData()), 9, 10),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Mitarashi), 2, 0, "みたらし団子を含んで役を2回作れ", 30f, true, false,new(new PortraitTextData.PTextData()), 11),
            questManager.Creater.CreateQuestCreateRole(10, new QuestCreateRole.EstablishRole(true,false,DangoColor.Yomogi), 2, 0, "緑色の団子を含んで役を2回作れ", 30f, true, false,new(new PortraitTextData.PTextData()), 11),

            questManager.Creater.CreateQuestDestination(11, FloorManager.Floor.floor11, false, "城内の最上層に向かえ", 0f, false, true,new(new PortraitTextData.PTextData()), 12),
            questManager.Creater.CreateQuestDestination(12, FloorManager.Floor.Max, false, "クリア！", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
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