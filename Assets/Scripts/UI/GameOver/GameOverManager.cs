using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    enum GameOverChoice
    {
        Retry,
        BackToStageSelect,

        Max,
    }

    [SerializeField] Canvas _canvas;
    [SerializeField] ImageUIData _backGround;
    [SerializeField] TextUIData _gameOverText;
    [SerializeField] ImageUIData _retry;
    [SerializeField] ImageUIData _backToStageSelect;

    GameOverChoice _choice;

    private async void OnEnable()
    {
        //操作をUIに変更
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");

        SoundManager.Instance.PlayBGM(SoundSource.BGM2_GAMEOVER);

        _backToStageSelect.ImageData.SetAlpha(0);

        //UI組み込み時に削除
        _retry.gameObject.SetActive(false);
        _backToStageSelect.gameObject.SetActive(false);

        _retry.ImageData.SetAlpha(0);
        _gameOverText.TextData.SetAlpha(0);

        //黒画面をフェードイン
        await _backGround.ImageData.Fadein(0.5f, 2.5f, 0);

        //上記処理が終わったら文字を表示
        await _gameOverText.TextData.Fadein(0.5f);

        //UI組み込み時に削除
        _retry.gameObject.SetActive(true);
        _backToStageSelect.gameObject.SetActive(true);

        await UniTask.WhenAll(
            _retry.ImageData.Fadein(0.5f),
            _backToStageSelect.ImageData.Fadein(0.5f)
            );

        _retry.ImageData.SetColor(Color.red);

        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
    }

    private void OnNavigate()
    {
        if (!ChangeChoiceUtil.Choice(InputSystemManager.Instance.NavigateAxis, ref _choice, GameOverChoice.Max, true, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

        _retry.ImageData.SetColor(_choice == GameOverChoice.Retry ? Color.red : Color.white);
        _backToStageSelect.ImageData.SetColor(_choice == GameOverChoice.BackToStageSelect ? Color.red : Color.white);
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void OnChoice()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;

        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.GameOver,true);
        SceneSystem.Instance.UnLoad(SceneSystem.Instance.CurrentIngameScene, true);

        SceneSystem.Scenes scene = _choice switch
        {
            GameOverChoice.Retry => SceneSystem.Instance.CurrentIngameScene,
            GameOverChoice.BackToStageSelect => SceneSystem.Scenes.StageSelect,
            _ => throw new System.NotImplementedException(),
        };

        if (scene == SceneSystem.Instance.CurrentIngameScene) SceneSystem.Instance.ReLoad(scene);
        else SceneSystem.Instance.Load(scene);
    }
}
