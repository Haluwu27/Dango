using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    //クエスト更新までの待機時間
    const int COOLTIME = 150;
    int _coolTime = COOLTIME;

    [SerializeField,Tooltip("タイル")] Image[] images;
    [SerializeField,Tooltip("クエストテキスト")] TextMeshProUGUI[] quests;

    //クエストUIのインスタンス
    QuestUI questUI = new();

    QuestManager _questManager = new();

    //待機中判定
    bool _isWaitingCoolTime = false;

    private void Start()
    {
        questUI.OnGUIChangeQuest(images, quests);
    }

    private void FixedUpdate()
    {
        if (questUI.OnGUIQuestUpdate(quests)) _isWaitingCoolTime = true;

        NextQuest();
    }

    private void NextQuest()
    {
        //次のクエスト表示の待機中でなければ実行しない
        if (!_isWaitingCoolTime) return;

        //待機終了したか判定。していなければ待機時間をへらす
        if (!questUI.IsWaiting(--_coolTime))
        {
            //クエストを更新し、設定を戻す。
            GameManager.Quests.Clear();
            GameManager.Quests.Add(_questManager.CreateQuestCreateRole(DangoRole.POSROLE_LOOP, 2, "役「ループ」を2個作れ"));
            GameManager.Quests.Add(_questManager.CreateQuestIncludeColor(DangoColor.Blue, 2, "青色を含めて役を2個作れ！"));

            questUI.OnGUIChangeQuest(images, quests);
            _coolTime = COOLTIME;
            _isWaitingCoolTime = false;
        }
    }

}
