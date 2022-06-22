using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dango.Quest
{
    class QuestCreateRole : QuestData
    {
        Role<int> _role;
        int _count;

        public QuestCreateRole(Role<int> role, int count,string name)
        {
            type = QuestType.CreateRole;
            this.name = name;
            _role = role;
            _count = count;
        }
    }

    class QuestIncludeColor : QuestData
    {
        DangoColor _color;
        int _count;

        public QuestIncludeColor(DangoColor color, int count)
        {
            type = QuestType.IncludeColor;
            _color = color;
            _count = count;
        }
    }

    class QuestPlayAction : QuestData
    {
        public QuestPlayAction()
        {
            type = QuestType.PlayAction;
        }
    }

    class QuestGetScore : QuestData
    {
        public QuestGetScore()
        {
            type = QuestType.GetScore;
        }
    }

    class QuestEatSpecialDango : QuestData
    {
        public QuestEatSpecialDango()
        {
            type = QuestType.EatSpecialDango;
        }
    }
}