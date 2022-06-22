using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;

class QuestManager
{
    int count = 1;

    public QuestData CreateQuest(QuestType type)
    {
        switch (type)
        {
            case QuestType.CreateRole:
                return new QuestCreateRole(DangoRole.GetPosRoles()[0], count, "ñ" + DangoRole.GetPosRoles()[0] + "Ç" + count + "å¬çÏÇÍ");

            case QuestType.IncludeColor:
                return new QuestIncludeColor(DangoColor.Red, 3);

            case QuestType.PlayAction:
                return new QuestPlayAction();

            case QuestType.GetScore:
                return new QuestGetScore();

            case QuestType.EatSpecialDango:
                return new QuestEatSpecialDango();
        }

        return null;
    }
}

