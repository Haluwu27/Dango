using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            if (isGround)
            {
                _rigidbody.AddForce(Vector3.up * (_jumpPower + (Maxdango)), ForceMode.Impulse);
            }
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

    //突き刺しアニメーション
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //突き刺せる数を超えていた場合、実行しない
            if (dangos.Count >= Maxdango)
            {
                //なんらかの突けないことを知らせる処理推奨。

                Logger.Warn("突き刺せる数を超えています");
                _event.text = "それ以上させないよ！";
                return;
            }

            //ここに突き刺しアニメーションを推奨。
            spitManager.isSticking = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);

        }
        if (context.phase == InputActionPhase.Canceled)
        {
            //ここに突き刺し終わりのアニメーションを推奨。
            spitManager.isSticking = false;
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
                _event.text = "食べチャージ中！";
                //SE推奨

                break;
            case InputActionPhase.Performed:
                Logger.Log("食べた！！");
                //SE推奨

                //食べた団子の点数を取得
                var score = DangoRole.CheckRole(dangos);

                _event.text = "食べた！" + (int)score + "点！";

                //満腹度を上昇
                _satiety += score;

                //スコアを上昇
                GameManager.gameScore += score * 100f;

                //串をクリア。
                dangos.Clear();

                //UI更新
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

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _jumpPower = 10f;
    //[SerializeField] private float _attackPower = 1f;
    //[SerializeField] private float _attackSpeed = 1f;
    //[SerializeField] private float _hitPoint = 100f;
    //[SerializeField] private float _strength = 1f;
    [SerializeField] private SpitManager spitManager;
    [SerializeField] private DangoUIScript DangoUISC;


    //仮UI用加筆分
    private TextMeshProUGUI _time;
    private TextMeshProUGUI _event;
    private TextMeshProUGUI _role;

    /// <summary>
    /// 満腹度、制限時間の代わり（単位:[sec]）
    /// </summary>
    /// フレーム数で管理しますが、ここでは秒管理で構いません。
    private float _satiety = 100f;

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

    private float time = 0;
    internal bool isGround;

    internal Vector3 move;

    private void OnEnable()
    {
        InitDangos();
    }

    private void Start()
    {
        if (DangoUISC == null)
        {
            DangoUISC = GameObject.Find("Canvas").transform.Find("DangoBackScreen").GetComponent<DangoUIScript>();
        }

        if (_time == null)
        {
            _time = GameObject.Find("Canvas").transform.Find("Time").GetComponent<TextMeshProUGUI>();
        }

        if (_event == null)
        {
            _event = GameObject.Find("Canvas").transform.Find("Event").GetComponent<TextMeshProUGUI>();
        }

        if (_role == null)
        {
            _role = GameObject.Find("Canvas").transform.Find("Role").GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        IsGround();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        DecreaseSatiety();
        GrowStab();

        //仮でここに
        _time.text = "残り時間：" + (int)_satiety + "秒";
        _role.text = GameManager.NowRoleList;

    }

    private void InitDangos()
    {
        if (dangos == null) return;

        //初期化
        dangos.Clear();
    }

    private void IsGround()
    {
        var ray = new Ray(transform.position, Vector3.down);
        isGround = Physics.Raycast(ray, 1f);
    }

    /// <summary>
    /// Playerをカメラの方向に合わせて動かす関数。
    /// </summary>
    private void PlayerMove()
    {
        angle = roteAxis.x;

        //カメラの向きを確認、Cameraforwardに代入
        var Cameraforward = Vector3.Scale(PlayerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        //カメラの向きを元にベクトルの作成
        Vector3 move = moveAxis.y * Cameraforward * _moveSpeed + moveAxis.x * PlayerCamera.transform.right * _moveSpeed;

        if (_rigidbody.velocity.magnitude < 8f)
            _rigidbody.AddForce(move * _moveSpeed);
    }

    /// <summary>
    /// 満腹度をへらす関数、fixedUpdateに配置。
    /// </summary>
    private void DecreaseSatiety()
    {
        //満腹度を0.02秒(fixedUpdateの呼ばれる秒数)減らす
        _satiety -= Time.fixedDeltaTime;

        //ゲームマネージャー管理のほうがいいと思うけど
        //とりあえずここに置いておきます。
        FinishGame();

        //[debug]10秒おきにデバッグログを表示
        if ((int)_satiety % 10 == 0) Logger.Log(_satiety);
    }

    /// <summary>
    /// 串が一定時間で伸びる処理
    /// </summary>
    private void GrowStab()
    {
        //刺せる団子の数が7だったら実行しない。
        if (Maxdango == 7) return;

        float growTime = 10f;

        time += Time.fixedDeltaTime;

        if (time >= growTime)
        {
            Maxdango++;
            Logger.Log("させる団子の数が増えた！" + Maxdango);
            _event.text = "させる団子の数が増えた！(" + Maxdango + "個)";
            time = 0;
        }
    }

    private void FinishGame()
    {
        var madeCount = 0;
        if (_satiety <= 0)
        {
            var posRoles = DangoRole.GetPosRoles();
            foreach (var posRole in posRoles)
            {
                if (posRole.GetMadeCount() > 0)
                {
                    madeCount++;
                }
            }
            Logger.Log("満足度：" + GameManager.gameScore * madeCount);
        }
    }

    public Vector2 GetRoteAxis() => roteAxis;
    public List<DangoColor> GetDangoType() => dangos;
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
    public int GetMaxDango() => Maxdango;
    public List<DangoColor> GetDangos() => dangos;
    public void AddDangos(DangoColor d) => dangos.Add(d);
}
