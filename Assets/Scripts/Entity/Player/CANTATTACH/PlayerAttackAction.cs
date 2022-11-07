using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TM.Entity.Player
{
    class PlayerAttackAction
    {
        const int ATTACK_FRAME = 25;
        const float DEFAULT_HEIGHT = 60f;

        int _currentTime = 0;
        ImageUIData _coolDownImage;
        PlayerData _playerData;
        Animator _animator;

        public PlayerAttackAction(ImageUIData coolDownImage,PlayerData playerData,Animator animator)
        {
            _coolDownImage = coolDownImage;
            _playerData = playerData;
            _animator = animator;
        }

        public bool FixedUpdate()
        {
            return _playerData.IsGround ? IsWaitingOnGround() : IsWaitingFallAttackAnimation();
        }

        private bool IsWaitingFallAttackAnimation()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).IsName("FallActionLanding");
        }

        private bool IsWaitingOnGround()
        {
            Logger.Log("Ç§Ç≤Ç¢ÇƒÇ‹Ç∑");
            //ìÀÇ´éhÇµíÜÅiÇ‡Ç∆Ç…ñﬂÇÈë“ã@íÜÅjÇ»ÇÁ
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