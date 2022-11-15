using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerEat
    {
        RoleDirectingScript _roleDirecting;
        PlayerUIManager _playerUIManager;
        PlayerKusiScript _playerKusiScript;

        public PlayerEat(RoleDirectingScript roleDirecting, PlayerUIManager playerUIManager,PlayerKusiScript kusiScript)
        {
            _roleDirecting = roleDirecting;
            _playerUIManager = playerUIManager;
            _playerKusiScript = kusiScript;
        }

        public void EatDango(PlayerData parent)
        {
            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE6_CREATE_ROLE_CHARACTER_ANIMATION);

            //食べた団子の点数を取得
            float score = DangoRole.instance.CheckRole(parent.GetDangos(), parent.GetCurrentStabCount());

            //演出関数の呼び出し
            _roleDirecting.Dirrecting(parent.GetDangos());

            //満腹度を上昇
            parent.AddSatiety(score);

            //串をクリア。
            parent.ResetDangos();

            //UI更新
            parent.GetDangoUIScript().DangoUISet(parent.GetDangos());
            parent.GetDangoUIScript().RemoveDango(parent.GetDangos());

            //串の団子変更
            _playerKusiScript.SetDango(parent.GetDangos());

            //一部UIの非表示
            _playerUIManager.EatDangoUI_False();
        }

        public bool WaitAnimationComplete(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
    }
}