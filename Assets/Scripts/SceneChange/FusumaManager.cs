using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Easing.Management;
#if UNITY_EDITOR
using UnityEditor;
#endif

class FusumaManager : MonoBehaviour
{
    [SerializeField] GameObject[] _fusumas;
    const float DEFAULT_CLOSE_POSITION = 480f;
    const float DEFAULT_OPEN_POSITION = 1440f;
    const float MOVE_AMOUNT = DEFAULT_OPEN_POSITION - DEFAULT_CLOSE_POSITION;
    const TM.Easing.EaseType EASETYPE = TM.Easing.EaseType.OutBounce;

    Vector3 _fusuma0Pos;
    Vector3 _fusuma1Pos;

    public async void Open(float time)
    {
        SetDefaultPosition(true);
        await FusumaOpen(time);
        SetDefaultPosition(false);
    }

    public async void Close(float time)
    {
        SetDefaultPosition(false);
        await FusumaClose(time);
        SetDefaultPosition(true);
    }

    private async UniTask FusumaOpen(float time, float waitTime = 0)
    {
        await UniTask.Delay((int)(waitTime * 1000f));
        float currentTime = 0;

        while (currentTime <= time)
        {
            await UniTask.DelayFrame(1);
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 0, 0);

            _fusuma0Pos.Set(-DEFAULT_CLOSE_POSITION + (-MOVE_AMOUNT * d), 0, 0);
            _fusuma1Pos.Set(DEFAULT_CLOSE_POSITION + (MOVE_AMOUNT * d), 0, 0);

            SetPos();
        }
    }
    private async UniTask FusumaClose(float time, float waitTime = 0)
    {
        await UniTask.Delay((int)(waitTime * 1000f));
        float currentTime = 0;

        while (currentTime <= time)
        {
            await UniTask.DelayFrame(1);
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 0, 0);

            _fusuma0Pos.Set(-DEFAULT_OPEN_POSITION + (MOVE_AMOUNT * d), 0, 0);
            _fusuma1Pos.Set(DEFAULT_OPEN_POSITION - (MOVE_AMOUNT * d), 0, 0);

            SetPos();
        }
    }

    private void SetDefaultPosition(bool isClose)
    {
        if (isClose)
        {
            _fusuma0Pos.Set(-DEFAULT_CLOSE_POSITION, 0, 0);
            _fusuma1Pos.Set(DEFAULT_CLOSE_POSITION, 0, 0);

            SetPos();
        }
        else
        {
            _fusuma0Pos.Set(-DEFAULT_OPEN_POSITION, 0, 0);
            _fusuma1Pos.Set(DEFAULT_OPEN_POSITION, 0, 0);

            SetPos();
        }
    }

    private void SetPos()
    {
        _fusumas[0].transform.localPosition = _fusuma0Pos;
        _fusumas[1].transform.localPosition = _fusuma1Pos;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FusumaManager))]
public class ExampleScriptEditor : Editor
{
    private FusumaManager fusumaManager;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        fusumaManager = target as FusumaManager;
        base.OnInspectorGUI();

        if (GUILayout.Button("Open"))
        {
            fusumaManager.Open(1f);
        }
        if (GUILayout.Button("Close"))
        {
            fusumaManager.Close(1f);
        }
        if (GUILayout.Button("Reset"))
        {
            fusumaManager.SendMessage("SetDefaultPosition", true);
        }
    }

}
#endif