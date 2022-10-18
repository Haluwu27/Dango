using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum SoundSource
{
    //BGM
    BGM1A_STAGE1,
    BGM1B_STAGE2,
    BGM1C_STAGE3,
    BGM1D_STAGE4,
    BGM2_GAMEOVER,
    BGM3_SUCCEED,
    BGM4_TITLE,
    BGM5_MENU,

    //SE
    SE1_FOOTSTEPS_HARD,//この項目はSEの先頭固定でお願いします。
    SE2_FOOTSTEPS_GRASS,
    SE3_LANDING_HARD,
    SE4_LANDING_GRASS,
    SE5_PLAYER_STAY_EATDANGO,
    SE6_CREATE_ROLE_CHARACTER_ANIMATION,
    SE7_CANT_STAB_DANGO,
    SE8_JUMP,
    SE9_REMOVE_DANGO,
    SE10_FALLACTION,
    SE11_FALLACTION_LANDING,
    SE12_QUEST_SUCCEED,
    SE13_ATTACK,
    SE14_STAB_DANGO,
    SE15_FUSUMA_CLOSE,
    SE15B_FUSUMA_OPEN,
    SE16_UI_SELECTION,
    SE17_UI_DECISION,
    SE18_INVALID_OPERATION,
    SE19_JUMPCHARGE_START,
    SE20_JUMPCHARGE_LOOP,

    //VOISE
    VOISE_PRINCE_ATTACK01,
    VOISE_PRINCE_ATTACK02,
    VOISE_PRINCE_JUMP01,
    VOISE_PRINCE_JUMP02,
    VOISE_PRINCE_FALL01,
    VOISE_PRINCE_FALL02,
    VOISE_PRINCE_STAYEAT01,
    VOISE_PRINCE_STAYEAT02,
    VOISE_PRINCE_CREATEROLE01,
    VOISE_PRINCE_CREATEROLE02,
    VOISE_PRINCE_NOROLE01,
    VOISE_PRINCE_NOROLE02,
}

#if UNITY_EDITOR
[CustomEditor(typeof(SoundManager))]
public class SoundManagerOnGUI : Editor
{
    private SoundManager soundManager;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        soundManager = target as SoundManager;

        EditorGUILayout.HelpBox("BGM、SEを追加する際は順番に注意してください。", MessageType.Info);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField(soundManager._debugSoundSource >= SoundSource.SE1_FOOTSTEPS_HARD ? "SE Clip : Element " + (int)(soundManager._debugSoundSource - SoundSource.SE1_FOOTSTEPS_HARD) : "BGM Clip : Element " + (int)soundManager._debugSoundSource);
        EditorGUILayout.Separator();

        base.OnInspectorGUI();

    }
}
#endif

public class SoundManager : MonoBehaviour
{
    [Tooltip("BGM、SEの追加場所を取得できます。")]
    public SoundSource _debugSoundSource;

    public static SoundManager Instance { get; private set; }

    private AudioSource[] SEs = new AudioSource[SENum];
    private const int SENum = 10;

    [SerializeField] private AudioSource _BGM;
    [SerializeField] private SESystem _SEPrefab;

    [SerializeField] private AudioClip[] BGMClip;
    [SerializeField] private AudioClip[] SEClip;

    [SerializeField] private AudioMixer _audioMixer;
    const float DEFAULT_BGM_VOLUME = 1f;

    private void Awake()
    {
        Instance = this;

        CreateSEs();
    }

    private void Start()
    {
        InitAudioMixerGroup();
    }

    private void InitAudioMixerGroup()
    {
        _audioMixer.SetFloat("MasterVolume", SoundSettingManager.ConvertVolume2dB(DataManager.configData.masterVolume / 10f));
        _audioMixer.SetFloat("BGMVolume", SoundSettingManager.ConvertVolume2dB(DataManager.configData.backGroundMusicVolume / 10f));
        _audioMixer.SetFloat("SEVolume", SoundSettingManager.ConvertVolume2dB(DataManager.configData.soundEffectVolume / 10f));
        _audioMixer.SetFloat("VoiceVolume", SoundSettingManager.ConvertVolume2dB(DataManager.configData.voiceVolume / 10f));
    }

    private void CreateSEs()
    {
        for (int i = 0; i < SENum; i++)
        {
            SEs[i] = Instantiate(_SEPrefab).AudioSource;
            SEs[i].name = ("SE" + (i + 1));
        }
    }

    private void ChangeBGM(SoundSource sound)
    {
        int temp = 0;
        foreach (var bgm in BGMClip)
        {
            if ((int)sound == temp)
            {
                _BGM.clip = bgm;
                temp = 0;
                break;
            }
            else temp++;
        }
    }

    /// <summary>
    /// BGMをかけます。
    /// </summary>
    /// <param name="sound">かけたいBGM</param>
    public void PlayBGM(SoundSource sound)
    {
        ChangeBGM(sound);
        _BGM.Play();
    }

    public void PlayBGM(SoundSource sound, float fadeTime, float volume = DEFAULT_BGM_VOLUME)
    {
        ChangeBGM(sound);
        _BGM.volume = 0;
        _BGM.Play();
        Fadein(fadeTime, volume).Forget();
    }

    public void StopBGM()
    {
        _BGM.Stop();
    }

    public async void StopBGM(float fadeTime)
    {
        await Fadeout(fadeTime);

        StopBGM();
        _BGM.volume = DEFAULT_BGM_VOLUME;
    }

    private async UniTask Fadein(float time, float volume)
    {
        if (time <= 0) return;

        float fadeTime = 0;
        float lastVolume = volume;

        while (_BGM.volume < lastVolume)
        {
            await UniTask.Yield();
            fadeTime += Time.deltaTime;

            _BGM.volume = Mathf.Min(lastVolume * (fadeTime / time), lastVolume) * DataManager.configData.masterVolume * DataManager.configData.backGroundMusicVolume / 100 / 100;
        }
    }
    private async UniTask Fadeout(float time)
    {
        if (time <= 0) return;

        float fadeTime = 0;
        float firstVolume = _BGM.volume;

        while (_BGM.volume > 0)
        {
            await UniTask.Yield();
            fadeTime += Time.deltaTime;

            _BGM.volume = Mathf.Max(firstVolume * (1 - (fadeTime / time)), 0);
        }
    }

    private void ChangeSE(AudioSource _as, SoundSource sound)
    {
        int temp = BGMClip.Length;
        foreach (var se in SEClip)
        {
            if ((int)sound == temp)
            {
                _as.clip = se;
                temp = BGMClip.Length;
                break;
            }
            else temp++;
        }

    }

    /// <summary>
    /// SEを再生します。10チャンネルすべて利用されていた場合流れません
    /// </summary>
    /// <param name="sound">再生したいSE</param>
    /// <param name="stopPrebSE">以前に再生している同じSEを停止させるか否か</param>    
    public void PlaySE(int sound, bool stopPrebSE = false)
    {
        if (stopPrebSE) StopSE(sound);

        foreach (var se in SEs)
        {
            if (se.isPlaying) continue;

            ChangeSE(se, (SoundSource)sound);
            se.Play();
            return;
        }
        //すべてのチャンネルが使用中ならここにくる
        Logger.Warn("全SEチャンネルが使用中で" + sound + "が再生できませんでした");
    }
    /// <summary>
    /// SEを再生します。10チャンネルすべて利用されていた場合流れません
    /// </summary>
    /// <param name="sound">再生したいSE</param>
    /// <param name="stopPrebSE">以前に再生している同じSEを停止させるか否か</param>    
    public void PlaySE(SoundSource sound, bool stopPrebSE = false)
    {
        if (stopPrebSE) StopSE(sound);

        foreach (var se in SEs)
        {
            if (se.isPlaying) continue;

            ChangeSE(se, sound);
            se.Play();
            return;
        }
        //すべてのチャンネルが使用中ならここにくる
        Logger.Warn("全SEチャンネルが使用中で" + sound + "が再生できませんでした");
    }

    /// <summary>
    /// すべてのSEを停止させます。
    /// </summary>
    public void StopAllSE()
    {
        foreach (AudioSource se in SEs)
        {
            if (!se.isPlaying) continue;

            se.Stop();
        }
    }
    /// <summary>
    /// 指定のSEをすべて停止させます
    /// </summary>
    /// <param name="sound"></param>
    public void StopSE(SoundSource sound)
    {
        foreach (AudioSource se in SEs)
        {
            if (!se.isPlaying) continue;
            if (se.clip == null) continue;
            if (SEClip[(int)sound - BGMClip.Length] != se.clip) continue;

            se.Stop();
        }
    }
    public void StopSE(int sound)
    {
        foreach (AudioSource se in SEs)
        {
            if (!se.isPlaying) continue;
            if (se.clip == null) continue;
            if (SEClip[sound - BGMClip.Length] != se.clip) continue;

            se.Stop();
        }
    }


    /// <summary>
    /// オーディオミキサーの値を変更します
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dB"></param>
    public void ChangeAudioMixerDB(string name, float dB)
    {
        _audioMixer.SetFloat(name, dB);
    }
}
