using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plater1 : MonoBehaviour
{
    private Vector2 moveAxis;

    public void OnMove(InputAction.CallbackContext context)
    {
        // MoveAction‚Ì“ü—Í’l‚ðŽæ“¾
        moveAxis = context.ReadValue<Vector2>().normalized;
    }


    [SerializeField]
    CharacterController _controller;

    [SerializeField] float _moveSpeed = 3f;

    private void Update()
    {
        Vector3 move = new Vector3(moveAxis.x, 0, moveAxis.y);
        _controller.Move(move * Time.deltaTime * _moveSpeed);
    }

    private void FixedUpdate()
    {
        
    }
}
