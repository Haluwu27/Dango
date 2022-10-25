using System.Collections.Generic;

public abstract class QuestData
{
    int _id;
    List<int> _nextQuestId = new();

    QuestType _questType;
    string _questName;
    float _rewardTime;
    bool _enableDangoCountUp;
    bool _isKeyQuest;

    public QuestData(int id, QuestType questType, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int nextQuestId)
    {
        _id = id;
        _questType = questType;
        _questName = questName;
        _rewardTime = rewardTime;
        _enableDangoCountUp = enableDangoCountUp;
        _isKeyQuest = isKeyQuest;
        _nextQuestId.Add(nextQuestId);
    }
    public QuestData(int id, QuestType questType, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, int[] nextQuestId)
    {
        _id = id;
        _questType = questType;
        _questName = questName;
        _rewardTime = rewardTime;
        _enableDangoCountUp = enableDangoCountUp;
        _isKeyQuest = isKeyQuest;
        _nextQuestId.AddRange(nextQuestId);
    }
    public QuestData(int id, QuestType questType, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, List<int> nextQuestId)
    {
        _id = id;
        _questType = questType;
        _questName = questName;
        _rewardTime = rewardTime;
        _enableDangoCountUp = enableDangoCountUp;
        _isKeyQuest = isKeyQuest;
        _nextQuestId.AddRange(nextQuestId);
    }

    public int Id => _id;
    public QuestType QuestType => _questType;
    public string QuestName => _questName;
    public float RewardTime => _rewardTime;
    public bool EnableDangoCountUp => _enableDangoCountUp;
    public bool IsKeyQuest => _isKeyQuest;

    public bool SetKeyQuest(bool b) => _isKeyQuest = b;
    public List<int> NextQuestId => _nextQuestId;
}
