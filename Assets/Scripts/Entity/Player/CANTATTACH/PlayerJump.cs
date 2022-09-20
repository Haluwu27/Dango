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
        Action _onJump;
        Action _onJumpExit;
        Animator _animator;

        public PlayerJump(Rigidbody rigidbody, Action onJump, Action onJumpExit, Animator animator)
        {
            _rb = rigidbody;
            _onJump = onJump;
            _onJumpExit = onJumpExit;
            _animator = animator;
        }

        public void SetIsGround(bool isGround)
        {
            _isGround = isGround;
        }

        public void SetMaxStabCount(int count)
        {
            _maxStabCount = count;
        }

        public async void Jump()
        {
            if (!_isGround) return;
            if (_isJumping) return;

            //SEの再生
            SoundManager.Instance.PlaySE(UnityEngine.Random.Range((int)SoundSource.VOISE_PRINCE_JUMP01, (int)SoundSource.VOISE_PRINCE_JUMP02 + 1));
            SoundManager.Instance.PlaySE(SoundSource.SE8_JUMP);

            //アニメーションの再生
            _animator.SetBool("IsJumping", true);

            //ベクトルを打ち消しジャンプ
            _isJumping = true;
            _onJump?.Invoke();
            _rb.velocity = Vector3.zero;

            //指定秒数待機
            await UniTask.Delay(1000);

            _rb.velocity = new(0, JUMP_POWER * _maxStabCount, 0);
            //_rb.AddForce(Vector3.up * (JUMP_POWER + _maxStabCount), ForceMode.Impulse);

            await WaitVelocityYLessZero();

            _animator.SetBool("IsJumping", false);
            _isJumping = false;
            _onJumpExit?.Invoke();
        }

        private async UniTask WaitVelocityYLessZero()
        {
            while (_rb.velocity.y > 0)
            {
                await UniTask.Yield();
                Logger.Log("upupupup");
            }
        }

        public bool IsJumping => _isJumping;
    }
}