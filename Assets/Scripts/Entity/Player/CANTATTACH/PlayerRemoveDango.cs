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

        //���V��
        const float FLOATING_POWER = 3f;

        public bool HasRemoveDango => _hasRemoveDango;

        public PlayerRemoveDango(List<DangoColor> dangos, DangoUIScript dangoUIScript, PlayerData playerData, Animator animator,PlayerKusiScript kusit)
        {
            _dangos = dangos;
            _dangoUIScript = dangoUIScript;
            _playerData = playerData;
            _animator = animator;
            _kusiScript = kusit;
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

        //�c�q�e(���O��)
        public void Remove()
        {
            //���ɉ����Ȃ���������s���Ȃ��B
            if (_dangos.Count == 0) return;

            //�󒆎��ɍs������
            if (!_playerData.IsGround)
            {
                _playerData.Rb.velocity = _playerData.Rb.velocity.SetY(FLOATING_POWER);
            }

            //[Debug]�������������킩����
            //���܂ł́Adangos[dangos.Count - 1]�Ƃ��Ȃ���΂Ȃ�܂���ł������A
            //C#8.0�ȍ~�ł͈ȉ��̂悤�ɏȗ��ł���悤�ł��B
            //���́A�����m��Ȃ��l���ǂނƂ킯��������Ȃ��B
            Logger.Log(_dangos[^1]);

            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE9_REMOVE_DANGO);

            //���������B
            _dangos.RemoveAt(_dangos.Count - 1);

            //UI�X�V
            _dangoUIScript.DangoUISet(_dangos);
            _dangoUIScript.RemoveDango(_dangos);

            _hasRemoveDango = false;
            
            //���̒c�q�ύX
            _kusiScript.SetDango(_dangos);
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