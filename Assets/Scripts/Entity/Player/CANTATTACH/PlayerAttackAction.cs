using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TM.Entity.Player
{
    class PlayerAttackAction
    {
        public enum AttackPattern
        {
            NormalAttack,
            FallAttack
        }

        const int ATTACK_FRAME = 25;
        const float DEFAULT_HEIGHT = 60f;

        int _currentTime = 0;
        ImageUIData _coolDownImage;
        Animator _animator;
        SpitManager _spitManager;

        public PlayerAttackAction(ImageUIData coolDownImage, Animator animator, SpitManager spitManager)
        {
            _coolDownImage = coolDownImage;
            _animator = animator;
            _spitManager = spitManager;
        }

        public bool FixedUpdate(AttackPattern attack)
        {
            return attack == AttackPattern.NormalAttack ? IsWaitingOnGround() : IsWaitingFallAttackAnimation();
        }

        private bool IsWaitingFallAttackAnimation()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("AN1")) return true;
            if (!stateInfo.IsName("AN7B")) return false;

            _spitManager.IsSticking = false;
            return stateInfo.normalizedTime >= 0.7f;
        }

        private bool IsWaitingOnGround()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName("AN5")) return false;

            _coolDownImage.ImageData.SetHeight(DEFAULT_HEIGHT * _currentTime / ATTACK_FRAME);
            return stateInfo.normalizedTime >= 0.9f;
        }

        public void ResetTime()
        {
            _currentTime = 0;
        }
    }
}