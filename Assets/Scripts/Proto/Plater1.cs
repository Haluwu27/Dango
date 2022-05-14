using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Plater1 : MonoBehaviour
{
    #region inputSystem
    private Vector2 moveAxis;
    [SerializeField]
    private Rigidbody _rigidbody;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveAxis = context.ReadValue<Vector2>().normalized;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveAxis = Vector2.zero;
            //減速処理入れると良さそう
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Logger.Log("団子弾");
            for(int i = 0; i < Maxdango; i++)
            {
                Logger.Log(i+"番目の色"+dangos[i]);
            }
            if (dangoNum != 0)
            {
                Logger.Log(dangos[dangoNum - 1]);

                dangos[dangoNum - 1] = DangoType.None;
                dangoNum--;
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (dangoNum >= Maxdango)
            {
                Logger.Warn("刺せる団子の数を超えています。");
                return;
            }
            Logger.Log("突き刺し！");
            spitManager.canStab = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);
            var dangoType = spitManager.GetDangoType();
            if (dangoType != DangoType.None&&dangoNum<=Maxdango)
            {
                dangos[dangoNum] = dangoType;
                dangoNum++;
            }
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            spitManager.canStab = false;
            spitManager.gameObject.transform.rotation = Quaternion.identity;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0.4f, 1.1f);
        }
    }

    public void OnEatDango(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Logger.Log("食べチャージ開始！！");
                break;
            case InputActionPhase.Performed:
                Logger.Log("食べた！");
                //役の判定

                dangoNum = 0;
                break;
        }
    }

    #endregion

    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float jumpPower = 10f;
    [SerializeField] SpitManager spitManager;
    DangoType[] dangos;
    int dangoNum = 0;
    int Maxdango = 7;

    private void Start()
    {
        dangos = new DangoType[Maxdango];
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveAxis.x, 0, moveAxis.y);
        if (_rigidbody.velocity.magnitude < 8f)
            _rigidbody.AddForce(move * _moveSpeed);
    }
}
