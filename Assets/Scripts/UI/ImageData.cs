using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace TM.UI
{
    public class ImageData
    {
        Image _image;

        //UniTaskのキャンセルに必要
        //キャンセルの際は、CancellationTokenSourceのCancelメソッドをコールすればよい
        /// 注意：このキャンセルは「例外扱い」です。例外名：OperationCanceledException 
        CancellationTokenSource _cts = new();

        public ImageData(Image image)
        {
            _image = image;
        }

        public Vector3 GetPosition() => _image.rectTransform.localPosition;
        public float GetWidth() => _image.rectTransform.sizeDelta.x;
        public float GetHeight() => _image.rectTransform.sizeDelta.y;

        public void SetPosition(Vector3 position)
        {
            _image.rectTransform.localPosition = position;
        }
        public void SetPositionX(float value)
        {
            _image.rectTransform.localPosition = new(value, _image.rectTransform.localPosition.y, _image.rectTransform.localPosition.z);
        }
        public void SetPositionY(float value)
        {
            _image.rectTransform.localPosition = new(_image.rectTransform.localPosition.x, value, _image.rectTransform.localPosition.z);

        }
        public void SetPositionZ(float value)
        {
            _image.rectTransform.localPosition = new(_image.rectTransform.localPosition.x, _image.rectTransform.localPosition.y, value);

        }
        public void SetSizeDelta(float Width, float Height)
        {
            _image.rectTransform.sizeDelta.Set(Mathf.Max(Width, _image.minWidth), Mathf.Max(Height, _image.minHeight));
        }
        public void SetWidth(float Width)
        {
            _image.rectTransform.sizeDelta.Set(Mathf.Max(Width, _image.minHeight), _image.rectTransform.sizeDelta.y);
        }
        public void SetHeight(float Height)
        {
            _image.rectTransform.sizeDelta.Set(_image.rectTransform.sizeDelta.x, Mathf.Max(Height, _image.minHeight));
        }
        public void SetSprite(Sprite sprite)
        {
            SetAlpha(1f);
            _image.sprite = sprite;
        }
        public void SetSprite()
        {
            _image.sprite = null;
        }
        public void SetColor(Color color)
        {
            _image.color = color;
        }
        public void SetColor(Color32 color)
        {
            _image.color = color;
        }
        public void SetAlpha(float alpha)
        {
            Color c = _image.color;
            c.a = Mathf.Clamp01(alpha);
            _image.color = c;
        }
        public void SetImageType(Image.Type type)
        {
            _image.type = type;
        }
        public void SetFillMethod(Image.FillMethod fillMethod)
        {
            if (_image.type != Image.Type.Filled) return;

            _image.fillMethod = fillMethod;
        }
        public void SetFillOrigin(Image.OriginHorizontal origin)
        {
            if (_image.fillMethod != Image.FillMethod.Horizontal) return;

            _image.fillOrigin = (int)origin;
        }
        public void SetFillOrigin(Image.OriginVertical origin)
        {
            if (_image.fillMethod != Image.FillMethod.Vertical) return;

            _image.fillOrigin = (int)origin;
        }
        public void SetFillOrigin(Image.Origin90 origin)
        {
            if (_image.fillMethod != Image.FillMethod.Radial90) return;

            _image.fillOrigin = (int)origin;
        }
        public void SetFillOrigin(Image.Origin180 origin)
        {
            if (_image.fillMethod != Image.FillMethod.Radial180) return;

            _image.fillOrigin = (int)origin;
        }
        public void SetFillOrigin(Image.Origin360 origin)
        {
            if (_image.fillMethod != Image.FillMethod.Radial360) return;

            _image.fillOrigin = (int)origin;
        }
        public void SetFillAmount(float value)
        {
            _image.fillAmount = Mathf.Clamp01(value);
        }
        public void SetPreserveAspect(bool enable)
        {
            _image.preserveAspect = enable;
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

            while (_image.color.a < 1f)
            {
                try
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
                }
                catch (OperationCanceledException)//現状この例外の変数は使わない予定なので棄却しています。使う際は棄却をやめてください
                {
                    //キャンセル時の例外処理があればここ(別にエラーではない)
                }

                alpha += Time.deltaTime / time;
                SetAlpha(alpha);
            }
        }
        public async UniTask Fadeout(float time, float waitTime = 0)
        {
            await UniTask.Delay((int)(waitTime * 1000f));

            if (time <= 0)
            {
                SetAlpha(0);
                return;
            }

            float alpha = 1;

            while (_image.color.a > 0)
            {
                try
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
                }
                catch (OperationCanceledException)//現状この例外の変数は使わない予定なので棄却しています。使う際は棄却をやめてください
                {
                    //キャンセル時の例外処理があればここ(別にエラーではない)
                }

                alpha -= Time.deltaTime / time;
                SetAlpha(alpha);
            }
        }

        public async UniTask WipeinHorizontal(float time, Image.OriginHorizontal origin = Image.OriginHorizontal.Left, float waitTime = 0)
        {
            await UniTask.Delay((int)(waitTime * 1000f));

            if (time <= 0)
            {
                SetFillAmount(1f);
                return;
            }

            _image.type = Image.Type.Filled;
            _image.fillMethod = Image.FillMethod.Horizontal;
            _image.fillOrigin = (int)origin;
            SetFillAmount(0);

            float fillAmount = 0;

            while (_image.fillAmount < 1f)
            {
                try
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
                }
                catch (OperationCanceledException)//現状この例外の変数は使わない予定なので棄却しています。使う際は棄却をやめてください
                {
                    //キャンセル時の例外処理があればここ(別にエラーではない)
                }

                fillAmount += Time.deltaTime / time;
                SetFillAmount(fillAmount);
            }
        }
        public async UniTask WipeoutHorizontal(float time, Image.OriginHorizontal origin = Image.OriginHorizontal.Left, float waitTime = 0)
        {
            await UniTask.Delay((int)(waitTime * 1000f));

            if (time <= 0)
            {
                SetFillAmount(0);
                return;
            }

            _image.type = Image.Type.Filled;
            _image.fillMethod = Image.FillMethod.Horizontal;
            _image.fillOrigin = (int)origin;
            SetFillAmount(1);

            float fillAmount = 1f;

            while (_image.fillAmount > 0)
            {
                try
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
                }
                catch (OperationCanceledException)//現状この例外の変数は使わない予定なので棄却しています。使う際は棄却をやめてください
                {
                    //キャンセル時の例外処理があればここ(別にエラーではない)
                }

                fillAmount -= Time.deltaTime / time;
                SetFillAmount(fillAmount);
            }
        }

        public async UniTask MoveX(float startPosX, float moveValue, float time, float waitTime = 0)
        {
            await UniTask.Delay((int)(waitTime * 1000f));

            if (time <= 0)
            {
                return;
            }

            SetPositionX(startPosX);
            float progress = 0;

            while (progress < 1f)
            {
                try
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
                }
                catch (OperationCanceledException)//現状この例外の変数は使わない予定なので棄却しています。使う際は棄却をやめてください
                {
                    //キャンセル時の例外処理があればここ(別にエラーではない)
                }

                progress += Time.deltaTime / time;

                SetPositionX(startPosX + (Mathf.Clamp01(progress) * moveValue));
            }
        }

        public void CancelUniTask()
        {
            _cts.Cancel();
        }
    }
}