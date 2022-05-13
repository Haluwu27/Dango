using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IS_PlayerLookAt_RStick : MonoBehaviour
{
    private Vector2 axis;
    private void Update()
    {
       
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // LookAction‚Ì“ü—Í’l‚ðŽæ“¾
        axis = context.ReadValue<Vector2>().normalized;
    }

    public Vector2 GetLookAxis() => axis;

}
