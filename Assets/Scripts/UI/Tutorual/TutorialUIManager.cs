using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TM.Easing.Management;
using TM.Input.KeyConfig;
using UnityEngine;
using static TM.Input.KeyConfig.KeyData;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] U7 _u7;
    [SerializeField] U8 _u8;

    [SerializeField] QuestManager _questManager;

    [SerializeField] IngameUIManager _ingameUIManager;
    [SerializeField] TextUIData _firstText;

    private void Start()
    {
        _u7.SetText(0);
        StartTextDirecting();
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

    private async void StartTextDirecting()
    {
        _firstText.TextData.SetAlpha(0);
        await _firstText.TextData.Fadein(1f);

        SoundManager.Instance.PlaySE(SoundSource.SE6_CREATE_ROLE_CHARACTER_ANIMATION);

        TextFontSizeAnimation();
    }

    private async void TextFontSizeAnimation()
    {
        float fontSize;
        float time = 0;
        float duration = 1.5f;

        while (time < duration)
        {
            await UniTask.Yield();
            time += Time.deltaTime;
            fontSize = 110f + (30f * EasingManager.EaseProgress(TM.Easing.EaseType.Linear, time, duration, 0, 0));
            _firstText.TextData.SetFontSize(fontSize);
        }
        time = 0;

        _firstText.TextData.Fadeout(0.5f).Forget();

        while (time < duration)
        {
            await UniTask.Yield();
            time += Time.deltaTime;
            fontSize = 140f + (105f * EasingManager.EaseProgress(TM.Easing.EaseType.OutBack, time, duration, 0, 0));
            _firstText.TextData.SetFontSize(fontSize);
        }

        _firstText.TextData.SetFontSize(250);
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

            if (InputSystemManager.Instance.IsPressBack)
            {
                skip = false;
                break;
            }
            if (InputSystemManager.Instance.NavigateAxis.magnitude <= 0.5f) continue;

            skip = InputSystemManager.Instance.NavigateAxis.x < 0;
            _u8.SetImageColor(skip);
        }

        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");
        _u8.SetCanvasEnable(false);

        if (!skip) return;

        ChangeNextGuide(_questManager.GetQuest(0).NextQuestId[0]);
        _questManager.SucceedChecker.QuestSkip();
    }

    [Serializable]
    struct U7
    {
        [SerializeField] ImageUIData _operationGuideImage;
        [SerializeField] TextUIData _actionTextData;
        [SerializeField] List<KeyData.GameAction> _actions;

        static readonly List<string> _actionTexts = new() { "移動アクション", "カメラ回転", "高跳びアクション", "突き刺しアクション", "食事アクション", "団子外しアクション", "UI拡張" };

        public void SetText(int nextQuestID)
        {
            if (nextQuestID < 0 || nextQuestID >= _actions.Count) return;

            _actionTextData.TextData.SetText(KeyCode(nextQuestID) + "\n" + ActionText(nextQuestID));
        }

        private string ActionText(int nextQuestID)
        {
            //急降下はベタ打ちで返す
            if (nextQuestID == 6) return "空中で刺すと急降下アクション";

            return _actions[nextQuestID] switch
            {
                KeyData.GameAction.Move => _actionTexts[0],
                KeyData.GameAction.LookRotation => _actionTexts[1],
                KeyData.GameAction.Jump => _actionTexts[2],
                KeyData.GameAction.Attack => _actionTexts[3],
                KeyData.GameAction.Eat => _actionTexts[4],
                KeyData.GameAction.Remove => _actionTexts[5],
                KeyData.GameAction.UIExpansion => _actionTexts[6],
                _ => throw new NotImplementedException(),
            };
        }

        private string KeyCode(int nextQuestID)
        {
            string str = "";

            for (int i = 0; i < DataManager.keyConfigData.keys.Length; i++)
            {
                if (DataManager.keyConfigData.keys[i] == (int)_actions[nextQuestID])
                {
                    str += (GamepadKey)i switch
                    {
                        GamepadKey.ButtonNorth => "Y,",
                        GamepadKey.ButtonEast => "B,",
                        GamepadKey.ButtonWest => "X,",
                        GamepadKey.ButtonSouth => "A,",
                        GamepadKey.L => "L,",
                        GamepadKey.R => "R,",
                        GamepadKey.LTrigger => "ZL,",
                        GamepadKey.RTrigger => "ZR,",
                        GamepadKey.LStick => "左スティック,",
                        GamepadKey.LStickPress => "左スティック押し込み,",
                        GamepadKey.RStick => "右スティック,",
                        GamepadKey.RStickPress => "右スティック押し込み,",
                        GamepadKey.D_pad => "Dパッド,",
                        GamepadKey.D_padDown => "Dパッド(下),",
                        GamepadKey.D_padUp => "Dパッド(上),",
                        GamepadKey.D_padLeft => "Dパッド(左),",
                        GamepadKey.D_padRight => "Dパッド(右),",
                        GamepadKey.Select => "セレクト,",
                        GamepadKey.Start => "スタート,",
                        _ => throw new NotImplementedException(),
                    };
                }
            }

            return str;
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
