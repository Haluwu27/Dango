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
    private Vector3 idou;
    public float angle;
    private DangoType dangoType;


    //�ړ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveAxis = context.ReadValue<Vector2>().normalized;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveAxis = Vector2.zero;
            //�������������Ɨǂ�����
        }
    }

    //�W�����v����
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
    }

    //�c�q�e
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //�c�q���h�����ĂȂ������烊�^�[���B
            if (dangos.Count == 0) return;

            //[UI�o���������]�������擪�c�q�̕\��
            Logger.Log(dangos[dangos.Count - 1]);

            //�擪������
            dangos.RemoveAt(dangos.Count - 1);
            DangoUISC.DangoUISet(dangos);
        }
    }

    //�˂��h��
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //�h�����Ă�c�q�̐����A�h����c�q�̐��Ɠ����������烊�^�[���B
            if (dangos.Count >= Maxdango)
            {
                Logger.Warn("�h����c�q�̐��𒴂��Ă��܂��B");
                return;
            }

            //�˂��h���A�j���[�V���������B
            //���Ƃ���bool�^�����Ⴄ�H
            spitManager.canStab = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);

            //�h�����c�q���擾
            var dangoType = spitManager.GetDangoType();

            //�擾�����c�q��None����Ȃ���������ɒǉ��B
            if (dangoType != DangoColor.None)
            {
                dangos.Add(dangoType);
            }
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            //�˂��h�������Ƃ̃A�j���[�V���������B
            spitManager.canStab = false;
            spitManager.gameObject.transform.rotation = Quaternion.identity;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0.4f, 1.1f);
        }
    }

    //�H�ׂ�
    public void OnEatDango(InputAction.CallbackContext context)
    {
        //�������Ɏh�����ĂȂ������烊�^�[���B
        if (dangos.Count == 0) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                Logger.Log("�H�׃`���[�W�J�n�I�I");
                //���ʉ��Ƃ����o�I�Ȃ��̂�ǉ������B

                break;
            case InputActionPhase.Performed:
                Logger.Log("�H�ׂ��I");
                //���ʉ��Ƃ����o�I�Ȃ��̂�ǉ������B

                //���ƂłȂ�Ƃ����ĂˁB
                var tensuu = DangoRole.CheckRole(dangos);
                Logger.Log("�_���F"+tensuu);

                //�����c�q�����Z�b�g
                dangos.Clear();
                DangoUISC.DangoUISet(dangos);
                break;
        }
    }

    //��]
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

    //���k�i�f�o�t�j����
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
    /// �����c�q�̃��X�g
    /// </summary>
    private List<DangoColor> dangos = new List<DangoColor>();

    /// <summary>
    /// �h����c�q�̍ő吔
    /// </summary>    
    private int Maxdango = 3;

    private DangoUIScript DangoUISC;

    public List<DangoColor> GetDangoType() => dangos;
    public int GetMaxDango() => Maxdango;

    //�C���K�v�H�H
    public DangoColor GetDangoType(int value)
    {
        try
        {
            return dangos[value];
        }
        catch (IndexOutOfRangeException e)
        {
            Logger.Error(e + "�c�q�T�C�Y�͈̔͊O�ɃA�N�Z�X���Ă��܂��B");
            Logger.Error("����ɍŏ��̃f�[�^��ԋp���܂��B");
            return dangos[0];
        }
    }

    private void OnEnable()
    {
        //������
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
        if (dangoType != DangoType.None && dangoNum <= Maxdango)
        {
            dangos[dangoNum] = dangoType;
            dangoNum++;
            DangoUISC.DangoUISet(dangos);
            Logger.Log("�c�q�̒ǉ�");
        }
    }

    private void FixedUpdate()
    {
        Vector3 move;
        angle = roteAxis.x;

        //�J�����̌������m�F�ACameraforward�ɑ��
        Cameraforward = Vector3.Scale(PlayerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        //�J�����̌��������Ƀx�N�g���̍쐬
        move = moveAxis.y * Cameraforward * _moveSpeed + moveAxis.x * PlayerCamera.transform.right * _moveSpeed;

        if (_rigidbody.velocity.magnitude < 8f)
            _rigidbody.AddForce(move * _moveSpeed);

        //player�̌������J�����̕�����
        transform.rotation = Quaternion.Euler(0, PlayerCamera.transform.localEulerAngles.y , 0);
    }
}
