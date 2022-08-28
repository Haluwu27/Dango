using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField, Tooltip("空腹度テキスト")] TextMeshProUGUI timeText;
    [SerializeField, Tooltip("イベントテキスト")] TextMeshProUGUI eventText;
    [SerializeField, Tooltip("空腹度ゲージ")] Slider[] timeGage;
    public static float time = 0;
    private float maxTime;
    private float currentTime;
    private int[] warningTimes = new int[3];

    private RectTransform textrect;
    private GameObject TimeObj;
    private bool[] warningbool = new bool[3];

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    public void SetEventText(string text)
    {
        eventText.text = text;
    }

    private void Start()
    {
        maxTime = time;
        currentTime = maxTime;
        for(int i=0;i<timeGage.Length;i++)
            timeGage[i].value = 1;
        for (int i = 0; i < 3; i++)
            warningTimes[i] = 30 - (i * 10);//仮で入れてます

        TimeObj = timeGage[1].gameObject;
        textrect = timeGage[1].GetComponent<RectTransform>();
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

        for (int i = 0; i < warningTimes.Length; i++)
        {
            if (time <= warningTimes[i] && !warningbool[i])
            {
                textrect.sizeDelta *= 1.2f;
                warningbool[i] = true;
            }
            else if (time >= warningTimes[i]&&warningbool[i])
            {
                textrect.sizeDelta /= 1.2f;
                warningbool[i] = false;
            }
        }


        for (int i = 0; i < timeGage.Length; i++)
            timeGage[i].value = (float)currentTime / (float)maxTime;
        SetTimeText(""+(int)time);
    }
}
