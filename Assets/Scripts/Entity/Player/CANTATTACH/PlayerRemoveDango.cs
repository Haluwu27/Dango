using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerRemoveDango
    {
        //�X�e�[�g�J�ڗp
        bool _hasRemoveDango = false;

        List<DangoColor> _dangos;
        DangoUIScript _dangoUIScript;
        PlayerData _playerData;
        Animator _animator;
        PlayerKusiScript _kusiScript;
        SpitManager _spitManager;
        PlayerUIManager _playerUIManager;

        //���V��
        const float FLOATING_POWER = 3f;

        public bool HasRemoveDango => _hasRemoveDango;

        public PlayerRemoveDango(List<DangoColor> dangos, DangoUIScript dangoUIScript, PlayerData playerData, Animator animator,PlayerKusiScript kusit,SpitManager spitManager,PlayerUIManager playerUIManager)
        {
            _dangos = dangos;
            _dangoUIScript = dangoUIScript;
            _playerData = playerData;
            _animator = animator;
            _kusiScript = kusit;
            _spitManager = spitManager;
            _playerUIManager = playerUIManager;
        }

        public void OnPerformed()
        {
            if (_dangos.Count == 0) return;
            if (_spitManager.IsHitStop) return;

            _hasRemoveDango = true;
        }

        public void OnCanceled()
        {
            _hasRemoveDango = false;
        }

        //�c�q�e(���O��)
        public void Remove()
        {
            if (_dangos.Count == 0) return;

            //空中はふわっと浮かせる
            if (!_playerData.IsGround)
            {
                _playerData.Rb.velocity = _playerData.Rb.velocity.SetY(FLOATING_POWER);
            }

            //[Debug]外した団子がわかるやつ
            //Logger.Log(_dangos[^1]);

            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE9_REMOVE_DANGO);

            //先端を外す
            _dangos.RemoveAt(_dangos.Count - 1);

            //UIのセット
            _dangoUIScript.DangoUISet(_dangos);
            _dangoUIScript.RemoveDango(_dangos);
            _playerUIManager.SetReach();

            _hasRemoveDango = false;
            
            //串のUIをセット
            _kusiScript.SetDango(_dangos);

            //MAX警告
            _playerUIManager.MAXDangoSet(false);
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