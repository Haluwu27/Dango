using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField] FadeManager _fadeManager;
    [SerializeField] FusumaManager _fusumaManager;

    private void Start()
    {
        InputSystemManager.Instance.onAnyKeyPerformed += GameStart;
        SoundManager.Instance.PlayBGM(SoundSource.BGM4_TITLE);
        _fadeManager.StartFade(TM.Easing.EaseType.Linear, FadeStyle.Fadeout, 1f);
    }

    public async void GameStart()
    {
        //var scenes = DataManager.HasSaveDataFile() ? SceneSystem.Scenes.Menu : SceneSystem.Scenes.Tutorial;
        var scenes = SceneSystem.Scenes.Menu;

        InputSystemManager.Instance.onAnyKeyPerformed -= GameStart;

        await _fusumaManager.UniTaskClose();
        SoundManager.Instance.StopBGM();
        
        SceneSystem.Instance.Load(scenes);
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Title,true);
    }
}
