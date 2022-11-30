using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] U7 _u7;
    [SerializeField] ImageUIData _u8;

    [SerializeField] QuestManager _questManager;

    private void Start()
    {
        _questManager.SetTutorialUIManager(this);
        _u7.SetText(0);
    }

    public void ChangeNextGuide(int nextQuestID)
    {
        _u7.SetText(nextQuestID);
    }

    [Serializable]
    struct U7
    {
        [SerializeField] ImageUIData _operationGuideImage;
        [SerializeField] TextUIData _actionTextData;
        [SerializeField] List<string> _actionTexts;

        public void SetText(int nextQuestID)
        {
            if (nextQuestID < 0 || nextQuestID >= _actionTexts.Count) return;
            _actionTextData.TextData.SetText(_actionTexts[nextQuestID]);
        }
    }
}
