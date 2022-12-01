using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using TM.Easing.Management;
using TM.Easing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class MenuManager : MonoBehaviour
{
    enum Menu
    {
        None,

        StageSelect,
        Option,
        Tutorial,
        Ex,
        Quit,

        Max,
    }

    [SerializeField] FusumaManager _fusumaManager;
    [SerializeField] RectTransform option;
    [SerializeField] RectTransform tutorial;
    [SerializeField] RectTransform ex;
    [SerializeField] Image dandou;
    [SerializeField] Image optionImage;
    [SerializeField] Sprite[] optionSprites;
    [SerializeField] Image tutorialImage;
    [SerializeField] Sprite[] tutorialSprites;
    [SerializeField] Image exImage;
    [SerializeField] Sprite[] exSprites;
    [SerializeField] Image quitImage;

    [SerializeField] Canvas _quitCanvas;
    [SerializeField] Image[] quitImages;

    Menu _currentMenu = Menu.StageSelect;
    bool _isTransition;

    bool isSelectedQuit;

    const float SELECTTIME = 1f;
    const float NOSELECTTIME = 1f;

    const float WIDTH_MAX = 400f;
    //const float WIDTH_MIN = 0f;
    const EaseType EASETYPE = EaseType.OutBack;

    private Menu CurrentMenu
    {
        get => _currentMenu;
        set
        {
            SetNoSelect(_currentMenu);
            _currentMenu = value;
        }
    }

    private async void Start()
    {
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onNavigatePerformed += QuitNavgate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
        InputSystemManager.Instance.onBackPerformed += OnCancel;

        SetSelect();

        SetNoSelect(Menu.Option);
        SetNoSelect(Menu.Tutorial);
        SetNoSelect(Menu.Ex);

        await _fusumaManager.UniTaskOpen(1f);

        SoundManager.Instance.PlayBGM(SoundSource.BGM5_MENU);
    }

    private void OnNavigate()
    {
        if (_isTransition) return;
        if (_quitCanvas.enabled) return;

        if (InputSystemManager.Instance.NavigateAxis == Vector2.left)
        {
            CurrentMenu--;
            if (CurrentMenu == Menu.None) CurrentMenu = Menu.Max - 1;
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.right)
        {
            CurrentMenu++;
            if (CurrentMenu == Menu.Max) CurrentMenu = Menu.None + 1;
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.up)
        {
            CurrentMenu = Menu.Quit;
        }

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
        SetSelect();
    }

    private async void OnChoice()
    {
        //TODO:S10解放したら消す
        if (CurrentMenu == Menu.Ex)
        {
            Logger.Log("メンテナンス中です");
            return;
        }

        if (_isTransition) return;
        _isTransition = true;
        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);

        if (CurrentMenu == Menu.Tutorial) SoundManager.Instance.StopBGM(1.5f);

        if (CurrentMenu == Menu.Quit && !isSelectedQuit)
        {
            SetQuitChoiceColor(isSelectedQuit);
         
            _quitCanvas.enabled ^= true;
            _isTransition = false;
            return;
        }

        await _fusumaManager.UniTaskClose(1.5f);

        switch (CurrentMenu)
        {
            case Menu.Option:
                ToOption();
                break;
            case Menu.Tutorial:
                ToTutorial();
                break;
            case Menu.Ex:
                ToEx();
                break;
            case Menu.StageSelect:
                ToStageSelect();
                break;
            case Menu.Quit:
                ToQuit();
                break;
        }
    }

    private void OnCancel()
    {
        if (!_quitCanvas.enabled) return;

        _quitCanvas.enabled = false;
        isSelectedQuit = false;
    }

    private async void SetSelect()
    {
        switch (CurrentMenu)
        {
            case Menu.Option:
                //ここのカラー変更を選択画像に変更すればOK
                optionImage.sprite = optionSprites[1];
                await Selecting(CurrentMenu, option, SELECTTIME);
                break;
            case Menu.Tutorial:
                tutorialImage.sprite = tutorialSprites[1];
                await Selecting(CurrentMenu, tutorial, SELECTTIME);
                break;
            case Menu.Ex:
                exImage.sprite = exSprites[1];
                await Selecting(CurrentMenu, ex, SELECTTIME);
                break;
            case Menu.StageSelect:
                dandou.color = Color.red;
                break;
            case Menu.Quit:
                quitImage.color = Color.red;
                break;
        }
    }

    private async UniTask Selecting(Menu menu, RectTransform rect, float time, float waitTime = 0)
    {
        await UniTask.Delay((int)(waitTime * 1000f));
        float currentTime = 0;

        Vector2 pos = new(0, 0);
        while (currentTime <= time)
        {
            if (CurrentMenu != menu) break;

            await UniTask.Yield();
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 3f, 0);

            pos.Set(rect.sizeDelta.x, WIDTH_MAX - (WIDTH_MAX * (1 - d)));
            rect.sizeDelta = pos;
        }
    }

    private async void SetNoSelect(Menu menu)
    {
        switch (menu)
        {
            case Menu.Option:
                optionImage.sprite = optionSprites[0];
                await NoSelecting(menu, option, NOSELECTTIME);
                break;
            case Menu.Tutorial:
                tutorialImage.sprite = tutorialSprites[0];
                await NoSelecting(menu, tutorial, NOSELECTTIME);
                break;
            case Menu.Ex:
                exImage.sprite = exSprites[0];
                await NoSelecting(menu, ex, NOSELECTTIME);
                break;
            case Menu.StageSelect:
                dandou.color = Color.white;
                break;
            case Menu.Quit:
                quitImage.color = Color.white;
                break;
        }
    }

    private async UniTask NoSelecting(Menu menu, RectTransform rect, float time, float waitTime = 0)
    {
        await UniTask.Delay((int)(waitTime * 1000f));
        float currentTime = 0;

        Vector2 pos = new(0, 0);
        while (currentTime <= time)
        {
            if (CurrentMenu == menu) break;

            await UniTask.Yield();
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 3f, 0);

            pos.Set(rect.sizeDelta.x, WIDTH_MAX * (1 - d));
            rect.sizeDelta = pos;
        }
    }

    private void QuitNavgate()
    {
        if (!_quitCanvas.enabled) return;
        if (_currentMenu != Menu.Quit) return;

        Vector2 axis = InputSystemManager.Instance.NavigateAxis;

        if (axis != Vector2.left && axis != Vector2.right) return;
        if (isSelectedQuit && axis == Vector2.left) return;
        if (!isSelectedQuit && axis == Vector2.right) return;

        isSelectedQuit = axis.Equals(Vector2.left);
        SetQuitChoiceColor(isSelectedQuit);
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void SetQuitChoiceColor(bool isSelectedQuit)
    {
        quitImages[isSelectedQuit ? 1 : 0].color = new Color32(176, 176, 176, 255);
        quitImages[isSelectedQuit ? 0 : 1].color = Color.red;
    }


    private void ToOption()
    {
        SceneSystem.Instance.Load(SceneSystem.Scenes.Option);
        Unload();
    }
    private void ToTutorial()
    {
        SceneSystem.Instance.Load(SceneSystem.Scenes.Tutorial);
        SceneSystem.Instance.SetIngameScene(SceneSystem.Scenes.Tutorial);
        Unload();
    }
    private void ToEx()
    {
        SceneSystem.Instance.Load(SceneSystem.Scenes.Ex);
        Unload();
    }
    private void ToStageSelect()
    {
        SceneSystem.Instance.Load(SceneSystem.Scenes.StageSelect);
        Unload();
    }

    private void ToQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void Unload()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onNavigatePerformed -= QuitNavgate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
        InputSystemManager.Instance.onBackPerformed -= OnCancel;
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Menu, true);
    }
}