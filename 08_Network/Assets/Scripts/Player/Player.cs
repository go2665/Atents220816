using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 3.5f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 350f;

    /// <summary>
    /// 이번 프레임에 움직여야 할 이동량
    /// </summary>
    Vector3 moveDelta;

    /// <summary>
    /// 이번 프레임에 회전해야 할 회전량
    /// </summary>
    float rotateDelta;

    // 컴포넌트들
    CharacterController contoller;
    PlayerInputActions inputActions;

    private void Awake()
    {
        contoller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        contoller.SimpleMove(moveDelta);
        transform.Rotate(0, rotateDelta * Time.deltaTime, 0, Space.World);
    }

    /// <summary>
    /// 입력을 하거나 취소했을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveDelta = moveInput.y * moveSpeed * transform.forward;    // 이동 입력 저장하기
        rotateDelta = moveInput.x * rotateSpeed;                    // 회전 입력 저장하기
    }
}
