using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystemManager;

/// <summary>
/// InputSystemのキー入力が、画面を生成したり廃棄したりするとバグるため
/// 総括的なキー入力の結果を受け取るものをここに定義します
/// </summary>
public class InputSystemManager : MonoBehaviour
{
    public static InputSystemManager Instance { get; private set; }

    public PlayerInput Input;

    //ある地点になった際に実行されるコールバック関数
    public delegate void CallBack();

    #region Player
    public CallBack onMovePerformed;
    public CallBack onMoveCanceled;
    public CallBack onLookPerformed;
    public CallBack onLookCanceled;
    public CallBack onJumpPerformed;
    public CallBack onJumpCanceled;
    public CallBack onAttackPerformed;
    public CallBack onAttackCanceled;
    public CallBack onEatDangoPerformed;
    public CallBack onEatDangoCanceled;
    public CallBack onFirePerformed;
    public CallBack onFireCanceled;
    public CallBack onPausePerformed;
    public CallBack onPauseCanceled;

    Vector2 _moveAxis;
    Vector2 _lookAxis;
    bool _isPressJump;
    bool _isPressAttack;
    bool _isPressEatDango;
    bool _isPressFire;
    bool _isPressPause;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onMovePerformed.SafeCall();
            _moveAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onMoveCanceled.SafeCall();
            _moveAxis = Vector2.zero;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onLookPerformed.SafeCall();
            _lookAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onLookCanceled.SafeCall();
            _lookAxis = Vector2.zero;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onJumpPerformed.SafeCall();
            _isPressJump = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onJumpCanceled.SafeCall();
            _isPressJump = false;
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onAttackPerformed.SafeCall();
            _isPressAttack = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onAttackCanceled.SafeCall();
            _isPressAttack = false;
        }
    }
    public void OnEatDango(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onEatDangoPerformed.SafeCall();
            _isPressEatDango = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onEatDangoCanceled.SafeCall();
            _isPressEatDango = false;
        }
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onFirePerformed.SafeCall();
            _isPressFire = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onFireCanceled.SafeCall();
            _isPressFire = false;
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onPausePerformed.SafeCall();
            _isPressPause = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onPauseCanceled.SafeCall();
            _isPressPause = false;
        }
    }

    public Vector2 MoveAxis => _moveAxis;
    public Vector2 LookAxis => _lookAxis;
    public bool IsPressJump => _isPressJump;
    public bool IsPressAttack => _isPressAttack;
    public bool IsPressEatDango => _isPressEatDango;
    public bool IsPressFire => _isPressFire;
    public bool IsPressPause => _isPressPause;
    #endregion

    #region UI
    public CallBack onNavigatePerformed;
    public CallBack onNavigateCanceled;
    public CallBack onBackPerformed;
    public CallBack onBackCanceled;
    public CallBack onChoicePerformed;
    public CallBack onChoiceCanceled;
    public CallBack onStickPerformed;
    public CallBack onStickCanceled;

    Vector2 _navigateAxis;
    bool _isPressBack;
    bool _isPressChoice;
    Vector2 _stickAxis;

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onNavigatePerformed.SafeCall();
            _navigateAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onNavigateCanceled.SafeCall();
            _navigateAxis = Vector2.zero;
        }
    }
    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onBackPerformed.SafeCall();
            _isPressBack = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onBackCanceled.SafeCall();
            _isPressBack = false;
        }
    }
    public void OnChoice(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onChoicePerformed.SafeCall();
            _isPressChoice = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onChoiceCanceled.SafeCall();
            _isPressChoice = false;
        }
    }
    public void OnStick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onStickPerformed.SafeCall();
            _stickAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            onStickCanceled.SafeCall();
            _stickAxis = Vector2.zero;
        }
    }

    public Vector2 NavigateAxis => _navigateAxis;
    public bool IsPressBack => _isPressBack;
    public bool IsPressChoice => _isPressChoice;
    public Vector2 StickAxis => _stickAxis;
    #endregion

    private void Awake()
    {
        Instance = this;
    }
}

//拡張メソッドです。コールバック関数を呼ぶとnullの際にエラー吐くのですが
//毎回nullチェックするのは嫌なので新たに作りました。
public static class CallBackExpantion
{
    public static CallBack SafeCall(this CallBack performed)
    {
        if (performed == null) return null;

        performed.Invoke();

        return performed;
    }
}