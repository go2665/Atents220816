using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RunningMan : MonoBehaviour
{
    public float jumpPower = 10.0f;

    BirdInputActions inputActions;
    
    Rigidbody2D rigid;
    Animator anim;

    bool isJump = true;

    public Action onDead;


    // 유니티 이벤트 함수 --------------------------------------------------------------------------
    private void Awake()    // 이 스크립트(컴포넌트)가 생성 완료 되었을 때
    {
        inputActions = new BirdInputActions();      // 인풋액션 객체 생성
        rigid = GetComponent<Rigidbody2D>();        // 리지드바디 컴포넌트 찾기
        anim = GetComponent<Animator>();
    }

    private void OnEnable() // 오브젝트가 활성화 될 때
    {
        inputActions.Bird.Enable();                 // 인풋 액션 활성화
        inputActions.Bird.Jump.performed += OnJump; // 점프 액션과 OnJump 함수 연결
    }


    private void OnDisable()
    {
        inputActions.Bird.Jump.performed -= OnJump; // 점프 액션과 OnJump 함수 연결 해제
        inputActions.Bird.Disable();                // 인풋 액션 비활성화
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            anim.SetBool("IsJump", isJump);
        }
    }

    private void OnJump(InputAction.CallbackContext _)
    {
        if(!isJump)
        {
            //rigid.velocity = Vector2.up * jumpPower;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJump = true;
            anim.SetBool("IsJump", isJump);
        }
    }

    public void Die()
    {
        Debug.Log("사망");
        onDead?.Invoke();        
    }
}
