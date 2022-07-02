using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField, Tooltip("空腹度テキスト")] TextMeshProUGUI timeText;
    [SerializeField, Tooltip("イベントテキスト")] TextMeshProUGUI eventText;

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    public void SetEventText(string text)
    {
        eventText.text = text;
    }
}
