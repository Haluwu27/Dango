using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Volume
{
    /// <summary>
    /// Volumeを設定します。0-1の範囲で入力してください。
    /// </summary>
    public float volume
    {
        get => volume;
        set => volume = Mathf.Clamp01(value);
    }

    /// <summary>
    /// Volumeを設定します。0-1の範囲で入力してください。
    /// </summary>
    public Volume(float v)
    {
        volume = v;
    }
};
public struct Pan
{
    /// <summary>
    /// 左右どちらから聞こえるか設定。-1～1の範囲で入力してください。
    /// </summary>
    public float pan
    {
        get => pan;
        set => pan = Mathf.Clamp(value, -1, 1);
    }

    /// <summary>
    /// 左右どちらから聞こえるか設定。-1～1の範囲で入力してください。
    /// </summary>
    public Pan(float v)
    {
        pan = v;
    }

};
public struct Pitch
{
    /// <summary>
    /// 音の速さを設定。-3～3の範囲で入力してください。
    /// </summary>
    public float pitch
    {
        get => pitch;
        set => pitch = Mathf.Clamp(value, -3, 3);
    }

    /// <summary>
    /// 音の速さを設定。-3～3の範囲で入力してください。
    /// </summary>
    public Pitch(float v)
    {
        pitch = v;
    }
};

public class SESystem : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    private bool _isPlaying;

    public delegate void StopSEEventHandler();
    public static event StopSEEventHandler OnStopSE;

    public AudioSource AudioSource => _audioSource;

    private void Update()
    {
        if (_audioSource.isPlaying)
        {
            _isPlaying = true;
        }
        else if (_isPlaying)
        {
            _isPlaying = false;
            StopCallBack();
        }
    }

    private void StopCallBack()
    {
        OnStopSE?.Invoke();
    }

    #region public void Set()
    public void Set(Volume v)
    {
        _audioSource.volume = v.volume;
    }
    public void Set(Pan p)
    {
        _audioSource.panStereo = p.pan;
    }
    public void Set(Pitch p)
    {
        _audioSource.panStereo = p.pitch;
    }
    public void Set(Volume v, Pan p)
    {
        _audioSource.volume = v.volume;
        _audioSource.panStereo = p.pan;
    }
    public void Set(Volume v, Pitch p)
    {
        _audioSource.volume = v.volume;
        _audioSource.pitch = p.pitch;
    }
    public void Set(Pan pa, Pitch pi)
    {
        _audioSource.panStereo = pa.pan;
        _audioSource.pitch = pi.pitch;
    }
    public void Set(Volume v, Pan pa, Pitch pi)
    {
        _audioSource.volume = v.volume;
        _audioSource.panStereo = pa.pan;
        _audioSource.pitch = pi.pitch;
    }

    #endregion

}