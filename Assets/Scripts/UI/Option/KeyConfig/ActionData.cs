using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TM.Input.KeyConfig
{
    class ActionData
    {
        //このデータに対応する動的なアクション
        KeyData.GameAction _action;

        //現在のこのアクションに対するキー
        List<KeyData.GamepadKey> _currentKeys = new();

        //動的なアクションのリファレンス(example : Player/Move)
        //これだけで制御するのは直感的ではないためGameActionも採用しています
        InputActionReference _actionReference = ScriptableObject.CreateInstance<InputActionReference>();

        //アクションマップの名前。想定される名前は"Player"やUI等
        string _actionMapName;

        public ActionData(KeyData.GameAction action, KeyData.GamepadKey[] key, InputActionAsset asset, string actionMapName)
        {
            _action = action;
            _currentKeys.AddRange(key);
            _actionMapName = actionMapName;
            SetActionReference(action, asset);
        }

        //ActionReferenceを動的に設定する関数。外部使用は想定していません。
        private void SetActionReference(KeyData.GameAction action, InputActionAsset asset)
        {
            _actionReference.Set(asset, _actionMapName, ToActionName(action, asset));
        }

        //アセットのアクション名に合わせて変換する関数。外部使用は想定していません。
        private string ToActionName(KeyData.GameAction action, InputActionAsset asset)
        {
            return asset.FindActionMap(_actionMapName).actions[(int)action].name;
        }

        /// <summary>
        /// 指定のキーをアクションに含んでいるか判定するメソッド
        /// </summary>
        /// <param name="key">判定したいキー</param>
        /// <returns>true:含んでいる</returns>
        public bool IsContainsKey(KeyData.GamepadKey key)
        {
            return _currentKeys.Contains(key);
        }

        /// <summary>
        /// アクションに対してキーが割り当てられているか判定するメソッド
        /// </summary>
        /// <returns>true:割り当てあり</returns>
        public bool HasKey()
        {
            return _currentKeys.Count > 0;
        }

        public KeyData.GameAction Action => _action;
        public InputActionReference ActionReference => _actionReference;
        public List<KeyData.GamepadKey> Keys => _currentKeys;
    }
}