using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingManager : MonoBehaviour
{
    enum SoundChoices
    {
        None,

        Master,
        SE,
        Voice,
        BGM,

        Max,
    }

    //表示・非表示切り替え用に管理するもの
    [SerializeField] Canvas _canvas = default!;
    [SerializeField] Image[] _images;

    SoundChoices _choice = SoundChoices.None + 1;

    private void Start()
    {
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
    }

    public void OnChangeScene()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
    }

    /// <summary>
    /// Canvasの表示・非表示を設定する関数
    /// </summary>
    public void SetCanvasEnable(bool enable)
    {
        _canvas.enabled = enable;

        if (enable)
        {
            _images[(int)_choice - 1].color = new Color32(176, 176, 176, 255);
            _choice = SoundChoices.None + 1;
            _images[(int)_choice - 1].color = Color.red;
        }
    }

    private void OnNavigate()
    {
        if (!_canvas.enabled) return;

        Vector2 axis = InputSystemManager.Instance.NavigateAxis;

        ChangeVolume(axis.x);
        ChangeChoice(axis);
    }

    private void ChangeChoice(Vector2 axis)
    {
        if (axis != Vector2.up && axis != Vector2.down) return;

        if (axis == Vector2.up)
        {
            _choice--;
            if (_choice <= SoundChoices.None)
            {
                _choice = SoundChoices.None + 1;
                return;
            }
        }
        else if (axis == Vector2.down)
        {
            _choice++;
            if (_choice >= SoundChoices.Max)
            {
                _choice = SoundChoices.Max - 1;
                return;
            }
        }

        _images[(int)_choice - 1 + (int)axis.y].color = new Color32(176, 176, 176, 255);
        _images[(int)_choice - 1].color = Color.red;
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void ChangeVolume(float vecX)
    {
        if (!_canvas.enabled) return;
        if (vecX != 1 && vecX != -1) return;

        switch (_choice)
        {
            case SoundChoices.Master:
                ChangeAudioMixerDB(ref DataManager.configData.masterVolume, vecX);
                break;
            case SoundChoices.BGM:
                ChangeAudioMixerDB(ref DataManager.configData.backGroundMusicVolume, vecX);
                break;
            case SoundChoices.SE:
                ChangeAudioMixerDB(ref DataManager.configData.soundEffectVolume, vecX);
                break;
            case SoundChoices.Voice:
                ChangeAudioMixerDB(ref DataManager.configData.voiceVolume, vecX);
                break;
        }
    }

    //下記変換関数の参照サイト
    //https://www.hanachiru-blog.com/entry/2022/08/22/120000
    
    /// <summary>
    /// ボリュームからデシベルへの変換
    /// </summary>
    /// <param name="volume">変換させたいボリューム（0-1）</param>
    /// <returns>デシベル（-80 ~ 0）</returns>
    public static float ConvertVolume2dB(float volume) => Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);
    
    /// <summary>
    /// デシベルからボリュームへの変換
    /// </summary>
    /// <param name="db">変換させたいデシベル（-80 ~ 0）</param>
    /// <returns>ボリューム（0-1）</returns>
    public static float ConvertDB2Volume(float db) => Mathf.Clamp(Mathf.Pow(10, Mathf.Clamp(db, -80, 0) / 20f), 0, 1);

    private void ChangeAudioMixerDB(ref int volume, float axis)
    {
        volume = Mathf.Clamp(volume + (int)axis, 0, 10);
        SoundManager.Instance.ChangeAudioMixerDB(AudioGroupName(_choice), ConvertVolume2dB(volume / 10f));
        
        Logger.Log(volume);
    }

    private string AudioGroupName(SoundChoices sound)
    {
        return sound switch
        {
            SoundChoices.Master => "MasterVolume",
            SoundChoices.SE => "SEVolume",
            SoundChoices.Voice => "VoiceVolume",
            SoundChoices.BGM => "BGMVolume",
            _ => throw new System.NotImplementedException(),
        };
    }
}