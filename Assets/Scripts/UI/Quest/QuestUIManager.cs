using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dango.Quest.UI
{
    class QuestUIManager : MonoBehaviour
    {
        [SerializeField] Canvas _canvas;

        [SerializeField] ImageUIData _icon;
        [SerializeField] Sprite[] sprites;

        public void ChangeIcon(QuestData[] questDatas)
        {
            //TODO:クエストの種類に応じて画像を差し替え
            //if (questData is QuestEatDango questEa)
            _icon.ImageData.SetSprite(sprites[0]);
        }
    }
}
