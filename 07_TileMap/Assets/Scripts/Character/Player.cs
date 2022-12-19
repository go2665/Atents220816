using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 3.0f;

    Animator anim;
    Rigidbody2D rigid;

    PlayerInputActions inputActions;

    Vector2 inputDir;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + Time.deltaTime * speed * inputDir);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        anim.SetFloat("InputX", inputDir.x);
        anim.SetFloat("InputY", inputDir.y);
        anim.SetBool("IsMove", true);
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        anim.SetBool("IsMove", false);
        inputDir = Vector2.zero;
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
    }
}
