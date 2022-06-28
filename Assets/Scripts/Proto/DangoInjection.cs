using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangoInjection : MonoBehaviour
{
    [SerializeField] GameObject spawner = default!;

    [SerializeField] private int defalutInjectionCount = 1;
    [SerializeField] private int defalutContinueFrame = 0;
    [SerializeField] private int waitFrame = 50;

    [SerializeField] private float shotPower = 10f;

    [SerializeField, Tooltip("縦方向の可動域")] Vector2 verticalRot;
    [SerializeField, Tooltip("横方向の可動域")] Vector2 horizontalRot;

    private DangoPoolManager _poolManager = default!;

    private Vector3 _lookAngle = default;
    private Vector3 _nextLookAngle = default;
    private Vector3 _firstLookAngle = default;

    private int _injectionCount = default;
    private int _continueFrame = default;
    private int _currentWaitFrame = default;


    private void Awake()
    {
        Logger.Assert(verticalRot.x < verticalRot.y);
        Logger.Assert(horizontalRot.x < horizontalRot.y);

        //初期化
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
        _firstLookAngle = transform.rotation.eulerAngles;
        NextLook();
    }

    private void FixedUpdate()
    {
        //待機中はアニメーション、待機終了で発射
        if (--_currentWaitFrame <= 0)Injection();
        else SmoothLookRotation(_currentWaitFrame);
    }

    private void Injection()
    {
        //方向確定
        transform.rotation = Quaternion.Euler(_lookAngle);

        //連続打ちのFRAMEを管理
        if (--_continueFrame > 0) return;

        //団子発射
        if (!ShotDango()) return;

        //次の発射地点を決定
        NextLook();
    }

    private bool ShotDango()
    {
        var dango = _poolManager.DangoPool.Get();
        dango.transform.position = spawner.transform.position;
        dango.Rb.AddForce(transform.forward.normalized * shotPower, ForceMode.Impulse);
        _continueFrame = defalutContinueFrame;

        return --_injectionCount <= 0;
    }

    /// <summary>
    /// アニメーションの関数。変なため変更推奨。
    /// </summary>
    /// <param name="frame"></param>
    private void SmoothLookRotation(int frame)
    {
        Vector3 interpolatedValue = _nextLookAngle - _lookAngle;

        transform.rotation = Quaternion.Euler(_nextLookAngle + (interpolatedValue / frame));
    }

    private void NextLook()
    {
        _currentWaitFrame = waitFrame;
        _nextLookAngle = transform.rotation.eulerAngles;
        _continueFrame = 0;
        _injectionCount = defalutInjectionCount;

        _lookAngle = _firstLookAngle + new Vector3(Random.Range(verticalRot.x, verticalRot.y), Random.Range(horizontalRot.x, horizontalRot.y), 0);
    }
}