using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerJump
    {
        const float JUMP_POWER = 10f;

        public void Jump(Rigidbody rb, bool isGround, int maxStabCount)
        {
            if (!isGround) return;
            if (!InputSystemManager.Instance.IsPressJump) return;

            rb.AddForce(Vector3.up * (JUMP_POWER + maxStabCount), ForceMode.Impulse);
        }
    }
}