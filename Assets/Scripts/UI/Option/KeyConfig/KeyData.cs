using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TM.Input.KeyConfig
{
    public class KeyData
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

            [InspectorName("")]
            Max,
        }

        public enum GameAction
        {
            Unknown,

            Move,
            LookRotation,
            Jump,
            Attack,
            Eat,
            Remove,
            Pause,
            UIExpansion,
        }

        public KeyData(GamepadKey key, InputActionReference currentActionReference, Func<InputActionReference, GameAction> action)
        {
            _key = key;
            _currentActionReference = currentActionReference;
            actionFunc = action;
        }

        GamepadKey _key;
        InputActionReference _currentActionReference;
        Func<InputActionReference, GameAction> actionFunc;

        public InputActionReference CurrentActionReference => _currentActionReference;
        public GamepadKey Key => _key;
        public GameAction Action => actionFunc(_currentActionReference);

        /// <summary>
        /// 選択されているKeyDataのバインドを変更する関数。
        /// </summary>
        public void KeyBindingOverride(InputActionReference actionReference)
        {
            if (_currentActionReference != null)
            {
                _currentActionReference.action.ChangeBinding(new InputBinding { path = ToGamepadKeyPass(_key) }).Erase();
            }
            if (actionReference != null) actionReference.action.AddBinding(new InputBinding { path = ToGamepadKeyPass(_key) });
            _currentActionReference = actionReference;

            //Logger.Log("キーバインドを変更したよ！");
        }

        public string ToGamepadKeyPass(GamepadKey gamepadKey)
        {
            return gamepadKey switch
            {
                GamepadKey.ButtonNorth => "<Gamepad>/buttonNorth",
                GamepadKey.ButtonEast => "<Gamepad>/buttonEast",
                GamepadKey.ButtonWest => "<Gamepad>/buttonWest",
                GamepadKey.ButtonSouth => "<Gamepad>/buttonSouth",
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