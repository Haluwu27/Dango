using System.Collections.Generic;

namespace Dango.Quest
{
    class QuestCreater
    {
        #region EatDango
        public QuestEatDango CreateQuestEatDango(int id, int dangoCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCount, bool isKeyQuest, PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, dangoCount, continueCount, canCountCreateRole, canCountNoCreateRole, questName, rewardTime, enableDangoCount, isKeyQuest, questTextData, nextQuestId);
        }
        public QuestEatDango CreateQuestEatDango(int id, List<DangoColor> colors, int dangoCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCount, bool isKeyQuest,PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, colors, dangoCount, continueCount, canCountCreateRole, canCountNoCreateRole, questName, rewardTime, enableDangoCount, isKeyQuest, questTextData, nextQuestId);
        }
        public QuestEatDango CreateQuestEatDango(int id, DangoColor[] colors, int dangoCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCount, bool isKeyQuest, PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, colors, dangoCount, continueCount, canCountCreateRole, canCountNoCreateRole, questName, rewardTime, enableDangoCount, isKeyQuest, questTextData, nextQuestId);
        }
        public QuestEatDango CreateQuestEatDango(int id, DangoColor color, int dangoCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCount, bool isKeyQuest, PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, color, dangoCount, continueCount, canCountCreateRole, canCountNoCreateRole, questName, rewardTime, enableDangoCount, isKeyQuest, questTextData, nextQuestId);
        }
        #endregion

        #region CreateRole
        public QuestCreateRole CreateQuestCreateRole(int id, QuestCreateRole.EstablishRole establishRole, int specifyCount, int continueCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, establishRole, specifyCount, continueCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, QuestCreateRole.SpecifyTheRole specifyTheRole, int specifyCount, int continueCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, specifyTheRole, specifyCount, continueCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, QuestCreateRole.UseColorCount includeColor, int specifyCount, int continueCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, includeColor, specifyCount, continueCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId);
        }
        public QuestCreateRole CreateQuestCreateRole(int id, QuestCreateRole.CreateSameRole createSameRole, int specifyCount, int continueCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, createSameRole, specifyCount, continueCount, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId);
        }
        #endregion

        #region PlayAction
        public QuestPlayAction CreateQuestPlayAction(int id, QuestPlayAction.PlayerAction action, int specifyCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest,PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, action, specifyCount, questName, rewardTime, enableDangoCountUp, isKeyQuest,questTextData, nextQuestId);
        }
        #endregion

        #region Destination
        public QuestDestination CreateQuestDestination(int id, FloorManager.Floor floor, bool onEatSucceed, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest,PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, floor, onEatSucceed, questName, rewardTime, enableDangoCountUp, isKeyQuest,questTextData, nextQuestId);
        }
        public QuestDestination CreateQuestDestination(int id, FloorManager.Floor[] floors, bool onEatSucceed, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, params int[] nextQuestId)
        {
            return new(id, floors, onEatSucceed, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId);
        }
        #endregion
    }
}