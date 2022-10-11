using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerJump
    {
        const float JUMP_POWER = 3.72f * 3.75f / 2.8f * (5f / 6f);

        Rigidbody _rb;
        bool _isGround;
        int _maxStabCount;
        bool _isJumping;
        bool _isStayJump;

        Action _onJump;
        Action _onJumpExit;
        Animator _animator;

        int an11TriggerHash;
        int jumpingTriggerHash;

        public PlayerJump(Rigidbody rigidbody, Action onJump, Action onJumpExit, Animator animator)
        {
            _rb = rigidbody;
            _onJump = onJump;
            _onJumpExit = onJumpExit;
            _animator = animator;
            an11TriggerHash = Animator.StringToHash("AN11Trigger");
            jumpingTriggerHash = Animator.StringToHash("JumpingTrigger");
        }

        public void SetIsGround(bool isGround)
        {
            _isGround = isGround;
        }

        public void SetMaxStabCount(int count)
        {
            _maxStabCount = count;
        }

        public void OnStayJumping()
        {
            _animator.SetTrigger(an11TriggerHash);
            _isStayJump = true;
        }

        public async void Jump()
        {
            _animator.ResetTrigger(an11TriggerHash);

            if (!_isGround) return;
            if (_isJumping) return;

            _isStayJump = false;

            //SEの再生
            SoundManager.Instance.PlaySE(UnityEngine.Random.Range((int)SoundSource.VOISE_PRINCE_JUMP01, (int)SoundSource.VOISE_PRINCE_JUMP02 + 1));
            SoundManager.Instance.PlaySE(SoundSource.SE8_JUMP);
            SoundManager.Instance.PlaySE(SoundSource.SE19_JUMPCHARGE_START);
            SoundManager.Instance.PlaySE(SoundSource.SE20_JUMPCHARGE_LOOP);

            //アニメーションの再生
            _animator.SetTrigger(jumpingTriggerHash);

            //ベクトルを打ち消しジャンプ
            _isJumping = true;
            _onJump?.Invoke();
            _rb.velocity = Vector3.zero.SetY(JUMP_POWER * _maxStabCount);
            //_rb.AddForce(Vector3.up * (JUMP_POWER + _maxStabCount), ForceMode.Impulse);

            //自由落下まで待機
            await WaitVelocityYLessZero();

            SoundManager.Instance.StopSE(SoundSource.SE20_JUMPCHARGE_LOOP);

            _isJumping = false;
            _onJumpExit?.Invoke();
        }

        private async UniTask WaitVelocityYLessZero()
        {
            while (_rb.velocity.y > 0)
            {
                await UniTask.Yield();
            }
        }

        public bool IsJumping => _isJumping;
        public bool IsStayJump => _isStayJump;
    }
}