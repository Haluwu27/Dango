using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TM.UI.Text;

/// <summary>
/// テキストの基本クラス。設定の際はTextDataを参照。
/// </summary>
public class TextUIData : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUGUI = default!;

    TextData _textData;
    public TextData TextData
    {
        get
        {
            _textData ??= new(textUGUI);
            return _textData;
        }
        private set => _textData = value;
    }

    private void Awake()
    {
        _textData ??= new(textUGUI);
    }
}