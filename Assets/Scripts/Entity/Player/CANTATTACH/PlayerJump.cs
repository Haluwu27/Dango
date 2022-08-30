using System;
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

            GameManager.SoundManager.PlaySE(UnityEngine.Random.Range((int)SoundSource.VOISE_PRINCE_JUMP01, (int)SoundSource.VOISE_PRINCE_JUMP02+1));
            rb.AddForce(Vector3.up * (JUMP_POWER + maxStabCount), ForceMode.Impulse);
        }
    }
}