using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player1 : MonoBehaviour
{
    #region inputSystem
    private Vector2 moveAxis;
    private Vector2 roteAxis;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private GameObject PlayerCamera;
    private Vector3 Cameraforward;
    public float angle;
    private DangoColor dangoType;


    //移動処理
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveAxis = context.ReadValue<Vector2>().normalized;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveAxis = Vector2.zero;
            //ここに減速処理を入れるの推奨
        }
    }

    //ジャンプ処理
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
    }

    //団子弾
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //串に何もなかったら実行しない。
            if (dangos.Count == 0) return;

            //[Debug]何が消えたかわかるやつ
            //今までは、dangos[dangos.Count - 1]としなければなりませんでしたが、
            //C#8.0以降では以下のように省略できるようです。
            //問題は、これを知らない人が読むとわけが分からない。
            Logger.Log(dangos[^1]);

            //消す処理。
            dangos.RemoveAt(dangos.Count - 1);
            DangoUISC.DangoUISet(dangos);
        }
    }

    //突き刺し
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //突き刺せる数を超えていた場合、実行しない
            if (dangos.Count >= Maxdango)
            {
                //なんらかの突けないことを知らせる処理推奨。

                Logger.Warn("突き刺せる数を超えています");
                return;
            }

            //ここに突き刺しアニメーションを推奨。
            spitManager.canStab = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);

            //団子を取得
            var dangoType = spitManager.GetDangoType();

            //それを串に突き刺す処理
            if (dangoType != DangoColor.None)
            {
                dangos.Add(dangoType);
            }
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            //ここに突き刺し終わりのアニメーションを推奨。
            spitManager.canStab = false;
            spitManager.gameObject.transform.rotation = Quaternion.identity;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0.4f, 1.1f);
        }
    }

    //食べる
    public void OnEatDango(InputAction.CallbackContext context)
    {
        //串に刺さってなかったら実行しない。
        if (dangos.Count == 0) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                Logger.Log("食べチャージ開始！");
                //SE推奨

                break;
            case InputActionPhase.Performed:
                Logger.Log("食べた！！");
                //SE推奨

                //この辺の処理はまだ手を付けていません。
                var tensuu = DangoRole.CheckRole(dangos);
                Logger.Log("点数:" + tensuu);

                //串をクリア。
                dangos.Clear();
                DangoUISC.DangoUISet(dangos);
                break;
        }
    }

    //回転処理
    public void OnRote(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            roteAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            roteAxis = Vector2.zero;
        }

    }

    //（現状使用しません）
    public void OnCompression(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //for(int i = 0; i < debuffs.; i++)
        }
    }

    #endregion

    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _jumpPower = 10f;
    [SerializeField] float _attackPower = 1f;
    [SerializeField] float _attackSpeed = 1f;
    [SerializeField] float _hitPoint = 100f;
    [SerializeField] float _strength = 1f;

    [SerializeField] SpitManager spitManager;

    /// <summary>
    /// 串、持ってる団子
    /// </summary>
    /// 今まではnew List<DangoColor>()としなければなりませんでしたが
    /// C#9.0以降はこのように簡素化出来るそうです。
    private List<DangoColor> dangos = new();

    /// <summary>
    /// 刺せる数、徐々に増える
    /// </summary>    
    private int Maxdango = 3;

    private DangoUIScript DangoUISC;

    public List<DangoColor> GetDangoType() => dangos;
    public int GetMaxDango() => Maxdango;

    //刺さってる団子の1要素を取得
    public DangoColor GetDangoType(int value)
    {
        try
        {
            return dangos[value];
        }
        catch (IndexOutOfRangeException e)
        {
            Logger.Error(e);
            Logger.Error("代わりに先頭（配列の0番）を返します。");
            return dangos[0];
        }
    }

    private void OnEnable()
    {
        //初期化
        dangos.Clear();
    }

    private void Start()
    {
        DangoUISC = GameObject.Find("Canvas").transform.Find("DangoBackScreen").GetComponent<DangoUIScript>();
    }

    private void Update()
    {
        if (_hitPoint <= 0) gameObject.SetActive(false);
        dangoType = spitManager.GetDangoType();
        if (dangoType != DangoColor.None && dangos.Count <= Maxdango)
        {
            dangos[dangos.Count - 1] = dangoType;
            DangoUISC.DangoUISet(dangos);
            Logger.Log("団子の追加");
        }
    }

    private void FixedUpdate()
    {
        Vector3 move;
        angle = roteAxis.x;

        //カメラの向きを確認、Cameraforwardに代入
        Cameraforward = Vector3.Scale(PlayerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        //カメラの向きを元にベクトルの作成
        move = moveAxis.y * Cameraforward * _moveSpeed + moveAxis.x * PlayerCamera.transform.right * _moveSpeed;

        if (_rigidbody.velocity.magnitude < 8f)
            _rigidbody.AddForce(move * _moveSpeed);

        //playerの向きをカメラの方向に
        transform.rotation = Quaternion.Euler(0, PlayerCamera.transform.localEulerAngles.y, 0);
    }
}
