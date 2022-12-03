using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] U7 _u7;
    [SerializeField] U8 _u8;

    [SerializeField] QuestManager _questManager;

    [SerializeField]IngameUIManager _ingameUIManager;

    private void Start()
    {
        _u7.SetText(0);

        InputSystemManager.Instance.onTutorialSkipPerformed += CheckSkipQuest;
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onTutorialSkipPerformed -= CheckSkipQuest;
    }

    public void ChangeNextGuide(int nextQuestID)
    {
        _u7.SetText(nextQuestID);
    }

    private async void CheckSkipQuest()
    {
        if (_ingameUIManager.DuringEndProduction) return;

        _u8.SetCanvasEnable(true);

        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");

        bool skip = false;
        _u8.SetImageColor(skip);

        while (!InputSystemManager.Instance.IsPressChoice)
        {
            await UniTask.Yield();

            if (InputSystemManager.Instance.NavigateAxis.magnitude <= 0.5f) continue;

            skip = InputSystemManager.Instance.NavigateAxis.x < 0;
            _u8.SetImageColor(skip);
        }

        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");
        _u8.SetCanvasEnable(false);

        if (!skip) return;

        _questManager.SucceedChecker.QuestSkip();
        ChangeNextGuide(_questManager.GetQuest(0).NextQuestId[0]);
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

    [Serializable]
    struct U8
    {
        [SerializeField] Canvas U8Canvas;
        [SerializeField] ImageUIData ok;
        [SerializeField] ImageUIData ng;

        public void SetImageColor(bool isOk)
        {
            ok.ImageData.SetColor(isOk ? Color.red : Color.gray);
            ng.ImageData.SetColor(isOk ? Color.gray : Color.red);
        }

        public void SetCanvasEnable(bool enabled)
        {
            U8Canvas.enabled = enabled;
        }
    }
}
