using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest.UI;
using System.Linq;
using System.Data;

namespace Dango.Quest
{
    class QuestSucceedChecker
    {
        QuestManager _manager;

        public QuestSucceedChecker(QuestManager manager)
        {
            _manager = manager;
        }

        #region EatDango
        public bool CheckQuestEatDangoSucceed(QuestManager questManager, List<DangoColor> colors, bool createRole)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestEatDango questEa)
                {
                    if (CheckQuestSucceed(questEa, colors, createRole)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestEatDango quest, List<DangoColor> colors, bool createRole)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.EatDango) return false;

            //役の成立次第で弾くものは弾く
            if (!quest.CanCountCreateRole && createRole)
            {
                quest.SetIsPrebCreateRole(createRole);
                return false;
            }
            if (!quest.CanCountNoCreateRole && !createRole)
            {
                quest.SetIsPrebCreateRole(createRole);
                return false;
            }

            //色を判定し、正しい色なら食べた数を追加
            foreach (var color in colors)
            {
                if (!quest.Colors.Contains(color)) continue;

                quest.AddEatCount();
            }

            if (quest.IsPrebCreateRole != createRole) quest.AddContinueCount();
            else quest.ResetContinueCount();

            //判定の前に今回役を作ったか記録
            quest.SetIsPrebCreateRole(createRole);

            //指定回数作ったか判定
            if (quest.SpecifyCount > quest.EatCount) return false;
            if (quest.ContinueCount > quest.CurrentContinueCount) return false;

            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region CreateRole
        public bool CheckQuestCreateRoleSucceed(QuestManager questManager, Role<int> posRole, List<DangoColor> dangos, bool createRole, int currentMaxDango)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceed(questCr, posRole,createRole,dangos.Distinct().Count(), currentMaxDango)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceed(QuestManager questManager, List<DangoColor> dangos, int currentMaxDango)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceed(questCr, dangos, currentMaxDango)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceed(QuestManager questManager, bool createRole)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceed(questCr, createRole)) return true;
                }
            }

            return false;
        }
        
        private bool CheckQuestSucceed(QuestCreateRole quest, Role<int> role, bool createRole,int colorCount,int currentMaxDango)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.CreateRole) return false;
            if (role == null) return false;

            //役の成立・非成立のフラグが一致していなければ弾く
            if (createRole != quest.CreateRole) return false;

            //色の数を判定
            var color = role.GetData().Distinct();
            if (color.Count() != colorCount) return false;

            //今作った役がクエストと合致しているか判定
            if (role.GetRolename() != quest.RoleName) return false;

            //完全役のみの場合完全役か判定
            if (quest.OnlyPerfectRole && role.GetData().Length != currentMaxDango) return false;

            //作った回数をカウントして…
            quest.AddMadeCount();

            //指定回数作ったか判定
            if (quest.SpecifyCount != quest.MadeCount) return false;

            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceed(QuestCreateRole quest, List<DangoColor> colors, int currentMaxDango)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.CreateRole) return false;

            //何らかの役という条件がつかないクエストの場合弾く
            if (!quest.EnableAnyRole) return false;

            //完全役のみの場合完全役か判定
            if (quest.OnlyPerfectRole && colors.Count != currentMaxDango) return false;

            //色を判定し、正しい色なら食べた数を追加
            foreach (var color in colors.Distinct())
            {
                //指定色があったら抜ける
                if (quest.Colors.Contains(color)) break;

                //ぜんぶなかったら弾く
                return false;
            }

            //作った回数をカウントして…
            quest.AddMadeCount();

            //指定回数作ったか判定
            if (quest.SpecifyCount > quest.MadeCount) return false;

            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceed(QuestCreateRole quest,bool createRole)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.CreateRole) return false;

            //役を作る場合のパターンを弾く
            if (quest.CreateRole)
            {
                quest.SetIsPrebCreateRole(createRole);
                return false;
            }

            quest.AddMadeCount();

            //指定回数作ったか判定
            if (quest.MadeCount < quest.SpecifyCount) return false;

            quest.AddContinueCount();

            if (quest.IsPrebCreateRole == createRole) quest.AddContinueCount();
            else quest.ResetContinueCount();

            quest.SetIsPrebCreateRole(createRole);

            //指定回数作ったか判定
            if (quest.ContinueCount >= quest.CurrentContinueCount) return false;
            
            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest);

            return true;
        }
        #endregion

        #region PlayAction
        public bool CheckQuestPlayActionSucceed(QuestManager questManager, QuestPlayAction.PlayerAction action)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestPlayAction questPla)
                {
                    if (CheckQuestSucceed(questPla, action)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestPlayAction quest, QuestPlayAction.PlayerAction action)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.PlayAction) return false;

            //判定したいアクションが異なったら弾く
            if (quest.Action != action) return false;

            quest.AddMadeCount();

            //指定回数作ったか判定
            if (quest.SpecifyCount != quest.MadeCount) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region Destination
        public bool CheckQuestDestinationSucceed(QuestManager questManager,bool onEatSucceed)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestDestination questDest)
                {
                    if (CheckQuestSucceed(questDest,onEatSucceed)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestDestination quest,bool onEatSucceed)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.Destination) return false;

            //目的地につくだけでいいのか、ついて食べないといけないのか判定
            if (quest.OnEatSucceed != onEatSucceed) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        private void QuestSucceed(QuestData quest)
        {
            QuestUI.Instance.OnGUIQuestSucceed(quest.QuestName);

            SoundManager.Instance.PlaySE(SoundSource.SE12_QUEST_SUCCEED);

            List<QuestData> nextQuest = new();
            for (int i = 0; i < quest.NextQuestId.Count; i++)
            {
                nextQuest.Add(Stage001Data.Instance.QuestData[quest.NextQuestId[i]]);
            }

            _manager.ChangeQuest(nextQuest);
            _manager.Player.GrowStab(quest.EnableDangoCountUp);
            _manager.Player.AddSatiety(quest.RewardTime);
            _manager.CreateExpansionUIObj();

            if (quest.IsKeyQuest)
            {
                //TODO:S7に遷移
            }

            Logger.Log(quest.QuestName + " クエストクリア！");
        }
    }
}