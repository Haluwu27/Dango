using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField] FadeManager _fadeManager;

    private void Start()
    {
        InputSystemManager.Instance.onAnyKeyPerformed += GameStart;
        _fadeManager.StartFade(TM.Easing.EaseType.Linear, FadeStyle.Fadeout, 1f);
    }

    public void GameStart()
    {
        var scenes = DataManager.HasSaveDataFile() ? SceneSystem.Scenes.Menu : SceneSystem.Scenes.Tutorial;

        InputSystemManager.Instance.onAnyKeyPerformed -= GameStart;
        SceneSystem.Instance.Load(scenes);
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Title);
    }
}
