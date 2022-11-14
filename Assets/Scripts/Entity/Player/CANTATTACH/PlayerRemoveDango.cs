using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerRemoveDango
    {
        //ステート遷移用
        bool _hasRemoveDango = false;

        List<DangoColor> _dangos;
        DangoUIScript _dangoUIScript;
        PlayerData _playerData;
        Animator _animator;

        //浮遊力
        const float FLOATING_POWER = 3f;

        public bool HasRemoveDango => _hasRemoveDango;

        public PlayerRemoveDango(List<DangoColor> dangos, DangoUIScript dangoUIScript, PlayerData playerData, Animator animator)
        {
            _dangos = dangos;
            _dangoUIScript = dangoUIScript;
            _playerData = playerData;
            _animator = animator;
        }

        public void OnPerformed()
        {
            if (_dangos.Count == 0) return;

            _hasRemoveDango = true;
        }

        public void OnCanceled()
        {
            _hasRemoveDango = false;
        }

        //団子弾(取り外し)
        public void Remove()
        {
            //串に何もなかったら実行しない。
            if (_dangos.Count == 0) return;

            //空中時に行う処理
            if (!_playerData.IsGround)
            {
                _playerData.Rb.velocity = _playerData.Rb.velocity.SetY(FLOATING_POWER);
            }

            //[Debug]何が消えたかわかるやつ
            //今までは、dangos[dangos.Count - 1]としなければなりませんでしたが、
            //C#8.0以降では以下のように省略できるようです。
            //問題は、これを知らない人が読むとわけが分からない。
            Logger.Log(_dangos[^1]);

            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE9_REMOVE_DANGO);

            //消す処理。
            _dangos.RemoveAt(_dangos.Count - 1);

            //UI更新
            _dangoUIScript.DangoUISet(_dangos);

            _hasRemoveDango = false;
        }

        public bool IsStayCoolTime()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("AN8A")) return true;

            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
        }

        public bool IsRemoveCoolTime()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("AN8B")) return true;

            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
        }
    }
}