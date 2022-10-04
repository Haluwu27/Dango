using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dango.Quest.UI {
    public class DestinationScript : MonoBehaviour
    {
        private Vector3 destPos;
        private LineRenderer line;
        private bool Quest;
        private QuestManager Qmanager = QuestManager.Instance;
        private GameObject Player;

        private void Start()
        {
            line = GetComponent<LineRenderer>();
            line.positionCount = 2;
            Player = GameObject.Find("PlayerParent").transform.Find("Player1").gameObject;
            setDest(transform.position);
        }
        private void Update()
        {
            Questcheck();
            if (Quest)
            {
                line.SetPosition(0, destPos);
                line.SetPosition(1, Player.transform.position);
            }
        }
        public void setDest(Vector3 pos)
        {
            destPos = pos;
        }

        private void Questcheck()
        {
            //スペシャル団子、目的地系以外では出ない
            switch (Qmanager.GetQuest(0).QuestType)
            {
                case QuestType.EatDango:
                case QuestType.Destination:
                    Quest = true;
                    break;
                default:
                    Quest = false;
                    break;
            }
        }
    } 
}
