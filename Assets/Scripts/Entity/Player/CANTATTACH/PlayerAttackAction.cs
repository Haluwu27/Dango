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

        public PlayerAttackAction(ImageUIData coolDownImage)
        {
            _coolDownImage = coolDownImage;
        }

        public bool FixedUpdate()
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