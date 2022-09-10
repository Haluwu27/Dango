using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TM.Input.KeyConfig
{
    public class KeyData : MonoBehaviour
    {
        public enum GamepadKey
        {
            ButtonNorth,
            ButtonEast,
            ButtonWest,
            ButtonSouth,
            L,
            R,
            LTrigger,
            RTrigger,
            LStick,
            LStickDown,
            LStickUp,
            LStickLeft,
            LStickRight,
            LStickPress,
            RStick,
            RStickDown,
            RStickUp,
            RStickLeft,
            RStickRight,
            RStickPress,
            D_pad,
            D_padDown,
            D_padUp,
            D_padLeft,
            D_padRight,
            Select,
            Start,
        }

        public enum GameAction
        {
            Move = 0,
            LookRotation = 1,
            Jump = 2,
            Attack = 3,
            Eat = 4,
            Remove = 5,
            Pause = 6,
            UIExpansion = 7,
        }

        [SerializeField] GamepadKey key;
        [SerializeField] GameAction defalutAction;

        [SerializeField] InputActionReference currentActionReference;

        public InputActionReference CurrentActionReference => currentActionReference;
        public GamepadKey Key => key;

        //static readonly string REBINDJSON = "rebindKey";

        //バインディング状態をJSON形式で出力、読み込み
        //をしたいんだけどひとまずわからないので一旦放置

        //private bool LoadRebinding()
        //{
        //    //すでにリバインディングしたことがある場合はシーン読み込み時に変更。
        //    string rebinds = PlayerPrefs.GetString(REBINDJSON);

        //    //Nullか空白であったらロード失敗として返却
        //    if (string.IsNullOrEmpty(rebinds)) return false;

        //    //リバインディング状態をロード
        //    _playerInput.actions.LoadBindingOverridesFromJson(rebinds);


        //    return true;
        //}

        //public void RebindComplete()
        //{
        //    //fireアクションの1番目のコントロール(バインディングしたコントロール)のインデックスを取得
        //    int bindingIndex = _action.action.GetBindingIndexForControl(_action.action.controls[0]);

        //    //バインディングしたキーの名称を取得する
        //    _bindingName.text = InputControlPath.ToHumanReadableString(
        //        _action.action.bindings[bindingIndex].effectivePath,
        //        InputControlPath.HumanReadableStringOptions.OmitDevice);

        //    _rebindingOperation.Dispose();

        //    //画面を通常に戻す
        //    _rebindingButton.SetActive(true);
        //    _rebindingMessage.SetActive(false);

        //    //リバインディング時は空のアクションマップだったので通常のアクションマップに切り替え
        //    _pInput.SwitchCurrentActionMap("Player");

        //    //リバインディングしたキーを保存(シーン開始時に読み込むため)
        //    PlayerPrefs.SetString(REBINDJSON, _playerInput.actions.SaveBindingOverridesAsJson());
        //}

        /// <summary>
        /// 選択されているKeyDataのバインドを変更する関数。
        /// </summary>
        public void KeyBindingOverride(InputActionReference actionReference)
        {
            if (currentActionReference != null)
            {
                currentActionReference.action.ChangeBinding(new InputBinding { path = ToGamepadKeyPass(key) }).Erase();
            }
            actionReference.action.AddBinding(new InputBinding { path = ToGamepadKeyPass(key) });
            currentActionReference = actionReference;

            Logger.Log("キーバインドを変更したよ！");
        }

        private string ToGamepadKeyPass(GamepadKey gamepadKey)
        {
            return gamepadKey switch
            {
                GamepadKey.ButtonNorth => "<Gamepad>/buttonNorth",
                GamepadKey.ButtonEast => "<Gamepad>/buttonEast",
                GamepadKey.ButtonWest => "<Gamepad>/buttonSouth",
                GamepadKey.ButtonSouth => "<Gamepad>/buttonWest",
                GamepadKey.L => "<Gamepad>/leftShoulder",
                GamepadKey.R => "<Gamepad>/rightShoulder",
                GamepadKey.LTrigger => "<Gamepad>/leftTrigger",
                GamepadKey.RTrigger => "<Gamepad>/rightTrigger",
                GamepadKey.LStick => "<Gamepad>/leftStick",
                GamepadKey.LStickDown => "<Gamepad>/leftStick/down",
                GamepadKey.LStickUp => "<Gamepad>/leftStick/up",
                GamepadKey.LStickLeft => "<Gamepad>/leftStick/left",
                GamepadKey.LStickRight => "<Gamepad>/leftStick/right",
                GamepadKey.LStickPress => "<Gamepad>/leftStickPress",
                GamepadKey.RStick => "<Gamepad>/rightStick",
                GamepadKey.RStickDown => "<Gamepad>/rightStick/down",
                GamepadKey.RStickUp => "<Gamepad>/rightStick/up",
                GamepadKey.RStickLeft => "<Gamepad>/rightStick/left",
                GamepadKey.RStickRight => "<Gamepad>/rightStick/right",
                GamepadKey.RStickPress => "<Gamepad>/rightStickPress",
                GamepadKey.D_pad => "<Gamepad>/dpad",
                GamepadKey.D_padDown => "<Gamepad>/dpad/down",
                GamepadKey.D_padUp => "<Gamepad>/dpad/up",
                GamepadKey.D_padLeft => "<Gamepad>/dpad/left",
                GamepadKey.D_padRight => "<Gamepad>/dpad/right",
                GamepadKey.Select => "<Gamepad>/select",
                GamepadKey.Start => "<Gamepad>/start",
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}