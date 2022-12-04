using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Input.KeyConfig
{
    public class KeyConfigWarningManager : MonoBehaviour
    {
        [SerializeField] Canvas _canvas;
        [SerializeField] TextUIData _textUIData;
        [SerializeField] KeyConfigPopupManager _popupManager;
       
        public void SetEnable(bool enable)
        {
            _canvas.enabled = enable;
        }

        public void SetText(List<KeyData.GameAction> gameActions)
        {
            string action = "";
            foreach (var gameAction in gameActions)
            {
                action += _popupManager.ActionString((int)gameAction) + ", ";
            }
            _textUIData.TextData.SetText("ボタンに割り当てられていないアクションがあります。最低でも1つのボタンに割り当ててください。\n" + action);
        }

        public bool IsWarning => _canvas.enabled;
    }
}