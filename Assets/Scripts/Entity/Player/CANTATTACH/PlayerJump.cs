using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerJump
    {
        static readonly float[] JUMP_POWER_TABLE = { 11f, 12.5f, 14.5f, 17f, 19f };

        Rigidbody _rb;
        bool _isGround;
        int _maxStabCount;
        bool _isJumping;
        bool _isStayJump;

        Action _onJump;
        Action _onJumpExit;

        SpitManager _spitManager;

        public PlayerJump(Rigidbody rigidbody, Action onJump, Action onJumpExit,SpitManager spitManager)
        {
            _rb = rigidbody;
            _onJump = onJump;
            _onJumpExit = onJumpExit;
            _spitManager = spitManager;
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
            if (_spitManager.IsHitStop) return;

            _isStayJump = true;
        }

        public async void Jump()
        {
            _isStayJump = false;

            if (!_isGround) return;
            if (_isJumping) return;
            if (PlayerData.Event) return;

            //SEの再生
            SoundManager.Instance.PlaySE(UnityEngine.Random.Range((int)SoundSource.VOISE_PRINCE_JUMP01, (int)SoundSource.VOISE_PRINCE_JUMP02 + 1));
            SoundManager.Instance.PlaySE(SoundSource.SE8_JUMP);
            SoundManager.Instance.PlaySE(SoundSource.SE19_JUMPCHARGE_START);
            SoundManager.Instance.PlaySE(SoundSource.SE20_JUMPCHARGE_LOOP);

            //ベクトルを打ち消しジャンプ
            _isJumping = true;
            _onJump?.Invoke();

            //D5ごとにジャンプ力を個別にする
            _rb.velocity = Vector3.zero.SetY(JUMP_POWER_TABLE[_maxStabCount - 3]);

            //自由落下まで待機
            await WaitVelocityYLessZero();

            SoundManager.Instance.StopSE(SoundSource.SE20_JUMPCHARGE_LOOP);

            _isJumping = false;
            _onJumpExit?.Invoke();
        }

        private async UniTask WaitVelocityYLessZero()
        {
            try
            {
                while (_rb.velocity.y > 0)
                {
                    await UniTask.Yield();
                }
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }

        public bool IsJumping => _isJumping;
        public bool IsStayJump => _isStayJump;
    }
}