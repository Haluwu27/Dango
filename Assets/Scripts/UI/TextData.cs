﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

namespace TM.UI.Text
{
    public class TextData
    {
        TextMeshProUGUI _text;
        bool _isFlash;
        float _flashCurrentTime = 0;

        public TextData(TextMeshProUGUI text)
        {
            _text = text;
        }

        public void SetColor(Color color)
        {
            _text.color = color;
        }
        public void SetColor(Color32 color)
        {
            _text.color = color;
        }
        public void SetFontSize(float size)
        {
            _text.fontSize = size;
        }
        public void SetPosition(Vector3 pos)
        {
            _text.transform.position = pos;
        }
        public void SetRotation(Quaternion rot)
        {
            _text.transform.rotation = rot;
        }
        public void SetRotation(Vector3 rot)
        {
            _text.transform.rotation = Quaternion.Euler(rot);
        }
        public void SetText(string text)
        {
            SetAlpha(1f);
            _text.text = text;
        }
        public void SetText()
        {
            SetAlpha(1f);
            _text.text = "";
        }
        public void SetAlpha(float alpha)
        {
            Mathf.Clamp01(alpha);

            Color c = _text.color;
            c.a = alpha;
            _text.color = c;
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

            _isFlash = false;
        }

        public async UniTask Fadein(float time, float waitTime = 0)
        {
            await UniTask.Delay((int)(waitTime * 1000f));

            if (time <= 0)
            {
                SetAlpha(1f);
                return;
            }

            float alpha = 0;

            while (_text.color.a < 1f)
            {
                await UniTask.DelayFrame(1);
                alpha += Time.deltaTime / time;
                SetAlpha(alpha);
            }
        }

        public async UniTask Fadeout(float time, float waitTime = 0)
        {
            await UniTask.Delay((int)(waitTime * 1000f));

            float alpha = 1f;

            if (time <= 0)
            {
                SetAlpha(0);
                return;
            }

            while (_text.color.a > 0)
            {
                await UniTask.DelayFrame(1);
                alpha -= Time.deltaTime / time;
                SetAlpha(alpha);
            }
        }
    }
}