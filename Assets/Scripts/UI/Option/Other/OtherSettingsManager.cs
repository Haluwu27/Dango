using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherSettingsManager : MonoBehaviour
{
    enum OtherChoices
    {
        Credit,
        InitSaveData,

        Max,
    }

    //表示・非表示切り替え用に管理するもの
    [SerializeField] Canvas _canvas = default!;
    [SerializeField] Image[] _choiceImages;
    [SerializeField] Canvas _creditCanvas = default!;
    [SerializeField] Canvas _deleteDataCanvas = default!;
    [SerializeField] Image[] _deleteChoiceImages;

    OtherChoices _choice = 0;
    bool _canPassed;
    bool _canDelete;

    bool IsEffective => SceneSystem.Instance.PrebScene == SceneSystem.Scenes.Menu;

    private void Start()
    {
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
        if (IsEffective)
        {
            InputSystemManager.Instance.onAnyKeyPerformed += OnAnyKey;
            InputSystemManager.Instance.onBackCanceled += OnBack;
        }
        SetDeleteChoiceColor();
    }

    public void OnChangeScene()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
        if (IsEffective)
        {
            InputSystemManager.Instance.onAnyKeyPerformed -= OnAnyKey;
            InputSystemManager.Instance.onBackCanceled -= OnBack;
        }
    }

    /// <summary>
    /// Canvasの表示・非表示を設定する関数
    /// </summary>
    public void SetCanvasEnable(bool enable)
    {
        _canvas.enabled = enable;

        if (enable)
        {
            _choiceImages[(int)_choice].color = new Color32(176, 176, 176, 255);
            _choice = 0;
            _choiceImages[(int)_choice].color = Color.red;
            SetChoiceImagesColor();
        }
    }

    private void OnNavigate()
    {
        if (!_canvas.enabled) return;

        Vector2 axis = InputSystemManager.Instance.NavigateAxis;
        ChangeChoice(axis);
        InitDataChoice(axis);
    }

    private void OnChoice()
    {
        if (!_canvas.enabled) return;

        if (!IsEffective)
        {
            SoundManager.Instance.PlaySE(SoundSource.SE18_INVALID_OPERATION);
            return;
        }

        if (_choice == OtherChoices.Credit)
        {
            _creditCanvas.enabled = true;
            SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);
        }
        else if (_choice == OtherChoices.InitSaveData)
        {
            if (_deleteDataCanvas.enabled)
            {
                Decide();
                return;
            }

            _deleteDataCanvas.enabled = true;
        }
    }

    private void OnAnyKey()
    {
        if (!_canvas.enabled) return;
        if (!_creditCanvas.enabled) return;

        RunSecondTime();
    }

    private void OnBack()
    {
        if (!_canvas.enabled) return;
        if (!_deleteDataCanvas.enabled) return;

        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);
        _deleteDataCanvas.enabled = false;
    }

    // 2回目のみ実行される。クレジットを消すためにOnAnyKeyで行う付随処理
    private void RunSecondTime()
    {
        if (!InputSystemManager.Instance.WasPressedThisFrameAnyKey) return;

        //入ってくる最初のフレームも実行されてしまうので2回目のみ実行
        if (!_canPassed)
        {
            _canPassed = true;
            return;
        }

        _creditCanvas.enabled = false;
        _canPassed = false;
    }

    private void ChangeChoice(Vector2 axis)
    {
        if (IsPopUp) return;
        if (!ChangeChoiceUtil.Choice(axis, ref _choice, OtherChoices.Max, false, ChangeChoiceUtil.OptionDirection.Vertical)) return;

        _choiceImages[(int)_choice + (int)axis.y].color = new Color32(176, 176, 176, 255);
        _choiceImages[(int)_choice].color = Color.red;

        SetChoiceImagesColor();
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void InitDataChoice(Vector2 axis)
    {
        if (!_deleteDataCanvas.enabled) return;
        if (_choice != OtherChoices.InitSaveData) return;
        if (axis != Vector2.left && axis != Vector2.right) return;

        _canDelete = axis.Equals(Vector2.left);
        SetDeleteChoiceColor();
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void SetChoiceImagesColor()
    {
        //有効なら弾く
        if (IsEffective) return;

        //無効を示す色に変更
        foreach (var image in _choiceImages)
        {
            image.color = new Color32(50, 50, 50, 255);
        }
    }

    private void SetDeleteChoiceColor()
    {
        _deleteChoiceImages[_canDelete ? 1 : 0].color = new Color32(176, 176, 176, 255);
        _deleteChoiceImages[_canDelete ? 0 : 1].color = Color.red;
    }

    private void Decide()
    {
        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);

        if (_canDelete)
        {
#if UNITY_EDITOR
            //delete Savedata
            UnityEditor.EditorApplication.isPlaying = false;
#else
            //delete Savedata
            Application.Quit();
#endif
        }
        else
        {
            _deleteDataCanvas.enabled = false;
        }
    }

    public bool IsPopUp => _creditCanvas.enabled || _deleteDataCanvas.enabled;
}
