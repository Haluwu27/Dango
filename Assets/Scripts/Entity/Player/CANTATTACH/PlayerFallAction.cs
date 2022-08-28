using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerFallAction
    {
        //—Ž‰ºƒAƒNƒVƒ‡ƒ“’è”
        const int FALLACTION_STAY_AIR_FRAME = 50;
        const int FALLACTION_FALL_POWER = 30;
        const int FALLACTION_MOVE_POWER = 10;

        private int _currentTime = FALLACTION_STAY_AIR_FRAME;

        private bool _isFallAction = false;
        public bool IsFallAction
        {
            get => _isFallAction;
            set
            {
                if (!value) _currentTime = FALLACTION_STAY_AIR_FRAME;

                _isFallAction = value;
            }
        }

        public bool FixedUpdate(Rigidbody rigidbody, SpitManager spitManager)
        {
            if (!IsFallAction) return false;

            if (--_currentTime > 0)
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x / FALLACTION_MOVE_POWER, 0, rigidbody.velocity.z / FALLACTION_MOVE_POWER);
                return false;
            }

            rigidbody.AddForce(Vector3.down * FALLACTION_FALL_POWER, ForceMode.Impulse);
            spitManager.isSticking = true;
            return true;
        }
    }
}