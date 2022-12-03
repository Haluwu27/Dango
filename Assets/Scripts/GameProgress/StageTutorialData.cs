using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageTutorialData : StageData
{
    static readonly List<DangoColor> stageDangoColors = new() { DangoColor.Beni, DangoColor.Shiratama, DangoColor.Yomogi };

    protected override void Start()
    {
        base.Start();
    }

    protected override void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {
            questManager.Creater.CreateQuestPlayAction(0,QuestPlayAction.PlayerAction.Stab,1,"団子をさす",0,false,false,new(new PortraitTextData.PTextData(0,"お見事。刺した団子は画面右に出てるからな",5f,PortraitTextData.FacePatturn.Normal),new(1,"次に集めた団子を食べてみろ。団子を集めて『食べる』ボタンだ",10f,PortraitTextData.FacePatturn.Normal)),1),

            questManager.Creater.CreateQuestEatDango(1, 3, 0, true, true, "団子を3つ食べる", 0f, true, false,new(new PortraitTextData.PTextData(0,"くぅー美味い！良い調子だ",5f,PortraitTextData.FacePatturn.Normal)), 2,3),

            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.EstablishRole(true, false), 1, 0, "規則正しく並べて『団結』を作れ", 0, false, false,new(new PortraitTextData.PTextData(0,"見事！団子は基本的に『団結』を作って食べた方が腹持ちがいい",10f,PortraitTextData.FacePatturn.Normal),new(0,"次は『高跳び』ボタンで高いところに行ってみるぞ",10f,PortraitTextData.FacePatturn.Normal)), 4),
            questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.EstablishRole(false, false), 1, 0, "ここにヒントを出してみるといいかもな！", 0, false, false,new(new PortraitTextData.PTextData(0,"惜しいな、もうちょっと規則正しく並べてみろ！",8f,PortraitTextData.FacePatturn.Normal)), 2, 3),

            questManager.Creater.CreateQuestDestination(4, FloorManager.Floor.floor2, false, "高台に向かえ", 30f, true, false,new(new PortraitTextData.PTextData()), 5),

            questManager.Creater.CreateQuestPlayAction(5, QuestPlayAction.PlayerAction.FallAttack, 1, "急降下で団子を刺す", 0f, true, true,new(new PortraitTextData.PTextData()), 6),
            questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.Max, false, "クリア！", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(new(0, "団道の基本中の基本は、団子を刺して集めるところからだ。", 2f, PortraitTextData.FacePatturn.Normal), new(0, "『突き刺す』ボタンで団子を刺してみろ！", 2f, PortraitTextData.FacePatturn.Normal));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }
}