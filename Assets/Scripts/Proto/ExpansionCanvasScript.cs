using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace Dango.Quest.UI {
    public class ExpansionCanvasScript : MonoBehaviour
    {
        [SerializeField] GameObject[] times = new GameObject[2];//拡張前とあと
        [SerializeField] GameObject[] quests = new GameObject[2];//拡張前と後
        [SerializeField] GameObject[] ExpansionQuestsNow;//今のクエスト
        [SerializeField] GameObject ExpansionQuestsPast;//一個前のクエスト
        [SerializeField] GameObject[] ExpansionQuestsFuture;//次のクエスト
        [SerializeField] Animator timeAnima;
        [SerializeField] Animator questAnima;
        private Image[] questimage;
        private TextMeshProUGUI[] nowQuesttext;
        public TextMeshProUGUI oldQuesttext;

        public bool set;
        private void Start()
        {
            nowQuesttext = new TextMeshProUGUI[ExpansionQuestsNow.Length];
            questimage =new Image[ExpansionQuestsNow.Length];
            for (int i = 0; i < ExpansionQuestsNow.Length; i++)
            {
                nowQuesttext[i] = ExpansionQuestsNow[i].transform.Find("text").GetComponent<TextMeshProUGUI>();
                questimage[i] = ExpansionQuestsNow[i].GetComponent<Image>();
            }
            oldQuesttext =ExpansionQuestsPast.GetComponent<TextMeshProUGUI>();
            times[0].SetActive(false);
            quests[0].SetActive(false);
        }
        public void OnExpansion(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Onset();
            }
            if (context.phase == InputActionPhase.Canceled)
            {
                OffSet();
            }
        }

        private void Update()
        {
            QuestUI.Instance.OnGUIChangeQuest(questimage,nowQuesttext);
            QuestUI.Instance.OnGUIChangeOldQuest(oldQuesttext);
        }

        public void Onset()
        {
            if (!times[0].activeSelf)
            {
                times[0].SetActive(true);
                quests[0].SetActive(true);
                times[1].SetActive(false);
                quests[1].SetActive(false);
                timeAnima.SetTrigger("On");
                questAnima.SetTrigger("On");
            }
        }
        public void OffSet()
        {
            if (times[0].activeSelf&&!set)
            {
                times[0].SetActive(false);
                quests[0].SetActive(false);
                times[1].SetActive(true);
                quests[1].SetActive(true);
            }
        }
    }
}
