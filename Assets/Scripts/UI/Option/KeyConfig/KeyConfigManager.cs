using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TM.Input.KeyConfig
{
    enum InputKey
    {
        Up,
        UpperRight,
        Right,
        LowerRight,
        Down,
        LowerLeft,
        Left,
        UpperLeft,

        Unknown,
    }

    public class KeyConfigManager : MonoBehaviour
    {
        //現在選択されている変更したいキーのデータ
        KeyConfigData _currentData;

        [SerializeField, Tooltip("初期に選択されているキー")] KeyConfigData _firstData = default!;

        //スティックやキーボードの移動入力読み取り用
        Vector2 _axis;

        //表示・非表示切り替え用に管理するもの
        [SerializeField] Canvas _staticCanvas = default!;
        [SerializeField] Canvas _dynamicCanvas = default!;
        [SerializeField] KeyConfigWarningManager _warningManager = default!;
        [SerializeField] KeyConfigPopupManager _popupManager = default!;

        //InputSystemのInputActions本体
        [SerializeField] InputActionAsset _asset;

        //アクションリファレンス毎に設定するデータの全要素
        readonly ActionData[] _actionDatas = new ActionData[(int)KeyConfigData.GameAction.Max];
        readonly List<KeyData.GameAction> _gameActions = new();

        public void OnStick()
        {
            if (!_staticCanvas.isActiveAndEnabled) return;
            if (!_dynamicCanvas.isActiveAndEnabled) return;
            if (_popupManager.IsPopup) return;
            if (_warningManager.IsWarning) return;

            //読み取った値を保存
            _axis = InputSystemManager.Instance.StickAxis;

            //読み取った値から最も近い8方向のいずれかを返却
            InputKey key = CheckInputKey(_axis);
            if (key == InputKey.Unknown) return;

            ///仮に下がnullだった場合axis.xで調整とかするならここに条件分岐するしか今のところ無理。
            ///とりあえず違和感のある選択にはなるかもしれないが、こういう感じで。
            //nullチェック。nullの場合は上記設定を行う方が自然である。
            if (_currentData.GetKeyConfigDatas()[(int)key] == null) return;

            //ここをポップアップに変える
            _currentData.GetComponent<RawImage>().color = Color.white;

            //現在のデータを選択したデータに変更
            _currentData = _currentData.GetKeyConfigDatas()[(int)key];

            //ここをポップアップに変える
            _currentData.GetComponent<RawImage>().color = Color.red;

            Logger.Log(_currentData.name);
        }

        static readonly KeyData.GamepadKey[][] DefaultKeyTable = new KeyData.GamepadKey[(int)KeyConfigData.GameAction.Max][]
        {
           new KeyData.GamepadKey[] {KeyData.GamepadKey.LStick },                                       //Move
           new KeyData.GamepadKey[] {KeyData.GamepadKey.RStick },                                       //Look
           new KeyData.GamepadKey[] {KeyData.GamepadKey.ButtonSouth,KeyData.GamepadKey.ButtonEast },    //Jump
           new KeyData.GamepadKey[] {KeyData.GamepadKey.ButtonNorth,KeyData.GamepadKey.ButtonWest },    //Attack
           new KeyData.GamepadKey[] {KeyData.GamepadKey.LTrigger },                                     //EatDango
           new KeyData.GamepadKey[] {KeyData.GamepadKey.RTrigger },                                     //Fire
           new KeyData.GamepadKey[] {KeyData.GamepadKey.Start,KeyData.GamepadKey.Select },              //Option
           new KeyData.GamepadKey[] {KeyData.GamepadKey.R },                                            //UIExtra(not Found)
        };

        private void Awake()
        {
            InitConfigData();

            for (int i = 0; i < (int)KeyConfigData.GameAction.Max; i++)
            {
                _actionDatas[i] = new((KeyData.GameAction)i, DefaultKeyTable[i], _asset, "Player");
            }
        }

        private void Start()
        {
            InputSystemManager.Instance.onStickPerformed += OnStick;
            InputSystemManager.Instance.onChoicePerformed += OnSelect;
            InputSystemManager.Instance.onBackCanceled += OnBack;
        }

        public void OnChangeScene()
        {
            _popupManager.OnChangeScene();
            InputSystemManager.Instance.onStickPerformed -= OnStick;
            InputSystemManager.Instance.onChoicePerformed -= OnSelect;
            InputSystemManager.Instance.onBackCanceled -= OnBack;
        }

        //この関数をAwakeに置けば直前に抜けた地点が保存されてそこから移動できます。
        //逆に毎回初期化したい場合はOnEnableに配置してください。
        private void InitConfigData()
        {
            _currentData = _firstData;
        }

        const float RAD = 90f;

        private InputKey CheckInputKey(Vector2 axis)
        {
            //八分割してそれぞれ振り分けています。

            if (axis.x > 0)
            {
                if (axis.y is >= -90f / RAD and < -67.5f / RAD) return InputKey.Down;
                if (axis.y is >= -67.5f / RAD and < -22.5f / RAD) return InputKey.LowerRight;
                if (axis.y is >= -22.5f / RAD and < 22.5f / RAD) return InputKey.Right;
                if (axis.y is >= 22.5f / RAD and < 67.5f / RAD) return InputKey.UpperRight;
                if (axis.y is >= 67.5f / RAD and <= 90.0f / RAD) return InputKey.Up;
            }
            if (axis.x <= 0)
            {
                if (axis.y is >= -90.0f / RAD and < -67.5f / RAD) return InputKey.Down;
                if (axis.y is >= -67.5f / RAD and < -22.5f / RAD) return InputKey.LowerLeft;
                if (axis.y is >= -22.5f / RAD and < 22.5f / RAD) return InputKey.Left;
                if (axis.y is >= 22.5f / RAD and < 67.5f / RAD) return InputKey.UpperLeft;
                if (axis.y is >= 67.5f / RAD and <= 90.0f / RAD) return InputKey.Up;
            }

            Logger.Error("[Error]:割り振られていないものが存在しています。");
            return InputKey.Unknown;
        }

        /// <summary>
        /// Canvasの表示・非表示を設定する関数
        /// </summary>
        public void SetCanvasEnable(bool enable)
        {
            _staticCanvas.enabled = enable;
            _dynamicCanvas.enabled = enable;

            if (enable)
            {
                if (_currentData != null)  _currentData.GetComponent<RawImage>().color = Color.white;
                
                _currentData = _firstData;
                _currentData.GetComponent<RawImage>().color = Color.red;
            }
        }

        private void OnSelect()
        {
            if (!_staticCanvas.isActiveAndEnabled) return;
            if (!_dynamicCanvas.isActiveAndEnabled) return;
            if (IsPopup) return;
            if (_warningManager.IsWarning) return;

            _popupManager.OnCanvasEnabled();
        }

        public void OnBack()
        {
            if (!_staticCanvas.isActiveAndEnabled) return;
            if (!_dynamicCanvas.isActiveAndEnabled) return;

            if (_popupManager.IsPopup)
            {
                _popupManager.OnCanvasDisabled();
                return;
            }

            CheckHasKeyAllActions();
        }

        public void Rebinding(int index)
        {
            foreach (var actionData in _actionDatas)
            {
                if (_currentData.KeyData.CurrentActionReference == null) break;

                if (actionData.ActionReference.ToString().Equals(_currentData.KeyData.CurrentActionReference.ToString()))
                    actionData.Keys.Remove(_currentData.KeyData.Key);
            }
            _actionDatas[index].Keys.Add(_currentData.KeyData.Key);

            _currentData.KeyData.KeyBindingOverride(_actionDatas[index].ActionReference);
        }

        public bool CheckHasKeyAllActions()
        {
            if (_warningManager.IsWarning) return false;

            bool hasKey = true;
            _gameActions.Clear();

            //すべてのアクションに対して割り当てが存在しているかチェック
            foreach (var actionData in _actionDatas)
            {
                //割り当てがなかった場合
                if (!actionData.HasKey())
                {
                    _gameActions.Add(actionData.Action);
                    hasKey = false;
                }
            }
            if (!hasKey) HasAllAction();

            return hasKey;
        }

        private async void HasAllAction()
        {
            _warningManager.SetEnable(true);
            _warningManager.SetText(_gameActions);
            await WaitForAnyKey();
            _warningManager.SetEnable(false);
        }

        private async UniTask WaitForAnyKey()
        {
            await UniTask.Yield();

            while (true)
            {
                await UniTask.Yield();
                if (InputSystemManager.Instance.WasPressedThisFrameAnyKey) break;
            }
        }

        public KeyConfigData Data => _currentData;
        public bool IsPopup => _popupManager.IsPopup;
    }
}