using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;

class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    //クエスト一覧
    List<QuestData> _quests = new();

    [SerializeField] PlayerData _playerData;
    [SerializeField] GameObject _expansionUIObj;

    private void Awake()
    {
        Instance = this;
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
    public void ChangeQuest(List<QuestData> items)
    {
        _quests.Clear();

        _quests.AddRange(items);
    }

    public QuestData GetQuest(int index)
    {
        if (index < 0 || index >= _quests.Count) return null;

        return _quests[index];
    }

    public void CreateExpansionUIObj()
    {
        Instantiate(_expansionUIObj, _playerData.transform.position, Quaternion.identity);
    }

    public int QuestsCount => _quests.Count;
    public PlayerData Player => _playerData;

    public List<QuestData> GetQuests => _quests;
}

