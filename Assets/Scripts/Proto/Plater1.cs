using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Plater1 : MonoBehaviour
{
    #region inputSystem
    private Vector2 moveAxis;
    private Vector2 roteAxis;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private GameObject PlayerCamera;
    private Vector3 Cameraforward;
    private Vector3 idou;
    public float angle;


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
            //減速処理入れると良さそう
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
            //団子が刺さってなかったらリターン。
            if (dangos.Count == 0) return;

            //[UI出来たら消す]消した先頭団子の表示
            Logger.Log(dangos[dangos.Count - 1]);

            //先頭を消す
            dangos.RemoveAt(dangos.Count - 1);
        }
    }

    //突き刺し
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //刺さってる団子の数が、刺せる団子の数と同じだったらリターン。
            if (dangos.Count >= Maxdango)
            {
                Logger.Warn("刺せる団子の数を超えています。");
                return;
            }

            //突き刺しアニメーション推奨。
            //あとこのbool型多分違う？
            spitManager.canStab = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);

            //刺した団子を取得
            var dangoType = spitManager.GetDangoType();

            //取得した団子がNoneじゃなかったら串に追加。
            if (dangoType != DangoColor.None)
            {
                dangos.Add(dangoType);
            }
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            //突き刺したあとのアニメーション推奨。
            spitManager.canStab = false;
            spitManager.gameObject.transform.rotation = Quaternion.identity;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0.4f, 1.1f);
        }
    }

    //食べる
    public void OnEatDango(InputAction.CallbackContext context)
    {
        //何も串に刺さってなかったらリターン。
        if (dangos.Count == 0) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                Logger.Log("食べチャージ開始！！");
                //効果音とか視覚的なものを追加推奨。

                break;
            case InputActionPhase.Performed:
                Logger.Log("食べた！");
                //効果音とか視覚的なものを追加推奨。

                //あとでなんとかしてね。
                var tensuu = DangoRole.CheckRole(dangos);
                Logger.Log("点数："+tensuu);

                //所持団子をリセット
                dangos.Clear();
                break;
        }
    }

    //回転
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

    //圧縮（デバフ）発動
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
    /// 所持団子のリスト
    /// </summary>
    private List<DangoColor> dangos = new List<DangoColor>();

    /// <summary>
    /// 刺せる団子の最大数
    /// </summary>    
    private int Maxdango = 3;

    public List<DangoColor> GetDangoType() => dangos;
    public int GetMaxDango() => Maxdango;

    //修正必要？？
    public DangoColor GetDangoType(int value)
    {
        try
        {
            return dangos[value];
        }
        catch (IndexOutOfRangeException e)
        {
            Logger.Error(e + "団子サイズの範囲外にアクセスしています。");
            Logger.Error("代わりに最初のデータを返却します。");
            return dangos[0];
        }
    }

    private void OnEnable()
    {
        //初期化
        dangos.Clear();
    }

    private void Update()
    {
        if (_hitPoint <= 0) gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        //Vector3 move = new Vector3(moveAxis.x, 0, moveAxis.y);
        Vector3 move;
        angle = roteAxis.x;

        //カメラの向きを確認、Cameraforwardに代入
        Cameraforward = Vector3.Scale(PlayerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        //カメラの向きを元にベクトルの作成
        move = moveAxis.y * Cameraforward * _moveSpeed + moveAxis.x * PlayerCamera.transform.right * _moveSpeed;

        if (_rigidbody.velocity.magnitude < 8f)
            _rigidbody.AddForce(move * _moveSpeed * Time.deltaTime);
        //playerの向きをカメラの方向に
        transform.rotation = Quaternion.Euler(0, PlayerCamera.transform.localEulerAngles.y + Time.deltaTime, 0);
    }
}
