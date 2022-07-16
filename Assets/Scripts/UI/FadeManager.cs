using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TM.Easing;
using TM.Easing.Management;

public class FadeManager : MonoBehaviour
{
    [SerializeField] Image image = default!;

    EaseType _easeType = EaseType.Linear;
    FadeStyle _fadeStyle = FadeStyle.Fadein;
    float _fadeTime = 0;
    float _fadeDuration = 0;

    public void StartFade(EaseType easeType,FadeStyle fadeStyle, float duration)
    {
        _easeType = easeType;
        _fadeStyle = fadeStyle;
        _fadeDuration = duration;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while (_fadeTime <= _fadeDuration)
        {
            Color c = image.color;

            c.a = _fadeStyle switch
            {
                FadeStyle.Fadein => EasingManager.EaseProgress(_easeType, _fadeTime, _fadeDuration, 0, 0),
                FadeStyle.Fadeout => 1 - EasingManager.EaseProgress(_easeType, _fadeTime, _fadeDuration, 0, 0),
                _ => throw new System.NotImplementedException(),
            };

            image.color = c;

            _fadeTime += Time.deltaTime;
            yield return null;
        }
    }
}