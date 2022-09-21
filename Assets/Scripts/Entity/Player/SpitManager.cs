using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] PlayerData player = default!;
    DangoUIScript DangoUISC;

    private void Start()
    {
        DangoUISC = player.GetDangoUIScript();
    }

    /// <summary>
    /// 突き刺しボタンが押されたときにtrueになる。
    /// </summary>
    public bool isSticking = false;

    private void OnTriggerEnter(Collider other)
    {
        //刺せる状態ではないなら実行しない
        if (!isSticking) return;
        if (player.GetDangos().Count >= player.GetMaxDango())
        {
            if (!player.PlayerFall.IsFallAction) return;

            //急降下中なら団子を弾く
            Logger.Log("ぽよーん");
            other.GetComponent<Rigidbody>().AddForce((other.transform.position - player.transform.position) * 50f, ForceMode.Impulse); ;
            return;
        }

        if (other.gameObject.TryGetComponent(out DangoManager dango))
        {
            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE14_STAB_DANGO);

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
        if (!player.PlayerFall.IsFallAction) return;

        Logger.Log("落下アクション中に刺した！");
    }
}
