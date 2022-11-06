using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ClearSceneScript : MonoBehaviour
{
    private enum Next
    {
        retry,
        stageSelect,

        Max,
    }
    [SerializeField] GameObject[] Ranks;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject QuestBlokObj;
    [SerializeField]GameObject QuestFlowChart;
    [SerializeField] ImageUIData[] retryOrSelect;
    float time;
    List<QuestData> dangoDatas = new List<QuestData>();
    List<float> dangoClearTime = new List<float>();
    Next next;
    bool canMove=true;
    Vector2 axis;
    bool _isFire;
    bool canAxis=true;
    bool temp = true;
    void Start()
    {
        dangoDatas = ScoreManager.Instance.GetClearQuest();
        dangoClearTime =ScoreManager.Instance.GetClearTime();
        CreateBloks();
    }

    // Update is called once per frame
    void Update()
    {
        time = ScoreManager.Instance.GetTime();
        axis = InputSystemManager.Instance.MoveAxis;
        _isFire = InputSystemManager.Instance.IsPressAttack;

        canChange();

        if (time < 180)
            SetObj(Ranks[0]);
        else if (time < 240)
            SetObj(Ranks[1]);
        else
            SetObj(Ranks[2]);

        text.text = time.ToString("f1");

        if (_isFire)
        {
            switch (next)
            {
                case Next.retry:
                    SceneSystem.Instance.Load(SceneSystem.Scenes.Stage2);
                    SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Success, true);
                    break;
                case Next.stageSelect:
                    SceneSystem.Instance.Load(SceneSystem.Scenes.StageSelect);
                    SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Success, true);
                    break;
            }
        }
    }

    private void SetObj(GameObject obj)
    {
        obj.SetActive(true);
    }
    private void CreateBloks()
    {
        for(int i=0;i< dangoDatas.Count; i++)
        {
            GameObject obj = Instantiate(QuestBlokObj, QuestFlowChart.transform.position, Quaternion.identity,QuestFlowChart.transform);
            obj.GetComponent<ClearQuestBlokScript>().SetText(dangoDatas[i], dangoClearTime[i]);
        }
    }

    private void Change(Vector2 axis)
    {

        if (!ChangeChoiceUtil.Choice(axis, ref next, Next.Max, canMove, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

        for (int i = 0; i < retryOrSelect.Length; i++)
        {
            retryOrSelect[i].ImageData.SetColor(Color.white);
        }
            retryOrSelect[(int)next].ImageData.SetColor(Color.red);
    }

    private void canChange()
    {
        if (canAxis)
        {
            Change(axis);
            canAxis = false;
        }

        if (axis.x > 0&&temp)
        {
            canAxis = true;
            temp = false;
        }

        if (axis.x < 0 && temp)
        {
            canAxis = true;
            temp = false;
        }
        if(axis.x>=-0.3&&0.3>=axis.x)
            temp= true;
    }
}
