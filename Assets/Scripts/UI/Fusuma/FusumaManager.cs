using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Easing.Management;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

class FusumaManager : MonoBehaviour
{
    [SerializeField] GameObject[] _fusumas;
    const float DEFAULT_CLOSE_POSITION = 360f;
    const float DEFAULT_OPEN_POSITION = 1440f;
    const float DEFAULT_SECOND_CLOSE_POSITION = 1080f;
    const float MOVE_AMOUNT = DEFAULT_OPEN_POSITION - DEFAULT_CLOSE_POSITION;
    const float MOVE_SECOND_AMOUNT = DEFAULT_OPEN_POSITION - DEFAULT_SECOND_CLOSE_POSITION;
    const TM.Easing.EaseType EASETYPE = TM.Easing.EaseType.InQuart;

    const float DEFAULT_OPEN_TIME = 1f;
    const float DEFAULT_CLOSE_TIME = 1f;

    Vector3 _fusuma0Pos;
    Vector3 _fusuma1Pos;
    Vector3 _fusuma2Pos;
    Vector3 _fusuma3Pos;

    public async void Open(float time = DEFAULT_OPEN_TIME)
    {
        SetDefaultPosition(true);
        await FusumaOpen(time);
        SetDefaultPosition(false);
    }
    public async UniTask UniTaskOpen(float time = DEFAULT_OPEN_TIME)
    {
        SetDefaultPosition(true);
        await FusumaOpen(time);
        SetDefaultPosition(false);
    }

    public async void Close(float time = DEFAULT_CLOSE_TIME)
    {
        SetDefaultPosition(false);
        await FusumaClose(time);
        SetDefaultPosition(true);
    }
    public async UniTask UniTaskClose(float time = DEFAULT_CLOSE_TIME)
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
            await UniTask.Yield();
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 0, 0);

            _fusuma0Pos.Set(-DEFAULT_CLOSE_POSITION + (-MOVE_AMOUNT * d), 0, 0);
            _fusuma1Pos.Set(DEFAULT_CLOSE_POSITION + (MOVE_AMOUNT * d), 0, 0);
            _fusuma2Pos.Set(-DEFAULT_SECOND_CLOSE_POSITION + (-MOVE_SECOND_AMOUNT * d), 0, 0);
            _fusuma3Pos.Set(DEFAULT_SECOND_CLOSE_POSITION + (MOVE_SECOND_AMOUNT * d), 0, 0);

            SetPos();
        }
    }
    private async UniTask FusumaClose(float time, float waitTime = 0)
    {
        await UniTask.Delay((int)(waitTime * 1000f));
        float currentTime = 0;

        while (currentTime <= time)
        {
            await UniTask.Yield();
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 0, 0);

            _fusuma0Pos.Set(-DEFAULT_OPEN_POSITION + (MOVE_AMOUNT * d), 0, 0);
            _fusuma1Pos.Set(DEFAULT_OPEN_POSITION - (MOVE_AMOUNT * d), 0, 0);
            _fusuma2Pos.Set(-DEFAULT_OPEN_POSITION + (MOVE_SECOND_AMOUNT * d), 0, 0);
            _fusuma3Pos.Set(DEFAULT_OPEN_POSITION - (MOVE_SECOND_AMOUNT * d), 0, 0);

            SetPos();
        }
    }

    private void SetDefaultPosition(bool isClose)
    {
        if (isClose)
        {
            _fusuma0Pos.Set(-DEFAULT_CLOSE_POSITION, 0, 0);
            _fusuma1Pos.Set(DEFAULT_CLOSE_POSITION, 0, 0);
            _fusuma2Pos.Set(-DEFAULT_SECOND_CLOSE_POSITION, 0, 0);
            _fusuma3Pos.Set(DEFAULT_SECOND_CLOSE_POSITION, 0, 0);

            SetPos();
        }
        else
        {
            _fusuma0Pos.Set(-DEFAULT_OPEN_POSITION, 0, 0);
            _fusuma1Pos.Set(DEFAULT_OPEN_POSITION, 0, 0);
            _fusuma2Pos.Set(-DEFAULT_OPEN_POSITION, 0, 0);
            _fusuma3Pos.Set(DEFAULT_OPEN_POSITION, 0, 0);

            SetPos();
        }
    }

    private void SetPos()
    {
        try
        {
            if (_fusumas == null) return;

            _fusumas[0].transform.localPosition = _fusuma0Pos;
            _fusumas[1].transform.localPosition = _fusuma1Pos;
            _fusumas[2].transform.localPosition = _fusuma2Pos;
            _fusumas[3].transform.localPosition = _fusuma3Pos;
        }
        catch(MissingReferenceException)
        {
            //UniTask実行中にゲームを終了したり、別画面に移行するとこのエラーを出す。
            //進行上の問題はないが、エラーのため対処。
            return;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FusumaManager))]
public class FusumaManagerEditor : Editor
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