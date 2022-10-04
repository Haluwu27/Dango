using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dango.Quest.UI
{
    class QuestUI
    {
        public static QuestUI Instance { get; private set; } = new();

        private QuestManager _questManager = QuestManager.Instance;

        private bool _succeed = false;
        private string _questName = null;

        private string clearQuestName = null;
        private QuestUI()
        {
        }

        public void OnGUIChangeQuest(Image[] images, TextMeshProUGUI[] quests)
        {
            for (int i = 0; i < images.Length; i++)
            {
                if (_questManager.GetQuest(i) != null)
                {
                    ChangeImageEnabled(i, true, images, quests);
                    quests[i].text = _questManager.GetQuest(i).QuestName;
                }
                else
                {
                    ChangeImageEnabled(i, false, images, quests);
                }
            }
        }
        public void OnGUIChangeOldQuest(TextMeshProUGUI quest)
        {
            if (clearQuestName != null)
                quest.text = clearQuestName;
        }
        public void OnGUIQuestSucceed(string questName)
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
                    clearQuestName = _questName;
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

        private void ChangeImageEnabled(int i, bool enable, Image[] images, TextMeshProUGUI[] quests)
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
}
