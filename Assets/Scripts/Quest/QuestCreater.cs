using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dango.Quest
{
    class QuestCreater
    {
        public QuestCreateRole CreateQuestCreateRole(string role_name, int count, string quest_name)
        {
            return new(role_name, count, quest_name);
        }

        public QuestIncludeColor CreateQuestIncludeColor(DangoColor color, int count, string quest_name)
        {
            return new(color, count, quest_name);
        }

        public QuestPlayAction CreateQuestPlayAction(int count, string quest_name)
        {
            return new(count, quest_name);
        }

        public QuestGetScore CreateQuestGetScore(int score, string quest_name)
        {
            return new(score, quest_name);
        }

        public QuestEatSpecialDango CreateQuestEatSpecialDango()
        {
            return new();
        }
    }
}