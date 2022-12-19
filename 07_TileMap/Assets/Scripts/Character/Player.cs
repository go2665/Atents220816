using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어 이동 속도
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 애니메이터 컴포넌트
    /// </summary>
    Animator anim;

    /// <summary>
    /// 리지드바디 컴포넌트
    /// </summary>
    Rigidbody2D rigid;

    /// <summary>
    /// 인풋 시스템용 인풋 액션맵 클래스 객체
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 현재 입력된 이동 방향
    /// </summary>
    Vector2 inputDir;

    /// <summary>
    /// 공격 이후에 복구 용도로 임시 저장해 놓은 이전 입력 방향
    /// </summary>
    Vector2 oldInputDir;

    private void Awake()
    {
        // 컴포넌트 찾고 
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        // 객체 생성
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // 입력 연결
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        // 입력 연결 해제
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
        // 이동 처리
        rigid.MovePosition(rigid.position + Time.deltaTime * speed * inputDir);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // 이동 입력이 들어왔을 때
        inputDir = context.ReadValue<Vector2>();    // 입력 이동 방향 저장하고
        anim.SetFloat("InputX", inputDir.x);        // 애니메이터 파라메터 변경
        anim.SetFloat("InputY", inputDir.y);
        anim.SetBool("IsMove", true);
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        anim.SetBool("IsMove", false);  // 이동 끝 표시
        inputDir = Vector2.zero;        // 입력 이동 방향 제거
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        oldInputDir = inputDir;         // 이후 복원을 위해 입력 이동 방향 저장
        inputDir = Vector2.zero;        // 입력 이동 방향 초기화
        anim.SetTrigger("Attack");      // 공격 애니메이션 실행
    }

    /// <summary>
    /// 공격 애니메이션 state가 끝날 때 실행되는 함수
    /// </summary>
    public void RestoreInputDir()
    {
        inputDir = oldInputDir;         // 입력 이동 방향 복원
    }
}
