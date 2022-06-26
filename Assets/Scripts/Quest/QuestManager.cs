using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;

class QuestManager
{
    //静的なクエスト一覧
    static List<QuestData> _quests = new();

    //クエストの生成・クリア判定のやつ
    public QuestCreater Creater { get; private set; } = new();
    public QuestSucceedChecker SucceedChecker { get; private set; } = new();

    public void ChangeQuest(params QuestData[] items)
    {
        _quests.Clear();
        
        foreach (QuestData item in items)
        {
            _quests.Add(item);
        }
    }

    public QuestData GetQuest(int index)
    {
        if (index < 0 || index >= _quests.Count) return null;

        return _quests[index];
    }

    public int QuestsCount => _quests.Count;
}

