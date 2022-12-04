using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestExpansionUIManager : MonoBehaviour
{
    [SerializeField] ImageUIData _prebQuest;
    [SerializeField] TextUIData _prebQuestText;
    [SerializeField] ImageUIData[] _currentQuests;
    [SerializeField] TextUIData[] _currentQuestTexts;
    [SerializeField] ImageUIData _nextQuest;
    [SerializeField] TextUIData _nextQuestText;

    [SerializeField] Canvas _canvas;

    private void Start()
    {
        SetCanvasEnable(false);

        InputSystemManager.Instance.onExpansionUIPerformed += OnExpansionPerformed;
        InputSystemManager.Instance.onExpansionUICanceled += OnExpansionCanceled;
        ChangeQuest();
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onExpansionUIPerformed -= OnExpansionPerformed;
        InputSystemManager.Instance.onExpansionUICanceled -= OnExpansionCanceled;
    }

    public void ChangeQuest()
    {
        if (this == null) return;

        try
        {
            _currentQuestTexts[0].TextData.SetText(QuestManager.Instance.GetQuest(0).QuestName);

            QuestData questData = QuestManager.Instance.GetQuest(1);

            if (questData == null)
            {
                _currentQuests[1].gameObject.SetActive(false);

                _currentQuests[0].ImageData.SetPositionY(0);
            }
            else
            {
                _currentQuestTexts[1].TextData.SetText(questData.QuestName);
                _currentQuests[1].gameObject.SetActive(true);

                _currentQuests[0].ImageData.SetPositionY(-60f);
                _currentQuests[1].ImageData.SetPositionY(60f);
            }
        }
        catch (NullReferenceException)
        {
            return;
        }
    }

    private void SetCanvasEnable(bool enable)
    {
        _canvas.enabled = enable;
    }
    private void OnExpansionPerformed()
    {
        if (PlayerData.Event) return;
        _canvas.enabled = true;
    }
    private void OnExpansionCanceled()
    {
        _canvas.enabled = false;
    }
}
