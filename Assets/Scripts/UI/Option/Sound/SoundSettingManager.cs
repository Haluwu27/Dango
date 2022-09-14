using System.Collections;
using System.Collections.Generic;
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

    static readonly int COUNT = 10;

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

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
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
                ChangeMasterVolume(vecX);
                break;
            case SoundChoices.BGM:
                ChangeBGMVolume(vecX);
                break;
            case SoundChoices.SE:
                ChangeSEVolume(vecX);
                break;
            case SoundChoices.Voice:
                ChangeVoiceVolume(vecX);
                break;
        }
    }

    private void ChangeMasterVolume(float volume)
    {
        DataManager.configData.masterVolume += (int)volume * COUNT;
        SoundManager.Instance.BGM.outputAudioMixerGroup.audioMixer.SetFloat("MasterVolume", DataManager.configData.masterVolume = Mathf.Clamp(DataManager.configData.masterVolume, -80, 20));
        Logger.Log(DataManager.configData.masterVolume);
    }

    private void ChangeBGMVolume(float volume)
    {
        DataManager.configData.backGroundMusicVolume += (int)volume * COUNT;
        SoundManager.Instance.BGM.outputAudioMixerGroup.audioMixer.SetFloat("BGMVolume", DataManager.configData.backGroundMusicVolume = Mathf.Clamp(DataManager.configData.backGroundMusicVolume, -80, 20));

        Logger.Log(DataManager.configData.backGroundMusicVolume);
    }

    private void ChangeSEVolume(float volume)
    {
        DataManager.configData.soundEffectVolume += (int)volume * COUNT;
        SoundManager.Instance.BGM.outputAudioMixerGroup.audioMixer.SetFloat("SEVolume", DataManager.configData.soundEffectVolume = Mathf.Clamp(DataManager.configData.soundEffectVolume, -80, 20));

        Logger.Log(DataManager.configData.soundEffectVolume);
    }

    private void ChangeVoiceVolume(float volume)
    {
        DataManager.configData.voiceVolume += (int)volume * COUNT;
        SoundManager.Instance.BGM.outputAudioMixerGroup.audioMixer.SetFloat("VoiceVolume", DataManager.configData.voiceVolume = Mathf.Clamp(DataManager.configData.voiceVolume, -80, 20));

        Logger.Log(DataManager.configData.voiceVolume);
    }

}
