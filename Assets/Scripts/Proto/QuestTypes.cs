using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dango.Quest
{
    class QuestCreateRole : QuestData
    {
        Role<int> _role;
        string _roleName;
        int _specifyCount;
        int _madeCount;

        public QuestCreateRole(Role<int> role, int count, string quest_name)
        {
            questType = QuestType.CreateRole;
            questName = quest_name;
            _role = role;
            _roleName = role.GetRolename();
            _specifyCount = count;
        }

        public QuestCreateRole(string role_name, int count, string quest_name)
        {
            _roleName = role_name;
            _specifyCount = count;
            questName = quest_name;
        }

        public Role<int> Role => _role;
        public string RoleName => _roleName;
        public int SpecifyCount => _specifyCount;
        public int MadeCount => _madeCount;
        public void AddMadeCount() => _madeCount++;

    }

    class QuestIncludeColor : QuestData
    {
        DangoColor _color;
        int _specifyCount;
        int _madeCount;

        public QuestIncludeColor(DangoColor color, int count, string quest_name)
        {
            questType = QuestType.IncludeColor;
            questName = quest_name;
            _color = color;
            _specifyCount = count;
        }

        public DangoColor Color => _color;
        public int SpecifyCount => _specifyCount;
        public int MadeCount => _madeCount;
        public void AddMadeCount() => _madeCount++;
    }

    class QuestPlayAction : QuestData
    {
        public QuestPlayAction()
        {
            questType = QuestType.PlayAction;
        }
    }

    class QuestGetScore : QuestData
    {
        public QuestGetScore()
        {
            questType = QuestType.GetScore;
        }
    }

    class QuestEatSpecialDango : QuestData
    {
        public QuestEatSpecialDango()
        {
            questType = QuestType.EatSpecialDango;
        }
    }
}