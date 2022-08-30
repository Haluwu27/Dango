using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum SoundSource
{
    //BGM
    BGM_Stage,
    

    //SE
    SE_PLAYER_STAY_EATDANGO,//この項目はSEの先頭固定でお願いします。
    SE_PLAYER_EATDANGO,
    SE_STAB_DANGO,
    SE_REMOVE_DANGO,
    SE_CREATE_ROLE,
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

        EditorGUILayout.LabelField(soundManager._debugSoundSource >= SoundSource.SE_PLAYER_STAY_EATDANGO ? "SE Clip : Element " + (int)(soundManager._debugSoundSource - SoundSource.SE_PLAYER_STAY_EATDANGO) : "BGM Clip : Element " + (int)soundManager._debugSoundSource);
        EditorGUILayout.Separator();

        base.OnInspectorGUI();

    }
}
#endif

public class SoundManager : MonoBehaviour
{
    [Tooltip("BGM、SEの追加場所を取得できます。")]
    public SoundSource _debugSoundSource;

    private AudioSource[] SEs = new AudioSource[SENum];
    private const int SENum = 10;

    [SerializeField] private AudioSource _BGM;
    [SerializeField] private GameObject _SEPrefab;

    [SerializeField] private AudioClip[] BGMClip;
    [SerializeField] private AudioClip[] SEClip;

    public AudioSource BGM => _BGM;

    private void Awake()
    {
        CreateSEs();
    }

    private void CreateSEs()
    {
        for (int i = 0; i < SENum; i++)
        {
            SEs[i] = Instantiate(_SEPrefab).GetComponent<AudioSource>();
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

    public void StopBGM()
    {
        _BGM.Stop();
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
    public void PlaySE(int sound)
    {
        foreach (var se in SEs)
        {
            if (se.isPlaying) continue;

            ChangeSE(se, (SoundSource)sound);
            se.volume = 0.125f;
            se.Play();
            return;
        }
        //すべてのチャンネルが使用中ならここにくる
        Logger.Warn("全SEチャンネルが使用中で" + sound + "が再生できませんでした");
    }
    public void PlaySE(SoundSource sound)
    {
        foreach (var se in SEs)
        {
            if (se.isPlaying) continue;

            ChangeSE(se, sound);
            se.volume = 0.125f;
            se.Play();
            return;
        }
        //すべてのチャンネルが使用中ならここにくる
        Logger.Warn("全SEチャンネルが使用中で" + sound + "が再生できませんでした");
    }
    public void PlaySE(int sound,float volume)
    {
        foreach (var se in SEs)
        {
            if (se.isPlaying) continue;

            ChangeSE(se, (SoundSource)sound);
            se.volume = volume;
            se.Play();
            return;
        }
        //すべてのチャンネルが使用中ならここにくる
        Logger.Warn("全SEチャンネルが使用中で" + sound + "が再生できませんでした");
    }
    public void PlaySE(SoundSource sound,float volume)
    {
        foreach (var se in SEs)
        {
            if (se.isPlaying) continue;

            ChangeSE(se, sound);
            se.volume = volume;
            se.Play();
            return;
        }
        //すべてのチャンネルが使用中ならここにくる
        Logger.Warn("全SEチャンネルが使用中で" + sound + "が再生できませんでした");
    }

    /// <summary>
    /// すべてのSEを停止させます。
    /// </summary>
    public void StopSE()
    {
        foreach (AudioSource se in SEs)
        {
            if (se.isPlaying == false) continue;

            se.Stop();
        }

    }
}
