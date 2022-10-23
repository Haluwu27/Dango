using System.Collections.Generic;
using System.Linq;

namespace Dango.Quest
{
    class QuestCreater
    {
        #region EatDango
        public QuestEatDango CreateQuestEatDango(int id, int dangoCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCount, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, dangoCount, continueCount, canCountCreateRole, canCountNoCreateRole, questName, rewardTime, enableDangoCount, isKeyQuest, nextQuestId);
        }
        public QuestEatDango CreateQuestEatDango(int id, List<DangoColor> colors, int dangoCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCount, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, colors, dangoCount, continueCount, canCountCreateRole, canCountNoCreateRole, questName, rewardTime, enableDangoCount, isKeyQuest, nextQuestId);
        }
        public QuestEatDango CreateQuestEatDango(int id, DangoColor[] colors, int dangoCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCount, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, colors, dangoCount, continueCount, canCountCreateRole, canCountNoCreateRole, questName, rewardTime, enableDangoCount, isKeyQuest, nextQuestId);
        }
        public QuestEatDango CreateQuestEatDango(int id, DangoColor color, int dangoCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCount, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, color, dangoCount, continueCount, canCountCreateRole, canCountNoCreateRole, questName, rewardTime, enableDangoCount, isKeyQuest, nextQuestId);
        }
        #endregion

        #region CreateRole
        public QuestCreateRole CreateQuestCreateRole(int id, Role<int> role, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, role.GetRolename(), color, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, Role<int> role, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, role.GetRolename(), color, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId.ToList());
        }
        public QuestCreateRole CreateQuestCreateRole(int id, Role<int> role, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, role.GetRolename(), color, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, string roleName, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, roleName, color, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, string roleName, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, roleName, color, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId.ToList());
        }
        public QuestCreateRole CreateQuestCreateRole(int id, string roleName, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, roleName, color, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, color, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, color, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId.ToList());
        }
        public QuestCreateRole CreateQuestCreateRole(int id, DangoColor color, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, color, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, Role<int> role, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, role.GetRolename(), colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, Role<int> role, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, role.GetRolename(), colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId.ToList());
        }
        public QuestCreateRole CreateQuestCreateRole(int id, Role<int> role, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, role.GetRolename(), colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, string roleName, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, roleName, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, string roleName, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, roleName, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId.ToList());
        }
        public QuestCreateRole CreateQuestCreateRole(int id, string roleName, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, roleName, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId.ToList());
        }
        public QuestCreateRole CreateQuestCreateRole(int id, DangoColor[] colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, Role<int> role, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, role.GetRolename(), colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, Role<int> role, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, role.GetRolename(), colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId.ToList());
        }
        public QuestCreateRole CreateQuestCreateRole(int id, Role<int> role, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, role.GetRolename(), colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, string roleName, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, roleName, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, string roleName, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, roleName, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId.ToList());
        }
        public QuestCreateRole CreateQuestCreateRole(int id, string roleName, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, roleName, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId.ToList());
        }
        public QuestCreateRole CreateQuestCreateRole(int id, List<DangoColor> colors, bool createRole, bool onlyPerfectRole, int specifyCount, int continueCount, int colorCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, colors, createRole, onlyPerfectRole, specifyCount, continueCount, colorCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        #endregion

        #region PlayAction
        public QuestPlayAction CreateQuestPlayAction(int id, QuestPlayAction.PlayerAction action, int specifyCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, action, specifyCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestPlayAction CreateQuestPlayAction(int id, QuestPlayAction.PlayerAction action, int specifyCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, action, specifyCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        public QuestPlayAction CreateQuestPlayAction(int id, QuestPlayAction.PlayerAction action, int specifyCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, action, specifyCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        #endregion

        #region Destination
        public QuestDestination CreateQuestDestination(int id, bool onEatSucceed, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
        {
            return new(id, onEatSucceed, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }        
        public QuestDestination CreateQuestDestination(int id, bool onEatSucceed, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
        {
            return new(id, onEatSucceed, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        } 
        public QuestDestination CreateQuestDestination(int id, bool onEatSucceed, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
        {
            return new(id, onEatSucceed, questName, rewardTime, enableDangoCountUp, isKeyQuest, nextQuestId);
        }
        #endregion
    }
}