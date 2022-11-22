using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dango.Quest
{
    public class ArrowUIScript : MonoBehaviour
    {
        private Vector3 LookPos;
        [SerializeField] GameObject[] targets;
        [SerializeField] Vector3 _forward = Vector3.forward;
        [SerializeField] FloorManager.Floor[] questFloorNum;
        private FloorData[] floorDatas;
        private int num;
        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
            InputSystemManager.Instance.onExpansionUIPerformed += OnSet;
            InputSystemManager.Instance.onExpansionUICanceled += OffSet;
            floorDatas = new FloorData[targets.Length];

            for (int i = 0; i < targets.Length; i++)
                floorDatas[i] = targets[i].GetComponent<FloorData>();
        }
        private void OnDestroy()
        {
            InputSystemManager.Instance.onExpansionUIPerformed -= OnSet;
            InputSystemManager.Instance.onExpansionUICanceled -= OffSet;
        }

        // Update is called once per frame
        void Update()
        {
            SetPos();
            var dir = targets[num].transform.position - this.transform.position;
            transform.LookAt(LookPos);
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);
            transform.rotation = lookAtRotation * offsetRotation;
        }

        private void OffSet()
        {
            this.gameObject.SetActive(false);
        }
        private void OnSet()
        {
            if (IsQuest())
                this.gameObject.SetActive(true);
        }

        private bool IsQuest()
        {
            if (QuestManager.Instance.GetQuest(0).QuestType == QuestType.Destination)
                return true;
            else
                return false;
        }


        public void SetPos()
        {
            if (QuestManager.Instance.GetQuest(0) is QuestDestination questDest)
            {
                Logger.Log(questDest.CurrentFloor);
                for (int i = 0; i < floorDatas.Length; i++)
                {
                    if (questDest.CurrentFloor == questFloorNum[i])
                    {
                        num = i;
                        LookPos = targets[i].transform.position;
                    }
                }
            }
        }
    }
}