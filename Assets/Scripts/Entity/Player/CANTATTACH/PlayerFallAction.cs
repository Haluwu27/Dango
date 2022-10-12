using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerFallAction
    {
        System.Random _rand = new();

        //落下アクション定数
        const int FALLACTION_STAY_AIR_FRAME = 50;
        const int FALLACTION_FALL_POWER = 30;
        const int FALLACTION_MOVE_POWER = 10;

        bool _isFallAction = false;

        int _currentTime = FALLACTION_STAY_AIR_FRAME;

        Action _onFall;
        Action _onFallExit;

        CapsuleCollider _collider;
        public PlayerFallAction(CapsuleCollider collider, Action onFall, Action onFallExit)
        {
            _collider = collider;
            _onFall = onFall;
            _onFallExit = onFallExit;
        }

        public bool ToFallAction(Vector3 playerPos, bool isGround)
        {
            if (isGround) return false;

            Ray ray = new(playerPos, Vector3.down);

            //近くに地面があるか(playerの半分の大きさ)判定
            return !Physics.Raycast(ray, _collider.height + _collider.height / 2f);
        }

        public bool FixedUpdate(Rigidbody rigidbody, SpitManager spitManager)
        {
            if (!IsFallAction) return false;

            if (--_currentTime > 0)
            {
                rigidbody.velocity = rigidbody.velocity.SetX(rigidbody.velocity.x / FALLACTION_MOVE_POWER).SetY(0).SetZ(rigidbody.velocity.z / FALLACTION_MOVE_POWER);
                return false;
            }

            //SE再生
            SoundManager.Instance.PlaySE(_rand.Next((int)SoundSource.VOISE_PRINCE_FALL01, (int)SoundSource.VOISE_PRINCE_FALL02 + 1));
            SoundManager.Instance.PlaySE(SoundSource.SE10_FALLACTION);
            
            //落下刺し処理(質量無視で力を加える)
            rigidbody.AddForce(Vector3.down * FALLACTION_FALL_POWER, ForceMode.VelocityChange);
            spitManager.IsSticking = true;
            
            return true;
        }

        public bool IsFallAction
        {
            get => _isFallAction;
            set
            {
                if (!value)
                {
                    _onFallExit?.Invoke();
                    _currentTime = FALLACTION_STAY_AIR_FRAME;
                }
                else
                {
                    _onFall?.Invoke();
                }

                _isFallAction = value;
            }
        }
    }
}