using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Input.KeyConfig;

public class OptionManager : MonoBehaviour
{
    /// <summary>オプション画面の選択肢</summary>
    enum OptionChoices
    {
        Option,

        KeyConfig,
        Operation,
        Sound,
        Other,

        Max,
    }

    enum OptionDirection
    {
        Vertical,
        Horizontal,
    }

    #region メンバ
    [SerializeField] KeyConfigManager _keyConfig = default!;
    [SerializeField] OperationManager _operationManager = default!;
    [SerializeField] SoundSettingManager _soundSettingManager = default!;
    [SerializeField] OtherSettingsManager _otherSettingsManager = default!;

    //現在未使用
    [SerializeField] Canvas _canvas = default!;

    [SerializeField] TextUIData[] _optionTexts;
    [SerializeField] FusumaManager _fusumaManager;

    /// <summary>
    /// 静的に取得出来るオプションキャンバス
    /// </summary>
    /// 単一シーンで実装するため、各画面間のやりとりの際に使用する静的なものです。
    public static Canvas OptionCanvas { get; private set; }

    //次に以降するオプション地点
    OptionChoices _currentChoice = OptionChoices.KeyConfig;

    //縦か横か
    static readonly OptionDirection direction = OptionDirection.Horizontal;
    static readonly Vector2[,] directionTable = { { Vector2.up, Vector2.down }, { Vector2.left, Vector2.right } };

    //上端から下端に移動するか否か
    static readonly bool canMoveTopToBottom = false;

    #endregion

    private void Awake()
    {
        OptionCanvas = _canvas;

        EnterNextChoice();
        SetFontSize();
    }

    private void Start()
    {
        OptionInputEnable();
        _fusumaManager.Open();
    }

    private void OptionInputEnable()
    {
        InputSystemManager.Instance.onBackPerformed += OnBack;
        InputSystemManager.Instance.onTabControlPerformed += ChangeChoice;
    }

    private void OptionInputDisable()
    {
        InputSystemManager.Instance.onBackPerformed -= OnBack;
        InputSystemManager.Instance.onTabControlPerformed -= ChangeChoice;
        _keyConfig.OnChangeScene();
        _operationManager.OnChangeScene();
        _soundSettingManager.OnChangeScene();
        _otherSettingsManager.OnChangeScene();
    }

    private async void OnBack()
    {
        if (_keyConfig.IsPopup) return;
        if (!_keyConfig.CheckHasKeyAllActions()) return;

        OptionInputDisable();
        await _fusumaManager.UniTaskClose(1.5f);
        SceneSystem.Instance.Load(SceneSystem.Scenes.Menu);
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Option);
    }

    private void ChangeChoice()
    {
        if (_keyConfig.IsPopup) return;
        if (!_keyConfig.CheckHasKeyAllActions()) return;

        Vector2 axis = InputSystemManager.Instance.TabControlAxis;

        //Up or Left
        if (axis == directionTable[(int)direction, 0])
        {
            _currentChoice--;

            if (_currentChoice == OptionChoices.Option) _currentChoice = canMoveTopToBottom ? OptionChoices.Max - 1 : OptionChoices.Option + 1;

            SetFontSize();
        }
        //Down or Right
        else if (axis == directionTable[(int)direction, 1])
        {
            _currentChoice++;

            if (_currentChoice == OptionChoices.Max) _currentChoice = canMoveTopToBottom ? OptionChoices.Option + 1 : OptionChoices.Max - 1;

            SetFontSize();
        }

        EnterNextChoice();
    }

    private void EnterNextChoice()
    {
        _keyConfig.SetCanvasEnable(_currentChoice == OptionChoices.KeyConfig);
        _soundSettingManager.SetCanvasEnable(_currentChoice == OptionChoices.Sound);
        _operationManager.SetCanvasEnable(_currentChoice == OptionChoices.Operation);
        _otherSettingsManager.SetCanvasEnable(_currentChoice == OptionChoices.Other);
    }

    private void SetFontSize()
    {
        for (int i = 0; i < _optionTexts.Length; i++)
        {
            float size = (int)_currentChoice - 1 == i ? 65f : 55.5f;

            _optionTexts[i].TextData.SetFontSize(size);
        }
    }
}