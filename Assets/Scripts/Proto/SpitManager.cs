using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] Player1 player;
    [SerializeField] private DangoUIScript DangoUISC;

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
            CheckIsFallAction();
            player.AddDangos(dango.GetDangoColor());
            DangoUISC.DangoUISet(player.GetDangos());
            
            isSticking = false;

            other.gameObject.SetActive(false);
        }
    }

    private void CheckIsFallAction()
    {
        if (!player.IsFallAction) return;

        Logger.Log("落下アクション中に刺した！");
    }
}
