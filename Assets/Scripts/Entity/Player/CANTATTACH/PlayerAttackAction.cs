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

        public PlayerAttackAction(ImageUIData coolDownImage, Animator animator)
        {
            _coolDownImage = coolDownImage;
            _animator = animator;
        }

        public bool FixedUpdate(AttackPattern attack)
        {
            return attack == AttackPattern.NormalAttack ? IsWaitingOnGround() : IsWaitingFallAttackAnimation();
        }

        private bool IsWaitingFallAttackAnimation()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName("AN7B")) return false;
            return stateInfo.normalizedTime >= 0.7f;
        }

        private bool IsWaitingOnGround()
        {
            //“Ë‚«h‚µ’†i‚à‚Æ‚É–ß‚é‘Ò‹@’†j‚È‚ç
            if (++_currentTime < ATTACK_FRAME)
            {
                _coolDownImage.ImageData.SetHeight(DEFAULT_HEIGHT * _currentTime / ATTACK_FRAME);
                return false;
            }

            _coolDownImage.ImageData.SetHeight(DEFAULT_HEIGHT);
            return true;
        }

        public void ResetTime()
        {
            _currentTime = 0;
        }
    }
}