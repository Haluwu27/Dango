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
            questManager.Creater.CreateQuestPlayAction(0,QuestPlayAction.PlayerAction.Stab,1,"団子を刺す",0,false,false,new(
                new PortraitTextData.PTextData(0,"お見事。刺した団子は画面右に出てるからな",5f,PortraitTextData.FacePatturn.Normal),
                new(1,"次に集めた団子を食べてみろ。団子を集めて『食べる』ボタンだ",10f,PortraitTextData.FacePatturn.Normal)),
                1),

            questManager.Creater.CreateQuestEatDango(1, 3, 0, true, true, "団子を3つ食べる", 0f, true, false,new(
                new PortraitTextData.PTextData(0,"くぅー美味い！良い調子だ",5f,PortraitTextData.FacePatturn.Normal),
                new(1,"流儀を以て、団子を食す、これが『団道』だ",5f,PortraitTextData.FacePatturn.Normal),
                new(2, "『団道』を達成すると、腹が膨れるだけじゃなく、串が伸びたり、色々と恩恵がある", 10f, PortraitTextData.FacePatturn.Normal),
                new(3, "さて、ただ食べるだけってのは味気ねぇ、次は『団結』を作ってみろ", 10f, PortraitTextData.FacePatturn.Normal),
                new(4, "『団結』には色々と種類があるが、基本的には規則正しく並べてやればいい", 10f, PortraitTextData.FacePatturn.Normal)),
                2,3),

            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.EstablishRole(true, false), 1, 0, "規則正しく並べて『団結』を作れ", 0, false, false,new(
                new PortraitTextData.PTextData(0,"見事！団子は基本的に『団結』を作って食べた方が腹持ちがいい",10f,PortraitTextData.FacePatturn.Normal),
                new(1,"次は『高跳び』ボタンで高いところに行ってみるぞ",10f,PortraitTextData.FacePatturn.Normal)),
                4),
            
            questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.EstablishRole(false, false), 1, 0, "※同じものだけを集めたり、線対称を作ってみよう", 0, false, false,new(
                new PortraitTextData.PTextData(0,"惜しいな、もうちょっと規則正しく並べてみろ！",8f,PortraitTextData.FacePatturn.Normal)),
                2, 3),

            questManager.Creater.CreateQuestDestination(4, FloorManager.Floor.floor2, false, "高台に向かえ", 30f, true, false,new(
                new PortraitTextData.PTextData(0,"良いじゃねぇか、その調子だ",5f,PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(1,"ジャンプの高さは串の長さで決まる。串が伸びればより高いところに行けるって覚えておけ", 10f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(2,"さて、次は少しテクニカルに団子を刺してみよう", 10f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(3,"ジャンプや飛び降りなどで空中にいる状態で『突き刺す』ボタンを押してみろ", 10f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(4,"その時に真下に団子があれば刺すことができるぜ", 10f, PortraitTextData.FacePatturn.Normal)),
                5),

            questManager.Creater.CreateQuestPlayAction(5, QuestPlayAction.PlayerAction.FallAttack, 1, "急降下で団子を刺す", 0f, true, true,new(
                new PortraitTextData.PTextData(0,"やるねぇ！上手いぜ！", 5f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(1,"おさらいするぜ", 5f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(2,"団子を刺して集めて『団結』を作る、これが基本", 10f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(3,"それを繰り返しながら、『団道』を達成していくのが遊び方だ", 10f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(4,"他にも『団子外し』ボタンで串から団子を外したり", 10f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(5,"『拡張表示』ボタンで現在の団道の確認をしたり", 10f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(6,"色々できるから実際に試してみてくれ", 10f, PortraitTextData.FacePatturn.Normal),
           　　 new PortraitTextData.PTextData(7,"以上！", 5f, PortraitTextData.FacePatturn.Normal)),
            6),

            questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.Max, false, "初心者指南完了！", 0f, false, true,new(
                new PortraitTextData.PTextData()),
                0),
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