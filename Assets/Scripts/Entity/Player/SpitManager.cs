using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] PlayerData player = default!;
    [SerializeField] CapsuleCollider _capsuleCollider = default!;
    DangoUIScript DangoUISC;

    [SerializeField] FloorManager _floorManager;
    [SerializeField] PlayerKusiScript kusiScript;

    private void Awake()
    {
        DangoUISC = player.GetDangoUIScript();
        _capsuleCollider.enabled = false;
    }

    private bool _isSticking;
    private bool _isInWall;

    /// <summary>
    /// 突き刺しボタンが押されたときにtrueになる。
    /// </summary>
    public bool IsSticking
    {
        get => _isSticking;
        set
        {
            _capsuleCollider.enabled = value;
            if (player.IsGround) _capsuleCollider.radius = 0.1f;
            else _capsuleCollider.radius = 0.5f;

            _isSticking = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DangoData>() == null) return;
        
        //刺せる状態ではないなら実行しない
        if (!IsSticking) return;
        if (_isInWall) return;

        if (LayerMask.LayerToName(other.gameObject.layer) == "Map")
        {
            _isInWall = true;
            return;
        }

        if (player.GetDangos().Count >= player.GetMaxDango())
        {
            if (!player.PlayerFall.IsFallAction) return;

            //急降下中なら団子を弾く
            Logger.Log("ぽよーん");
            other.GetComponent<Rigidbody>().AddForce((other.transform.position - player.transform.position) * 50f, ForceMode.Impulse); ;
            return;
        }

        if (other.gameObject.TryGetComponent(out DangoData dango))
        {
            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE14_STAB_DANGO);

            //落下アクション中に行う処理
            OnFallAction();

            //団子を刺す
            player.AddDangos(dango.GetDangoColor());

            //フィールドにある団子を消す
            dango.ReleaseDangoPool();

            //UIの更新
            DangoUISC.DangoUISet(player.GetDangos());
            DangoUISC.AddDango(player.GetDangos());

            //串の団子変更
            kusiScript.SetDango(player.GetDangos());

            //刺せ無くする
            IsSticking = false;
        }
        else
        {
            SoundManager.Instance.PlaySE(SoundSource.SE13_ATTACK);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isInWall) return;

        if (LayerMask.LayerToName(other.gameObject.layer) == "Map")
        {
            _isInWall = false;
            return;
        }
    }

    private void OnFallAction()
    {
        if (!player.PlayerFall.IsFallAction) return;

        //落下アクション○回しろ系クエストの判定
        QuestManager.Instance.SucceedChecker.CheckQuestPlayActionSucceed(QuestManager.Instance, Dango.Quest.QuestPlayAction.PlayerAction.FallAttack);

        Logger.Log("落下アクション中に刺した！");
    }
}
