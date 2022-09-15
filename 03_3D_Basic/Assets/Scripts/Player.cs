using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 180.0f;

    float moveDir = 0.0f;
    float rotateDir = 0.0f;

    Rigidbody rigid;

    PlayerInputActions inputActions;                // PlayerInputActions타입이고 inputActions 이름을 가진 변수를 선언.

    private void Awake()
    {
        inputActions = new PlayerInputActions();    // 인스턴스 생성. 실제 메모리를 할당 받고 사용할 수 있도록 만드는 것.
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();   // Player 액션맵에 들어있는 액션들을 처리하겠다.
        inputActions.Player.Move.performed += OnMoveInput;  // Move 액션에 연결된 키가 눌러졌을 때 실행되는 함수를 연결(바인딩)
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;  // 바인딩 해제
        inputActions.Player.Disable();  // Player 액션맵에 있는 액션들은 처리하지 않겠다.
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();   // 입력된 값을 읽어오기
        //Debug.Log(input);
        moveDir = input.y;      // w : +1,  s : -1   전진인지 후진인지 결정
        rotateDir = input.x;    // a : -1,  d : +1   좌회전인지 우회전인지 결정
    }

    void Move()
    {
        // 리지드바디로 이동 설정
        rigid.MovePosition(rigid.position + moveSpeed * Time.fixedDeltaTime * moveDir * transform.forward);
    }

    void Rotate()
    {
        // 리지드바디로 회전 설정
        //rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, rotateDir * rotateSpeed * Time.fixedDeltaTime, 0));
        rigid.MoveRotation(rigid.rotation * Quaternion.AngleAxis(rotateDir * rotateSpeed * Time.fixedDeltaTime, transform.up));

        // Quaternion.Euler(0, rotateDir * rotateSpeed * Time.fixedDeltaTime, 0) // x,z축은 회전 없과 y축 기준으로 회전
        // Quaternion.AngleAxis(rotateDir * rotateSpeed * Time.fixedDeltaTime, transform.up) // 플레이어의 Y축 기준으로 회전
    }

}
