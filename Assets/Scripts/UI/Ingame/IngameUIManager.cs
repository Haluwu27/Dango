using Cysharp.Threading.Tasks;
using Dango.Quest.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngameUIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup[] _canvasGroups;
    [SerializeField] TextUIData[] _textUIDatas;

    private void Start()
    {
        foreach (var text in _textUIDatas)
        {
            text.TextData.SetAlpha(0);
        }
    }

    public async UniTask EraseUIs()
    {
        float alpha = 1;

        while (alpha > 0)
        {
            await UniTask.Yield();
            alpha -= Time.deltaTime;
            alpha = Mathf.Max(alpha, 0);

            foreach (var canvasGroup in _canvasGroups)
            {
                canvasGroup.alpha = alpha;
            }
        }
    }

    public async UniTask TextAnimation()
    {
        float time = 0;

        while (time < 2.4f)
        {
            await UniTask.Yield();
            time += Time.deltaTime;

            int index = (int)(Mathf.Min(time, 2.3999f) / 0.6f);
            _textUIDatas[index].TextData.SetFontSize(200 - 50 * (time % 0.6f));
            _textUIDatas[index].TextData.Fadein(0.1f).Forget();
        }
    }
}
