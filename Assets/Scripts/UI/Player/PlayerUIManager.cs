using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField, Tooltip("空腹度テキスト")] TextMeshProUGUI timeText;
    [SerializeField, Tooltip("イベントテキスト")] EventTextUI eventText;
    [SerializeField, Tooltip("空腹度ゲージ")] Slider timeGage;
    public static float time = 0;
    private float maxTime;
    private float currentTime;

    public EventTextUI EventText => eventText;

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    private void Start()
    {
        maxTime = time;
        currentTime = maxTime;
        timeGage.value = 1;
    }
    private void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            //ゲームオーバー処理
        }
        currentTime = time;
        timeGage.value = (float)currentTime / (float)maxTime;
        SetTimeText("" + (int)time);
    }
}
