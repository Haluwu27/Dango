using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionSystem : MonoBehaviour
{
    private const float _volumeDefaultScale = 2.0f;
    private float _volumeScale = default;

    private SoundManager _soundManager = default;

    private void Awake()
    {
        _volumeScale = _volumeDefaultScale;
    }

    private void OnEnable()
    {
        if (_soundManager == null) _soundManager = GameManager.SoundManager;

        //FixedUpdateの実行を完全に停止させる
        Time.timeScale = 0;

        if (_soundManager != null)
        {
            _soundManager.BGM.volume /= volumeScale;
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;

        if (_soundManager != null)
        {
            _soundManager.BGM.volume *= volumeScale;
        }
    }

    private void Update()
    {
        ExitOption();
    }

    private void ExitOption()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// オプションに入った際のBGMの音量倍率(初期値：2)
    /// </summary>
    public float volumeScale
    {
        get => _volumeScale;
        set
        {
            if (value <= 0)
            {
                Logger.Warn("ボリュームの変更倍率を0以下にはできません。初期値の" + _volumeDefaultScale + "に変更します。");
                _volumeScale = _volumeDefaultScale;
            }
            else
            {
                _volumeScale = value;
            }
        }
    }

}
