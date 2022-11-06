using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngamePauseManager : MonoBehaviour
{
    enum IngameChoices
    {
        Settings,
        BackToGame,
        BackToMenu,

        Max,
    }

    enum WarningChoices
    {
        OK,
        NG,

        Max,
    }

    [SerializeField] Canvas _ingamePauseCanvas;
    [SerializeField] ImageUIData[] _ingameImages;
    [SerializeField] ImageUIData[] _warningImages;
    [SerializeField] GameObject _warning;

    IngameChoices _currentIngameChoice;
    WarningChoices _currentWarningChoice;

    bool IsPopup => _warning.activeSelf;
    bool isFirst = true;

    //上端から下端に移動するか否か
    static readonly bool canMoveTopToBottom = true;

    private void OnEnable()
    {
        //TODO：BGMを下げる
        //TODO：ぼかしを入れる
        Time.timeScale = 0;

        _currentIngameChoice = 0;
        _currentWarningChoice = 0;

        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoicePerformed;
        InputSystemManager.Instance.onBackPerformed += OnBack;

        _ingameImages[0].ImageData.SetColor(Color.red);
        _warningImages[0].ImageData.SetColor(Color.red);

        _warning.SetActive(false);
    }

    private void OnExit()
    {
        Time.timeScale = 1f;

        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoicePerformed;
        InputSystemManager.Instance.onBackPerformed -= OnBack;

        ResetImagesColor(_warningImages);
        ResetImagesColor(_ingameImages);
    }

    private void OnNavigate()
    {
        if (!gameObject.activeSelf) return;

        Vector2 axis = InputSystemManager.Instance.NavigateAxis;

        ChangeIngameChoices(axis);
        ChangeWarningChoice(axis);
    }

    private void ChangeWarningChoice(Vector2 axis)
    {
        if (!IsPopup) return;

        if (!ChangeChoiceUtil.Choice(axis, ref _currentWarningChoice, WarningChoices.Max, canMoveTopToBottom, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

        ResetImagesColor(_warningImages);
        _warningImages[(int)_currentWarningChoice].ImageData.SetColor(Color.red);
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void ChangeIngameChoices(Vector2 axis)
    {
        if (IsPopup) return;

        if (!ChangeChoiceUtil.Choice(axis, ref _currentIngameChoice, IngameChoices.Max, canMoveTopToBottom, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

        ResetImagesColor(_ingameImages);
        _ingameImages[(int)_currentIngameChoice].ImageData.SetColor(Color.red);
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void OnChoicePerformed()
    {
        if (!gameObject.activeSelf) return;

        EnterIngameChoice();
        EnterWarningChoice();

        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);
    }

    private void EnterIngameChoice()
    {
        if (IsPopup) return;

        switch (_currentIngameChoice)
        {
            case IngameChoices.Settings:
                SceneSystem.Instance.Load(SceneSystem.Scenes.Option);
                SceneSystem.Instance.UnLoad(SceneSystem.Scenes.InGamePause, false);
                OnExit();
                break;

            case IngameChoices.BackToGame:
                BackToGame();
                break;

            case IngameChoices.BackToMenu:
                _warning.SetActive(true);

                WarningPopup(true);
                break;
        }
    }

    private void EnterWarningChoice()
    {
        if (!IsPopup) return;

        //初回はポップアップと同フレームで来てしまうため弾く
        if (isFirst)
        {
            isFirst = false;
            return;
        }

        //NG選択ならポップアップを閉じる。またはBackキーが押されたら閉じる
        if (_currentWarningChoice == WarningChoices.NG)
        {
            WarningPopup(false);
        }
        //OK選択ならステージとこのポーズ画面を破棄し、メニューに戻る
        else if (_currentWarningChoice == WarningChoices.OK)
        {
            SceneSystem.Instance.UnLoad(SceneSystem.Scenes.InGamePause, true);
            SceneSystem.Instance.UnLoad(SceneSystem.Instance.CurrentIngameScene, true);

            SceneSystem.Instance.Load(SceneSystem.Scenes.Menu);
            ResetBGMAndBlur();
            OnExit();
        }

        isFirst = true;
    }

    private void OnBack()
    {
        if (IsPopup) WarningPopup(false);
        else BackToGame();
    }

    private void BackToGame()
    {
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");
        OnExit();

        //UnLoadだと効率悪いからcanvasの非アクティブにしたい。ただロード処理がなぁ。って感じ
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.InGamePause, false);

        ResetBGMAndBlur();
    }

    private void ResetImagesColor(ImageUIData[] images)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].ImageData.SetColor(Color.white);
        }
    }

    private void ResetBGMAndBlur()
    {
        //TODO：BGMを戻す
        //TODO：ぼかしを戻す
    }

    private void WarningPopup(bool enable)
    {
        _warning.SetActive(enable);
    }
}
