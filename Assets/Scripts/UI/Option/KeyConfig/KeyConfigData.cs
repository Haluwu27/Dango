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
            Unset = 1 << 0,
            Move = 1 << 1,
            LookRotation = 1 << 2,
            Jump = 1 << 3,
            Attack = 1 << 4,
            Eat = 1 << 5,
            Remove = 1 << 6,
            Pause = 1 << 7,
            UIExpansion = 1 << 8,

            [InspectorName("")]
            Max = 9,
        }

        [SerializeField] GameAction configSelection = GameAction.Unset;

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

        [SerializeField] KeyData.GamepadKey key;

        KeyData _keyData;

        public KeyData KeyData => _keyData;

        public KeyConfigData[] GetKeyConfigDatas() => keyConfigDatas;

        public GameAction ConfigSelection => configSelection;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            _keyData = DataManager.GetKeyData(key);
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
    }
}