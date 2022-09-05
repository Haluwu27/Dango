using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    Stage _currentStage;
    [SerializeField] FusumaManager _fusumaManager = default!;
    [SerializeField] StageData[] _stages;
    [SerializeField] Sprite[] _stageSprites;
    [SerializeField] Image _stageImage;

    private void Start()
    {
        _fusumaManager.Open();
        InputSystemManager.Instance.onNavigatePerformed += OnChangeStage;
        InputSystemManager.Instance.onChoicePerformed += OnChoiced;
        AssignCurrentStage();
    }

    private void OnChangeStage()
    {
        if (InputSystemManager.Instance.NavigateAxis == Vector2.right)
        {
            _currentStage++;
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.left)
        {
            _currentStage--;
        }
    }

    private void OnChoiced()
    {
        //‘I‘ğ‚µ‚½‚Æ‚«‚Ìˆ—
    }

    private void AssignCurrentStage()
    {
        foreach (var stage in _stages)
        {
            if (stage.IsRelease) _currentStage = stage.Stage;
        }

        _stageImage.sprite = _stageSprites[(int)_currentStage];
    }
}
