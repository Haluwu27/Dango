using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TM.Input.KeyConfig;
using Unity.VisualScripting;

public class OptionManager : MonoBehaviour
{
    /// <summary>オプション画面の選択肢</summary>
    enum OptionChoices
    {
        Option,

        TEMP,
        TEMP2,
        TEMP3,
        KeyConfig,

        Max,
    }

    enum OptionDirection
    {
        Vertical,
        Horizontal,
    }

    #region メンバ
    [SerializeField] KeyConfigManager _keyConfig = default!;
    [SerializeField] Canvas _canvas = default!;

    /// <summary>
    /// 静的に取得出来るオプションキャンバス
    /// </summary>
    /// 単一シーンで実装するため、各画面間のやりとりの際に使用する静的なものです。
    public static Canvas OptionCanvas { get; private set; }

    //現在のオプション地点
    OptionChoices _currentChoice = OptionChoices.Option;

    //次に以降するオプション地点
    OptionChoices _nextChoice = OptionChoices.TEMP;

    //縦か横か
    static readonly OptionDirection direction = OptionDirection.Vertical;
    static readonly Vector2[,] directionTable = { { Vector2.up, Vector2.down }, { Vector2.left, Vector2.right } };

    #endregion

    #region InputSystem
    private void OnBack()
    {
        if (_currentChoice == OptionChoices.Option)
        {
            //Player(通常プレイの入力)マップに戻る
            InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");

            //Bボタンだけ一瞬で戻ってジャンプしてしまうバグあり。原因不明

            _canvas.enabled = false;
        }
        else if (_currentChoice == OptionChoices.KeyConfig)
        {
            //ポップアップ中なら別処理を挟む
            if (!_keyConfig.OnBack()) return;

            _currentChoice = OptionChoices.Option;
            EnterOption();
        }
        else
        {
            _currentChoice = OptionChoices.Option;
            EnterOption();
        }
    }

    private void OnNavigate()
    {
        Vector2 axis = InputSystemManager.Instance.NavigateAxis;

        switch (_currentChoice)
        {
            case OptionChoices.Option:
                ChangeChoice(axis);
                break;
            case OptionChoices.TEMP:
                break;
            case OptionChoices.TEMP2:
                break;
            case OptionChoices.TEMP3:
                break;
            case OptionChoices.KeyConfig:
                break;
            case OptionChoices.Max:
                break;
        }
    }
    #endregion

    private void Awake()
    {
        OptionCanvas = _canvas;

       // PlayerData.MyPlayerInput.
    }

    private void Start()
    {
        InputSystemManager.Instance.onBackPerformed += OnBack;
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += Choiced;
    }

    private void ChangeChoice(Vector2 axis)
    {
        //縦のシステムにする？？
        if (axis == directionTable[(int)direction, 0])
        {
            _nextChoice--;
            if (_nextChoice == OptionChoices.Option) _nextChoice = OptionChoices.Max - 1;
            Logger.Log(_nextChoice);

        }
        if (axis == directionTable[(int)direction, 1])
        {
            _nextChoice++;
            if (_nextChoice == OptionChoices.Max) _nextChoice = OptionChoices.Option + 1;

            Logger.Log(_nextChoice);
        }

    }

    private void Choiced()
    {
        switch (_currentChoice)
        {
            case OptionChoices.Option:
                Logger.Log("選択したよ");
                EnterNextChoice();

                break;
            case OptionChoices.TEMP:
                break;
            case OptionChoices.TEMP2:
                break;
            case OptionChoices.TEMP3:
                break;
            case OptionChoices.KeyConfig:
                _keyConfig.OnSelect();
                break;
        }
    }

    private void EnterNextChoice()
    {
        switch (_nextChoice)
        {
            case OptionChoices.TEMP:
                break;
            case OptionChoices.TEMP2:
                break;
            case OptionChoices.TEMP3:
                break;
            case OptionChoices.KeyConfig:
                EnterKeyConfig();
                break;
        }
    }

    private void EnterOption()
    {
        _keyConfig.SetCanvasEnable(false);
    }

    private void EnterKeyConfig()
    {
        _currentChoice = OptionChoices.KeyConfig;
        _keyConfig.SetCanvasEnable(true);
    }
}