using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;

class QuestManager
{
    //静的なクエスト一覧
    static List<QuestData> _quests = new();

    public QuestManager()
    {
        SucceedChecker = new(this);
    }

    //クエストの生成・クリア判定のやつ
    public QuestCreater Creater { get; private set; } = new();
    public QuestSucceedChecker SucceedChecker { get; private set; }

    public void ChangeQuest(params QuestData[] items)
    {
        _quests.Clear();

        _quests.AddRange(items);
    }
    public void ChangeQuest(List<QuestData>items)
    {
        _quests.Clear();

        _quests.AddRange(items);
    }

    public QuestData GetQuest(int index)
    {
        if (index < 0 || index >= _quests.Count) return null;

        return _quests[index];
    }

    public int QuestsCount => _quests.Count;
}

