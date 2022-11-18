using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollow : MonoBehaviour
{
    [Range(1.0f,20.0f)]
    public float distance = 10.0f;

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Effect.Enable();
        inputActions.Effect.CursorMove.performed += OnMouseMove;
    }

    private void OnDisable()
    {
        inputActions.Effect.CursorMove.performed -= OnMouseMove;
        inputActions.Effect.Disable();
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector3 mousePos = context.ReadValue<Vector2>();
        mousePos.z = 10.0f;

        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = target;
        Debug.Log($"Mouse Move : {mousePos}");
        Debug.Log($"World Pos : {target}");

    }

    private void Update()
    {
        //Vector3 mousePos = Mouse.current.position.ReadValue();
        //Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);

        //transform.position = target;
        //Debug.Log($"Mouse Move : {mousePos}");
        //Debug.Log($"World Pos : {target}");
    }
}
