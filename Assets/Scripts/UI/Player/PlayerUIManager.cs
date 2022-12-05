using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField, Tooltip("空腹度テキスト")] TextMeshProUGUI timeText;
    [SerializeField, Tooltip("イベントテキスト")] TextUIData eventText;
    [SerializeField, Tooltip("空腹度ゲージ")] Slider[] timeGage;
    [SerializeField, Tooltip("制限時間残量警告画像")] Sprite[] Warningimgs;
    [SerializeField, Tooltip("制限時間残量警告オブジェクト")] Image W_obj;
    [SerializeField] PlayerData playerdata;
    [SerializeField] GameObject MAXDangoOBJ;
    [SerializeField] Animator[] TimegageAnima;
    [SerializeField] TimeGageAnima[] timeGageAnimaText;
    [SerializeField] ImageUIData DontEatUIOBJ;
    [SerializeField] Sprite dontEatSprite;
    [SerializeField] DangoUIScript dangoUIScript;
    [SerializeField] ImageUIData dangoHighlight;
    [SerializeField] bool tutorial;
    private float time { get { return playerdata.GetSatiety(); } }
    private float maxTime;
    private float currentTime;
    private int[] warningTimes = new int[3];

    RoleCheck roleCheck =new RoleCheck();

    private Image[] w_imgs;

    [SerializeField] GameObject[] ErasewithEatObj = new GameObject[3];//食事した際にきえるUI 

    private bool[] warningbool = new bool[3];
    public TextUIData EventText => eventText;

    public bool Expansion;

    public float DefaultEventTextFontSize { get; } = 100f;

    float temp=0;

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    private void Start()
    {
        maxTime = time;
        currentTime = maxTime;

        if(!tutorial)
        for (int i = 0; i < timeGage.Length; i++)
            timeGage[i].value = 1;
        for (int i = 0; i < warningTimes.Length - 1; i++)
            warningTimes[i] = (int)maxTime - ((i + 1) * 10);//仮で初期値の2/3,1/3の値

        w_imgs = new Image[Warningimgs.Length];
        warningbool = new bool[warningTimes.Length];

        //GameObject.Findを使うならシリアライズして取得しましょう。その方が名前に縛られず確実です。
        //ErasewithEatObj[0] = GameObject.Find("DangoBackScreen");
        //ErasewithEatObj[1] = GameObject.Find("QuestCanvas");
        //ErasewithEatObj[2] = GameObject.Find("PlayerParent").transform.Find("Player1").transform.Find("RangeUI").gameObject;
    }
    private void Update()
    {
        currentTime = time;
        if (!PlayerData.Event)
            ScoreManager.Instance.AddTime();

        Warning();

        if (!tutorial)
        {
            for (int i = 0; i < timeGage.Length; i++)//ゲージの増減
                timeGage[i].value = (float)currentTime / (float)maxTime;
            SetTimeText("" + (int)time);
        }
    }

    private void Warning()
    {
        if (time > warningTimes[0])
        {
            W_obj.gameObject.SetActive(false);
        }
        for (int i = 0; i < warningTimes.Length; i++)
        {
            if (time < warningTimes[i] && !warningbool[i])
            {
                W_obj.gameObject.SetActive(true);
                W_obj.sprite = Warningimgs[i];
                warningbool[i] = true;
            }
            else if (time >= warningTimes[i] && warningbool[i])//一段階上がった際の処理
            {
                W_obj.gameObject.SetActive(true);
                W_obj.sprite = Warningimgs[i];
                warningbool[i] = false;
            }
        }
    }
    public void EatDangoUI_False()
    {
        for (int i = 0; i < ErasewithEatObj.Length; i++)
            ErasewithEatObj[i].SetActive(false);
    }

    public void EatDangoUI_True()
    {
        for (int i = 0; i < ErasewithEatObj.Length; i++)
        {
            if (i == 2 && Expansion) continue;

            ErasewithEatObj[i].SetActive(true);
        }
        TimeGageUpAnima();
    }

    public void MAXDangoSet(bool Active)
    {
        if (Active)
            MAXDangoOBJ.SetActive(true);
        else
            MAXDangoOBJ.SetActive(false);
    }

    public void ScoreCatch(float score)
    {
        temp += score;
    }

    public void TimeGageUpAnima()
    {
        if(!tutorial)
        for (int i = 0; i < timeGageAnimaText.Length; i++)
        {
            timeGageAnimaText[i].SetText(temp);
            TimegageAnima[i].SetBool("Play",true);
        }
        temp = 0;
    }

    public void DontEat()
    {
        DontEatUIOBJ.ImageData.SetSprite(dontEatSprite);
    }

    public void SetReach()
    {
        if (playerdata.GetDangos().Count == playerdata.GetCurrentStabCount() - 1)
        {
            DangoColor color = roleCheck.GetReach(playerdata.GetDangos(), playerdata.GetCurrentStabCount());
            if (color != DangoColor.None)
            {
                dangoUIScript.ReachSet(color, playerdata.GetCurrentStabCount());
                Logger.Log("reach判定：" + color);
            }
        }
        else
            dangoUIScript.ReachClose();
    }

    public void StartDangoHighlight()
    {
        dangoHighlight.ImageData.FlashAlpha(-1f, 0.2f, 0f);
    }

    public void CancelHighlight()
    {
        dangoHighlight.ImageData.CancelFlash();
    }
}
