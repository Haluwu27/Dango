using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;
using Dango.Quest.UI;

class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    //クエスト一覧
    List<QuestData> _quests = new();

    [SerializeField] PlayerData _playerData;
    [SerializeField] GameObject _expansionUIObj;
    [SerializeField] PlayerUIManager _playerUIManager;
    [SerializeField] QuestExpansionUIManager _questExpansionUIManager;
    [SerializeField] QuestUIManager _questUIManager;
    [SerializeField] GameManager _gameManager;
    [SerializeField] StageData _stageData;
    [SerializeField] PortraitScript _portraitScript;

    private void Awake()
    {
        Instance = this;
        SucceedChecker = new(this, _playerUIManager, _portraitScript, _stageData);
    }

    private void Start()
    {
        SucceedChecker.Init();
        StageData.IAddQuest addQuest = _stageData.CurrentStage;
        addQuest.AddQuest();
    }

    //クエストの生成・クリア判定のやつ
    public QuestCreater Creater { get; private set; } = new();
    public QuestSucceedChecker SucceedChecker { get; private set; }

    public void ChangeQuest(params QuestData[] items)
    {
        _quests.Clear();

        _quests.AddRange(items);
        _questExpansionUIManager.ChangeQuest();
        _questUIManager.ChangeIcon(items);
    }
    public void ChangeQuest(List<QuestData> items)
    {
        _quests.Clear();

        _quests.AddRange(items);
        _questExpansionUIManager.ChangeQuest();
        _questUIManager.ChangeIcon(items.ToArray());
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

    public void SetIsComplete()
    {
        _gameManager.SetGameSucceed();
    }

    public void SetTutorialUIManager(TutorialUIManager manager)
    {
        SucceedChecker.SetTutorialUIManager(manager);
    }
}