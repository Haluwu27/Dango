using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;

class QuestUI
{
    //動的生成しても、これは静的に扱いたいためstaticに。
    private static bool _succeed = false;
    private static string _questName = null;

    public void OnGUIChangeQuest(Image[] images, TextMeshProUGUI[] quests)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (GameManager.Quests[i] != null)
            {
                Enabled(i, true, images, quests);
                quests[i].text = GameManager.Quests[i].QuestName;
            }
            else
            {
                Enabled(i, false, images, quests);
            }
        }
    }

    public static void OnGUIQuestSucceed(string questName)
    {
        _succeed = true;
        _questName = questName;
    }

    /// <summary>
    /// ※QuestUIManagerのFixedUpdate以外での使用を想定していません※
    /// </summary>
    /// <param name="quests"></param>
    /// <returns></returns>
    public bool OnGUIQuestUpdate(TextMeshProUGUI[] quests)
    {
        if (!_succeed) return false;

        foreach (var quest in quests)
        {
            if (quest.text == _questName)
            {
                quest.text = "クエストクリア！";
            }
        }

        ResetSucceed();

        return true;
    }

    private void ResetSucceed()
    {
        _succeed = false;
        _questName = null;
    }

    private void Enabled(int i, bool enable, Image[] images, TextMeshProUGUI[] quests)
    {
        images[i].enabled = enable;
        quests[i].enabled = enable;
    }

    /// <summary>
    /// --waitTimeでの使用を想定しています。
    /// </summary>
    /// <param name="waitFrame">待ち時間のメンバ</param>
    /// <returns>
    /// <para>true:待機中</para>
    /// <para>false:待機終了</para>
    /// </returns>
    public bool IsWaiting(int waitFrame)
    {
        if (waitFrame > 0) return true;

        return false;
    }
}

