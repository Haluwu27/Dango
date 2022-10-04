using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dango.Quest.UI {
    public class ExpansionUIcircleScript : MonoBehaviour
    {
        public ExpansionCanvasScript expansion;

        private void Awake()
        {
            expansion = GameObject.Find("ExpansionCanvas").GetComponent<ExpansionCanvasScript>();
        }
        private void OnTriggerStay(Collider col)//プレイヤーに当たったら拡張UI表示
        {
            if (col.gameObject.CompareTag("Player"))
            {
                expansion.Onset();
                expansion.set = true;
            }
        }
        private void OnTriggerExit(Collider col)//離れたら非表示
        {
            if (col.gameObject.CompareTag("Player"))
            {
                expansion.set = false;
                expansion.OffSet();
                Destroy(gameObject);
            }
        }
    } 
}
