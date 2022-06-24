using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;

class QuestManager
{
    public QuestCreateRole CreateQuestCreateRole(string role_name, int count, string quest_name)
    {
        return new(role_name, count, quest_name);
    }
    public QuestIncludeColor CreateQuestIncludeColor(DangoColor color, int count, string quest_name)
    {
        return new(color, count, quest_name);
    }

    /// <summary>
    /// クエストをクリアしたか判定する関数
    /// </summary>
    /// <param name="quest">判定したいクエスト</param>
    /// <param name="role">作った役</param>
    /// <returns></returns>
    public bool CheckQuestSucceed(QuestCreateRole quest, Role<int> role)
    {
        //不正なアクセスであれば弾く
        if (quest.QuestType != QuestType.CreateRole) return false;
        if (role == null) return false;

        //今作った役がクエストと合致しているか判定
        if (role.GetRolename() != quest.RoleName) return false;

        //条件すべてクリアした場合、クエスト成功として返却
        Logger.Log(quest.QuestName + " クエストクリア！");
        QuestSucceed();
        return true;
    }

    public bool CheckQuestSucceed(QuestIncludeColor quest, List<DangoColor> color)
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
            Logger.Log(quest.QuestName + " クエストクリア！");
            QuestSucceed();
            return true;
        }

        //クエスト失敗として返却
        return false;
    }

    private void QuestSucceed()
    {
        //ここにクエストクリア時に行う処理
    }

}

