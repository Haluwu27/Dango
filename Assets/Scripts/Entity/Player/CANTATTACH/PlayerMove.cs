using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TM.Entity.Player
{
    class PlayerMove
    {
        //移動速度定数
        const float MOVESPEED = 10f;
        const float MAX_VELOCITY_MAG = 11f;
        const float RUN_SPEED_MAG = 10f;
        const float MIN_AXIS_VALUE = 0.1f;

        AnimationManager _manager;
        bool _isMoveable = true;

        public void SetIsMoveable(bool enable) => _isMoveable = enable;

        public PlayerMove(AnimationManager manager)
        {
            _manager = manager;
        }

        public void Update(Rigidbody rb, Transform camera, bool isWalkState)
        {
            if (!_isMoveable) return;
            if (PlayerData.Event) return;

            //入力値を代入
            Vector2 axis = InputSystemManager.Instance.MoveAxis;

            if (axis.magnitude < MIN_AXIS_VALUE) return;

            //Yを無視
            Vector3 cameraForward = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;

            //カメラの向きを元にベクトルの作成
            Vector3 moveVec = (axis.y * cameraForward + axis.x * camera.right) * MOVESPEED;

            if (rb.velocity.magnitude < MAX_VELOCITY_MAG)
            {
                float mag = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
                float speedMag = RUN_SPEED_MAG - mag;
                rb.AddForce((isWalkState ? 0.4f : 1) * speedMag * moveVec, ForceMode.Acceleration);
            }

            RotateToMoveVec(moveVec, rb);
        }

        public void Animation()
        {
            float mag = InputSystemManager.Instance.MoveAxis.magnitude;

            if (mag > 0.5f) _manager.ChangeAnimation(AnimationManager.E_Animation.An2_Run, 0.2f);
            else if (mag > 0) _manager.ChangeAnimation(AnimationManager.E_Animation.An10_Walk, 0.2f);
            else _manager.ChangeAnimation(AnimationManager.E_Animation.An1_Idle, 0.2f);
        }

        private void RotateToMoveVec(Vector3 moveVec, Rigidbody rb)
        {
            if (moveVec.magnitude == 0) return;
            Vector3 lookRot = new(moveVec.x, 0, moveVec.z);
            rb.transform.localRotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.LookRotation(lookRot), 10f);
        }
    }
}