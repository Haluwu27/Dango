using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using TM.Easing.Management;
using TM.Easing;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    enum Menu
    {
        None,

        Option,
        Tutorial,
        Ex,
        StageSelect,

        Max,
    }

    [SerializeField] FusumaManager _fusumaManager;
    [SerializeField] RectTransform option;
    [SerializeField] RectTransform tutorial;
    [SerializeField] RectTransform ex;
    [SerializeField] Image dandou;
    [SerializeField] Image optionImage;
    [SerializeField] Image tutorialImage;
    [SerializeField] Image exImage;
    Menu _currentMenu = Menu.Option;
    bool _isTransition;

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
        InputSystemManager.Instance.onChoicePerformed += OnChoice;

        SetSelect();
        SetNoSelect(Menu.Tutorial);
        SetNoSelect(Menu.Ex);

        await _fusumaManager.UniTaskOpen(1f);

        SoundManager.Instance.PlayBGM(SoundSource.BGM5_MENU);
    }

    private void OnNavigate()
    {
        if (_isTransition) return;

        if (InputSystemManager.Instance.NavigateAxis == Vector2.up)
        {
            CurrentMenu--;
            if (CurrentMenu == Menu.None) CurrentMenu = Menu.Max - 1;
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.down)
        {
            CurrentMenu++;
            if (CurrentMenu == Menu.Max) CurrentMenu = Menu.None + 1;
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.right)
        {
            if (!CurrentMenu.Equals(Menu.StageSelect - 1)) return;
            CurrentMenu++;
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.left)
        {
            if (!CurrentMenu.Equals(Menu.StageSelect)) return;
            CurrentMenu--;
        }

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
        SetSelect();
    }

    private async void OnChoice()
    {
        if (_isTransition) return;
        _isTransition = true;
        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);

        if (CurrentMenu == Menu.Tutorial) SoundManager.Instance.StopBGM(1.5f);
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
        }
    }

    private async void SetSelect()
    {
        switch (CurrentMenu)
        {
            case Menu.Option:
                //ここのカラー変更を選択画像に変更すればOK
                optionImage.color = Color.red;
                await Selecting(CurrentMenu, option, SELECTTIME);
                break;
            case Menu.Tutorial:
                tutorialImage.color = Color.red;
                await Selecting(CurrentMenu, tutorial, SELECTTIME);
                break;
            case Menu.Ex:
                exImage.color = Color.red;
                await Selecting(CurrentMenu, ex, SELECTTIME);
                break;
            case Menu.StageSelect:
                dandou.color = Color.red;
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

            pos.Set(WIDTH_MAX - (WIDTH_MAX * (1 - d)), rect.sizeDelta.y);
            rect.sizeDelta = pos;
        }
    }

    private async void SetNoSelect(Menu menu)
    {
        switch (menu)
        {
            case Menu.Option:
                //ここのカラー変更を選択画像に変更すればOK
                optionImage.color = Color.white;
                await NoSelecting(menu, option, NOSELECTTIME);
                break;
            case Menu.Tutorial:
                tutorialImage.color = Color.white;
                await NoSelecting(menu, tutorial, NOSELECTTIME);
                break;
            case Menu.Ex:
                exImage.color = Color.white;
                await NoSelecting(menu, ex, NOSELECTTIME);
                break;
            case Menu.StageSelect:
                dandou.color = Color.white;
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

            pos.Set(WIDTH_MAX * (1 - d), rect.sizeDelta.y);
            rect.sizeDelta = pos;
        }
    }

    private void ToOption()
    {
        Logger.Log("Optionに遷移するよ");
        SceneSystem.Instance.Load(SceneSystem.Scenes.Option);
        Unload();
    }
    private void ToTutorial()
    {
        Logger.Log("チュートリアルに遷移するよ");

        SceneSystem.Instance.Load(SceneSystem.Scenes.Tutorial);
        Unload();
    }
    private void ToEx()
    {
        Logger.Log("ぎゃらりーに遷移するよ");
        SceneSystem.Instance.Load(SceneSystem.Scenes.Ex);
        Unload();
    }
    private void ToStageSelect()
    {
        Logger.Log("ステージセレクトに遷移するよ");
        SceneSystem.Instance.Load(SceneSystem.Scenes.StageSelect);
        Unload();
    }

    private void Unload()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Menu);
    }
}