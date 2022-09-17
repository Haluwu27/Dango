using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    public CallBack onExpansionUI;

    Vector2 _moveAxis;
    Vector2 _lookAxis;
    bool _isPressJump;
    bool _isPressAttack;
    bool _isPressEatDango;
    bool _isPressFire;
    bool _isPressPause;
    bool _isPressExpansionUI;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _moveAxis = context.ReadValue<Vector2>();
            onMovePerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _moveAxis = Vector2.zero;
            onMoveCanceled.SafeCall();
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _lookAxis = context.ReadValue<Vector2>();
            onLookPerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _lookAxis = Vector2.zero;
            onLookCanceled.SafeCall();
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isPressJump = true;
            onJumpPerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressJump = false;
            onJumpCanceled.SafeCall();
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isPressAttack = true;
            onAttackPerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressAttack = false;
            onAttackCanceled.SafeCall();
        }
    }
    public void OnEatDango(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isPressEatDango = true;
            onEatDangoPerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressEatDango = false;
            onEatDangoCanceled.SafeCall();
        }
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isPressFire = true;
            onFirePerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressFire = false;
            onFireCanceled.SafeCall();
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isPressPause = true;
            onPausePerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressPause = false;
            onPauseCanceled.SafeCall();
        }
    }
    public void OnExpansionUI(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isPressExpansionUI = true;
            onExpansionUI.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressExpansionUI = false;
            onExpansionUI.SafeCall();
        }
    }

    public Vector2 MoveAxis => _moveAxis;
    public Vector2 LookAxis => _lookAxis;
    public bool IsPressJump => _isPressJump;
    public bool IsPressAttack => _isPressAttack;
    public bool IsPressEatDango => _isPressEatDango;
    public bool IsPressFire => _isPressFire;
    public bool IsPressPause => _isPressPause;
    public bool IsExpantionUI => _isPressExpansionUI;
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
    public CallBack onAnyKeyPerformed;
    public CallBack onAnyKeyCanceled;
    public CallBack onTabControlPerformed;
    public CallBack onTabControlCanceled;

    Vector2 _navigateAxis;
    bool _isPressBack;
    bool _isPressChoice;
    Vector2 _stickAxis;
    bool _isPressAnyKey;
    bool _wasPressedThisFrameAnyKey;
    Vector2 _tabControlAxis;

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _navigateAxis = context.ReadValue<Vector2>();
            onNavigatePerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _navigateAxis = Vector2.zero;
            onNavigateCanceled.SafeCall();
        }
    }
    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isPressBack = true;
            onBackPerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressBack = false;
            onBackCanceled.SafeCall();
        }
    }
    public void OnChoice(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isPressChoice = true;
            onChoicePerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressChoice = false;
            onChoiceCanceled.SafeCall();
        }
    }
    public void OnStick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _stickAxis = context.ReadValue<Vector2>();
            onStickPerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _stickAxis = Vector2.zero;
            onStickCanceled.SafeCall();
        }
    }
    public void OnAnykey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _wasPressedThisFrameAnyKey = true;
            _isPressAnyKey = true;
            onAnyKeyPerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressAnyKey = false;
            onAnyKeyCanceled.SafeCall();
        }
    }
    public void OnTabControl(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _tabControlAxis = context.ReadValue<Vector2>();
            onTabControlPerformed.SafeCall();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _tabControlAxis = Vector2.zero;
            onTabControlCanceled.SafeCall();
        }
    }

    public Vector2 NavigateAxis => _navigateAxis;
    public bool IsPressBack => _isPressBack;
    public bool IsPressChoice => _isPressChoice;
    public Vector2 StickAxis => _stickAxis;
    public bool IsPressAnyKey => _isPressAnyKey;
    public bool WasPressedThisFrameAnyKey => _wasPressedThisFrameAnyKey;
    public Vector2 TabControlAxis => _tabControlAxis;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        WasPressedThisFrame(ref _wasPressedThisFrameAnyKey);
    }

    private void WasPressedThisFrame(ref bool key)
    {
        if (key) key = false;
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