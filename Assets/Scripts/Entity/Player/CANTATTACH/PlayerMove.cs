using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TM.Entity.Player
{
    class PlayerMove
    {
        //移動速度定数
        const float MOVESPEED = 75f;
        const float MAX_VELOCITY_MAG = 15f;
        const float RUN_SPEED_MAG = 25f;
        const float MIN_AXIS_VALUE = 0.1f;

        Animator _animator;

        public PlayerMove(Animator animator)
        {
            _animator = animator;
        }

        public void Update(Rigidbody rb, Transform camera, bool isRemoveCoolDown, bool isStayJump)
        {
            //落下中とその着地中に移動できないようにする
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("FallAction") || _animator.GetCurrentAnimatorStateInfo(0).IsName("FallActionLanding")) return;

            //入力値を代入
            Vector2 axis = InputSystemManager.Instance.MoveAxis;

            //Animation
            _animator.SetBool("IsDash", InputSystemManager.Instance.MoveAxis.magnitude > 0.5f && !isRemoveCoolDown);
            _animator.SetBool("IsWalking", InputSystemManager.Instance.MoveAxis.magnitude > 0);

            if (axis.magnitude < MIN_AXIS_VALUE) return;

            //Yを無視
            Vector3 cameraForward = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;

            //カメラの向きを元にベクトルの作成
            Vector3 moveVec = (axis.y * cameraForward + axis.x * camera.right) * MOVESPEED;

            if (rb.velocity.magnitude < MAX_VELOCITY_MAG)
            {
                float mag = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
                float speedMag = RUN_SPEED_MAG - mag;
                rb.AddForce((isRemoveCoolDown || isStayJump ? 0.4f : 1) * speedMag * moveVec);
            }

            RotateToMoveVec(moveVec, rb);
        }

        private void RotateToMoveVec(Vector3 moveVec, Rigidbody rb)
        {
            if (moveVec.magnitude == 0) return;
            Vector3 lookRot = new(moveVec.x, 0, moveVec.z);
            rb.transform.localRotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.LookRotation(lookRot), 5f);
        }
    }
}