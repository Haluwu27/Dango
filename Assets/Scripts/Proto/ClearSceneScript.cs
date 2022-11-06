//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//public class ClearSceneScript : MonoBehaviour
//{
//    private enum Next
//    {
//        retry,
//        stageSelect,
//        Max,
//    }

//    [SerializeField] GameObject[] Ranks;
//    [SerializeField] TextMeshProUGUI text;
//    [SerializeField] GameObject QuestBlokObj;
//    [SerializeField] GameObject QuestFlowChart;
//    [SerializeField] ImageUIData[] retryOrSelect;
//    float time;
//    List<QuestData> dangoDatas = new List<QuestData>();
//    List<float> dangoClearTime = new List<float>();
//    Next next;
//    bool canMove = true;
//    Vector2 axis;
//    bool _isFire;
//    bool canAxis = true;
//    bool temp = true;
//    void Start()
//    {
//        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");

//        dangoDatas = ScoreManager.Instance.GetClearQuest();
//        dangoClearTime = ScoreManager.Instance.GetClearTime();
//        CreateBloks();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        time = ScoreManager.Instance.GetTime();
//        axis = InputSystemManager.Instance.NavigateAxis;
//        _isFire = InputSystemManager.Instance.IsPressChoice;

//        canChange();

//        if (time < 180)
//            SetObj(Ranks[0]);
//        else if (time < 240)
//            SetObj(Ranks[1]);
//        else
//            SetObj(Ranks[2]);

//        text.text = time.ToString("f1");

//        if (_isFire)
//        {
//            switch (next)
//            {
//                case Next.retry:
//                    SceneSystem.Instance.Load(SceneSystem.Instance.CurrentIngameScene);
//                    SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Success, true);
//                    break;
//                case Next.stageSelect:
//                    SceneSystem.Instance.Load(SceneSystem.Scenes.StageSelect);
//                    SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Success, true);
//                    break;
//            }
//        }
//    }

//    private void SetObj(GameObject obj)
//    {
//        obj.SetActive(true);
//    }
//    private void CreateBloks()
//    {
//        for (int i = 0; i < dangoDatas.Count; i++)
//        {
//            GameObject obj = Instantiate(QuestBlokObj, QuestFlowChart.transform.position, Quaternion.identity, QuestFlowChart.transform);
//            obj.GetComponent<ClearQuestBlokScript>().SetText(dangoDatas[i], dangoClearTime[i]);
//        }
//    }

//    private void Change(Vector2 axis)
//    {

//        if (!ChangeChoiceUtil.Choice(axis, ref next, Next.Max, canMove, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

//        for (int i = 0; i < retryOrSelect.Length; i++)
//        {
//            retryOrSelect[i].ImageData.SetColor(Color.white);
//        }
//        retryOrSelect[(int)next].ImageData.SetColor(Color.red);
//    }

//    private void canChange()
//    {
//        if (canAxis)
//        {
//            Change(axis);
//            canAxis = false;
//        }

//        if (axis.x > 0 && temp)
//        {
//            canAxis = true;
//            temp = false;
//        }

//        if (axis.x < 0 && temp)
//        {
//            canAxis = true;
//            temp = false;
//        }
//        if (axis.x >= -0.3 && 0.3 >= axis.x)
//            temp = true;
//    }
//}

//ある程度原型を留めてリファクタリングしておきました
//まだ気になるところはありますが、とりあえずどんな工夫がされているかなど学んでください

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ClearSceneScript : MonoBehaviour
{
    private enum Next
    {
        Retry,
        StageSelect,

        Max,
    }

    [SerializeField] GameObject[] Ranks;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject QuestBlokObj;
    [SerializeField] GameObject QuestFlowChart;
    [SerializeField] ImageUIData[] retryOrSelect;
    [SerializeField] FusumaManager _fusumaManager;
    [SerializeField] Canvas _canvas;

    Next _next;
    static readonly bool _canMoveTopToBottom = true;

    private async void Awake()
    {
        //演出終了まで描画しない
        _canvas.enabled = false;

        //初期設定を行う
        SetRetryOrSelectColor();
        SetQusetFlowchart();
        CreateBloks();

        //ふすまの演出
        await _fusumaManager.UniTaskClose();

        //演出終了後に描画
        _canvas.enabled = true;

        //裏で生きていたインゲームを削除
        SceneSystem.Instance.UnLoad(SceneSystem.Instance.CurrentIngameScene, true);
    }

    void Start()
    {
        //ほぼ存在しているがこのシーンを最初に持ってきた場合Instanceがnullの場合があるためStartで行います
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
    }

    private void CreateBloks()
    {
        List<QuestData> dangoDatas = ScoreManager.Instance.GetClearQuest();
        List<float> dangoClearTime = ScoreManager.Instance.GetClearTime();

        for (int i = 0; i < dangoDatas.Count; i++)
        {
            GameObject obj = Instantiate(QuestBlokObj, QuestFlowChart.transform.position, Quaternion.identity, QuestFlowChart.transform);
            obj.GetComponent<ClearQuestBlokScript>().SetText(dangoDatas[i], dangoClearTime[i]);
        }
    }

    private void OnNavigate()
    {
        Vector2 axis = InputSystemManager.Instance.NavigateAxis;

        if (!ChangeChoiceUtil.Choice(axis, ref _next, Next.Max, _canMoveTopToBottom, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

        SetRetryOrSelectColor();
    }

    private void OnChoice()
    {
        switch (_next)
        {
            case Next.Retry:
                SceneSystem.Instance.Load(SceneSystem.Instance.CurrentIngameScene);
                SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Success, true);
                break;
            case Next.StageSelect:
                SceneSystem.Instance.Load(SceneSystem.Scenes.StageSelect);
                SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Success, true);
                break;
        }
    }

    private void SetRetryOrSelectColor()
    {
        for (int i = 0; i < retryOrSelect.Length; i++)
        {
            retryOrSelect[i].ImageData.SetColor(Color.white);
        }
        retryOrSelect[(int)_next].ImageData.SetColor(Color.red);
    }

    private void SetObj(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void SetQusetFlowchart()
    {
        float time = ScoreManager.Instance.GetTime();

        if (time < 180)
            SetObj(Ranks[0]);
        else if (time < 240)
            SetObj(Ranks[1]);
        else
            SetObj(Ranks[2]);

        text.text = time.ToString("f1");
    }
}
