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
        const float MAX_VELOCITY_MAG = 8f;
        const float RUN_SPEED_MAG = 7f;
        const float MIN_AXIS_VALUE = 0.1f;

        public void Update(Rigidbody rb, Transform camera)
        {
            //入力値を代入
            Vector2 axis = InputSystemManager.Instance.MoveAxis;

            //摩擦の計算をここで


            if (axis.magnitude < MIN_AXIS_VALUE) return;

            //カメラの向きを元にベクトルの作成
            Vector3 moveVec = (axis.y * camera.forward + axis.x * camera.right) * MOVESPEED;


            if (rb.velocity.magnitude < MAX_VELOCITY_MAG)
            {
                float mag = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
                float speedMag = RUN_SPEED_MAG - mag;
                rb.AddForce(moveVec * speedMag);
            }

            RotateToMoveVec(moveVec, rb);
        }

        private void RotateToMoveVec(Vector3 moveVec, Rigidbody rb)
        {
            if (moveVec.magnitude == 0) return;
            Vector3 lookRot = new(moveVec.x, 0, moveVec.z);
            rb.transform.localRotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.LookRotation(lookRot),5f);
        }
    }
}