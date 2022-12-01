using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollower : MonoBehaviour
{
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.MouseMove.performed += MouseMove;
    }

    private void OnDisable()
    {
        inputActions.Player.MouseMove.performed -= MouseMove;
        inputActions.Player.Disable();
    }

    private void MouseMove(InputAction.CallbackContext context)
    {
        Vector2 screen = context.ReadValue<Vector2>();
        Vector3 pos = Camera.main.ScreenToWorldPoint(screen);
        pos.z = 0.0f;
        transform.position = pos;
    }
}
