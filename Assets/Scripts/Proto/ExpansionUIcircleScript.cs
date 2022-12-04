using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dango.Quest.UI {
    public class ExpansionUIcircleScript : MonoBehaviour
    {
        public ExpansionCanvasScript expansion;
        private GameObject obj;
        float time = 0;

        private void Awake()
        {
            obj = GameObject.Find("ExpansionCanvas");
            if (obj != null)
            expansion = obj.GetComponent<ExpansionCanvasScript>();
        }
        private void Update()
        {
            time += Time.deltaTime;
            if(expansion!=null)
            if (expansion.set == true)
            {
                if (time > 3f)
                {
                    expansion.set = false;
                    expansion.OffSet();
                    expansion.PlayerUI_Set();
                    Destroy(gameObject);
                }
            }
        }
        private void OnTriggerStay(Collider col)//プレイヤーに当たったら拡張UI表示
        {
            if (expansion != null)
                if (col.gameObject.CompareTag("Player"))
            {
                expansion.Onset();
                expansion.set = true;
                expansion.PlayerUI_Set();
            }
        }
        private void OnTriggerExit(Collider col)//離れたら非表示
        {
                if (col.gameObject.CompareTag("Player"))
            {
                if (expansion != null)
                {
                    expansion.set = false;
                    expansion.OffSet();
                    expansion.PlayerUI_Set();
                }
                Destroy(gameObject);
            }
        }
    } 
}
