using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bird : MonoBehaviour
{
    [Range(1.0f, 10.0f)]
    public float power = 7.0f;

    public float pitchMaxAngle = 45.0f;

    BirdInputActions inputActions;

    Rigidbody2D rigid;

    private void Awake()    // 이 스크립트(컴포넌트)가 생성 완료 되었을 때
    {
        inputActions = new BirdInputActions();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()    // 첫번째 Update 함수가 실행되기 직전
    {
        
    }

    private void OnEnable() // 오브젝트가 활성화 될 때
    {
        inputActions.Bird.Enable();
        inputActions.Bird.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Bird.Jump.performed -= OnJump;
        inputActions.Bird.Disable();
    }

    private void Update()   // 매 프레임마다 호출
    {
        
    }

    private void FixedUpdate()  // 물리 업데이트 주기 마다(고정된 시간)
    {
        //rigid.velocity.y;   // +이면 새가 위로 올라가고 있다. -면 새가 아래로 내려가고 있다.
        float velocityY = Mathf.Clamp(rigid.velocity.y, -power, power);
        float angle = (velocityY / power) * pitchMaxAngle;

        rigid.MoveRotation(angle);        
    }

    private void OnJump(InputAction.CallbackContext _)
    {
        rigid.velocity = Vector2.up * power;
    }
}
