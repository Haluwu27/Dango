using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest.UI;

namespace Dango.Quest
{
    class QuestSucceedChecker
    {
        public bool CheckQuestCreateRoleSucceed(QuestManager questManager, Role<int> posRole)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceed(questCr, posRole)) return true;
                }
            }

            return false;
        }

        public bool CheckQuestIncludeColorSucceed(QuestManager questManager, List<DangoColor> color)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //キャスト可能かを確認（不可能な場合エラーが起こるためこの処理は必須）
                if (questManager.GetQuest(i) is QuestIncludeColor questInc)
                {
                    if (CheckQuestSucceed(questInc, color)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestCreateRole quest, Role<int> role)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.CreateRole) return false;
            if (role == null) return false;

            //今作った役がクエストと合致しているか判定
            if (role.GetRolename() != quest.RoleName) return false;

            //作った回数をカウントして…
            quest.AddMadeCount();

            //指定回数作ったか判定
            if (quest.SpecifyCount != quest.MadeCount) return false;

            //条件すべてクリアした場合、クエスト成功として返却
            QuestSucceed(quest.QuestName);
            return true;
        }

        private bool CheckQuestSucceed(QuestIncludeColor quest, List<DangoColor> color)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.IncludeColor) return false;
            if (color == null) return false;

            //今作った役がクエスト指定の色が含まれているか判定
            foreach (var colorItem in color)
            {
                if (colorItem != quest.Color) continue;

                //作った回数をカウントして…
                quest.AddMadeCount();

                //指定回数作ったか判定して
                if (quest.SpecifyCount != quest.MadeCount) break;//作っていないならここでカット

                //作っていたら成功として返却
                QuestSucceed(quest.QuestName);
                return true;
            }

            //クエスト失敗として返却
            return false;
        }

        private bool CheckQuestSucceed(QuestPlayAction quest)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.PlayAction) return false;


            //クエスト失敗として返却
            return false;
        }

        private bool CheckQuestSucceed(QuestGetScore quest, int score)
        {
            //不正なアクセスであれば弾く
            if (quest.QuestType != QuestType.GetScore) return false;

            //スコアを追加して
            quest.AddScore(score);

            //合計スコアがミッション指定値を超えているか判定
            if (quest.Score < quest.ClearScore) return false;

            //超えていたら成功として返却
            QuestSucceed(quest.QuestName);
            return true;
        }

        private void QuestSucceed(string quest_name)
        {
            QuestUI.Instance.OnGUIQuestSucceed(quest_name);

            //なんらかのクエスト成功時の処理
            Logger.Log(quest_name + " クエストクリア！");
        }
    }
}