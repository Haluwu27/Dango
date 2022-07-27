using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using System.Runtime.CompilerServices;

public class TextData
{
    TextMeshProUGUI _text;
    bool _isFlash;
    float _flashCurrentTime = 0;

    public TextData(TextMeshProUGUI text)
    {
        _text = text;
    }

    //積極的にインライン化させるやつです。メソッド本体が小さいときに有効らしいです（詳しく時間の計測とかはしていないのでサイトを参照しています。）
    //https://ufcpp.net/study/csharp/structured/miscinlining/
    //わからなければおまじないだと思って無視してください。
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetColor(Color color)
    {
        _text.color = color;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetColor(Color32 color)
    {
        _text.color = color;
    }
    public void SetFontSize(float size)
    {
        _text.fontSize = size;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetPosition(Vector3 pos)
    {
        _text.transform.position = pos;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetRotation(Quaternion rot)
    {
        _text.transform.rotation = rot;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetRotation(Vector3 rot)
    {
        _text.transform.rotation = Quaternion.Euler(rot);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetText(string text)
    {
        _text.text = text;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetText()
    {
        _text.text = "";
    }

    public async void FlashAlpha(float finishTime, float flashTime, float coolTime)
    {
        _flashCurrentTime = 0;
        if (_isFlash) return;

        _isFlash = true;

        while (_flashCurrentTime < finishTime)
        {
            await Fadein(flashTime, coolTime);
            await Fadeout(flashTime, coolTime);
            _flashCurrentTime += flashTime * 2f;
        }

        SetText();
        await Fadein(0);
        _isFlash = false;
    }

    public async UniTask Fadein(float time, float waitTime = 0)
    {
        Color c = _text.color;
        float alpha = 0;

        await UniTask.Delay((int)(waitTime * 1000f));

        while (c.a < 1f)
        {
            await UniTask.DelayFrame(1);
            alpha += Time.deltaTime / time;
            Mathf.Clamp01(alpha);
            c.a = alpha;
            _text.color = c;
        }
    }

    public async UniTask Fadeout(float time, float waitTime = 0)
    {
        Color c = _text.color;
        float alpha = 1;

        await UniTask.Delay((int)(waitTime * 1000f));

        while (c.a > 0)
        {
            await UniTask.DelayFrame(1);
            alpha -= Time.deltaTime / time;
            Mathf.Clamp01(alpha);
            c.a = alpha;
            _text.color = c;
        }
    }
}
