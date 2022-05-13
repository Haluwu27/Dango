//  ＜＜＜参照サイト＞＞＞
//
//https://nekojara.city/unity-input-system-player-input#Player_Input%E3%81%AE%E9%81%A9%E7%94%A8
//
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem_Move : MonoBehaviour
{
    private Vector2 axis;

    private void FixedUpdate()
    {
    }

    // メソッド名は何でもOK
    // publicにする必要がある
    public void OnMove(InputAction.CallbackContext context)
    {
        // MoveActionの入力値を取得
        axis = context.ReadValue<Vector2>().normalized;
        Logger.Log(axis);
    }

    public Vector2 GetMoveAxis() => axis;
}