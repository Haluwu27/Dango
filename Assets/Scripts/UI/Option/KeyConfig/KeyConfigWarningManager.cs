using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Input.KeyConfig
{
    public class KeyConfigWarningManager : MonoBehaviour
    {
        [SerializeField] Canvas _canvas = default!;
        [SerializeField] TextUIData _textUIData = default!;

        public void SetEnable(bool enable)
        {
            _canvas.enabled = enable;
        }

        public void SetText(List<KeyData.GameAction> gameActions)
        {
            string action = "";
            foreach (var gameAction in gameActions)
            {
                action += gameAction.ToString();
            }
            _textUIData.TextData.SetText("ボタンが割り当てられていない機能があります。最低でも1つのボタンに割り当ててください。\n" + action);
        }

        public bool IsWarming => _canvas.enabled;
    }
}