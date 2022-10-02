using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dango.Quest.UI
{
    class QuestUIManager : MonoBehaviour
    {
        //クエスト更新までの待機時間
        const int COOLTIME = 150;
        int _coolTime = COOLTIME;

        [SerializeField, Tooltip("タイル")] Image[] images;
        [SerializeField, Tooltip("クエストテキスト")] TextMeshProUGUI[] quests;

        QuestManager _questManager = new();

        //待機中判定
        bool _isWaitingCoolTime = false;

        private void Start()
        {
            QuestUI.Instance.OnGUIChangeQuest(images, quests);
        }

        private void FixedUpdate()
        {
            if (QuestUI.Instance.OnGUIQuestUpdate(quests)) _isWaitingCoolTime = true;

            NextQuest();
        }

        private void NextQuest()
        {
            //次のクエスト表示の待機中でなければ実行しない
            if (!_isWaitingCoolTime) return;

            //待機終了したか判定。していなければ待機時間をへらす
            if (!QuestUI.Instance.IsWaiting(--_coolTime))
            {
                //_questManager.ChangeQuest(_questManager.Creater.CreateQuestCreateRole(DangoRole.POSROLE_LOOP, 2, "役「ループ」を2個作れ！"),
                //                          _questManager.Creater.CreateQuestIncludeColor(DangoColor.Blue, 2, "青色を含めて役を2つ作れ！"));

                QuestUI.Instance.OnGUIChangeQuest(images, quests);

                _coolTime = COOLTIME;
                _isWaitingCoolTime = false;
            }
        }
    }
}
