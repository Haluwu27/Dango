using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace Dango.Quest.UI {
    public class ExpansionCanvasScript : MonoBehaviour
    {
        [SerializeField] GameObject[] times = new GameObject[2];//Šg’£‘O‚Æ‚ ‚Æ
        //[SerializeField] Animator timeAnima;
        //[SerializeField] Animator questAnima;

        public bool set;
        private void Start()
        {
            times[0].SetActive(false);
            InputSystemManager.Instance.onExpansionUIPerformed += OnExpansion;
        }
        public void OnExpansion()
        {
            if (InputSystemManager.Instance.IsExpantionUI)
            {
                Onset();
            }else
            {
                OffSet();
            }
        }

        public void Onset()
        {
            if (!times[0].activeSelf)
            {
                times[0].SetActive(true);
                times[1].SetActive(false);
                //quests[1].SetActive(false);
                //timeAnima.SetTrigger("On");
                //questAnima.SetTrigger("On");
            }
        }
        public void OffSet()
        {
            if (times[0].activeSelf&&!set)
            {
                times[0].SetActive(false);
                times[1].SetActive(true);
                //quests[1].SetActive(true);
            }
        }
    }
}
