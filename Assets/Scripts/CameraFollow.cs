using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("追従したいターゲット")]
    public Transform target = default!;

    [SerializeField, Range(0.01f, 1f),Tooltip("カメラの追従度")]
    private float smoothSpeed = 0.125f;

    [SerializeField,Tooltip("ターゲットからのカメラの位置")]
    private Vector3 offset = new Vector3(0f, 1f, -10f);

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
