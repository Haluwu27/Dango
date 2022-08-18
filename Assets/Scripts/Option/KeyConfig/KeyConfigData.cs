using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TM.Input.KeyConfig
{
    [RequireComponent(typeof(KeyData))]
    public class KeyConfigData : MonoBehaviour
    {
        [System.Flags]
        public enum GameAction
        {
            Move = 1 << 0,
            LookRotation = 1 << 1,
            Jump = 1 << 2,
            Attack = 1 << 3,
            Eat = 1 << 4,
            Remove = 1 << 5,
            Pause = 1 << 6,
            UIExpansion = 1 << 7,

            [InspectorName("")]
            Max = 8,
        }

        [SerializeField] GameAction configSelection = GameAction.Move;

        //コントロールした際の移動先の指定
        [SerializeField] KeyConfigData up;
        [SerializeField] KeyConfigData upperRight;
        [SerializeField] KeyConfigData right;
        [SerializeField] KeyConfigData lowerRight;
        [SerializeField] KeyConfigData down;
        [SerializeField] KeyConfigData lowerLeft;
        [SerializeField] KeyConfigData left;
        [SerializeField] KeyConfigData upperLeft;
        KeyConfigData[] keyConfigDatas = new KeyConfigData[8];

        public KeyConfigData[] GetKeyConfigDatas() => keyConfigDatas;

        public GameAction ConfigSelection => configSelection;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            keyConfigDatas[0] = up;
            keyConfigDatas[1] = upperRight;
            keyConfigDatas[2] = right;
            keyConfigDatas[3] = lowerRight;
            keyConfigDatas[4] = down;
            keyConfigDatas[5] = lowerLeft;
            keyConfigDatas[6] = left;
            keyConfigDatas[7] = upperLeft;
        }

        public void SelectAnimation()
        {
            //ここに選択したときのアニメーションでも・・・・
        }
    }
}