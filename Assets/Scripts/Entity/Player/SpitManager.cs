using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] PlayerData player = default!;
    DangoUIScript DangoUISC;

    private void Awake()
    {
        if (DangoUISC == null)
        {
            DangoUISC = GameObject.Find("Canvas").transform.Find("DangoBackScreen").GetComponent<DangoUIScript>();
        }
    }

    /// <summary>
    /// 突き刺しボタンが押されたときにtrueになる。
    /// </summary>
    public bool isSticking = false;
    
    private void OnTriggerStay(Collider other)
    {
        //刺せる状態ではないなら実行しない
        if (!isSticking) return;
        if (player.GetDangos().Count >= player.GetMaxDango()) return;

        if (other.gameObject.TryGetComponent(out DangoManager dango))
        {
            //落下アクション中に行う処理
            OnFallAction();

            //団子を刺す
            player.AddDangos(dango.GetDangoColor());

            //フィールドにある団子を消す
            other.gameObject.SetActive(false);

            //UIの更新
            DangoUISC.DangoUISet(player.GetDangos());
            
            //刺せ無くする
            isSticking = false;
        }
    }

    private void OnFallAction()
    {
        if (!player.IsFallAction) return;

        Logger.Log("落下アクション中に刺した！");
    }
}
