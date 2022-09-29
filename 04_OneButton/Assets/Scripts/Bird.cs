using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bird : MonoBehaviour
{
    /// <summary>
    /// 점프력
    /// </summary>
    [Range(1.0f, 10.0f)]
    public float jumpPower = 7.0f;

    /// <summary>
    /// 위나 아래로 움직일 때 회전 최대값
    /// </summary>
    public float pitchMaxAngle = 45.0f;

    /// <summary>
    /// 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action onDead;

    /// <summary>
    /// 새가 죽었는지 살았는지 표시하는 값
    /// </summary>
    bool isDead = false;

    /// <summary>
    /// InputSystem용 에셋 변수
    /// </summary>
    BirdInputActions inputActions;
    
    /// <summary>
    /// 물리 적용을 위한 리지드바디
    /// </summary>
    Rigidbody2D rigid;


    // 유니티 이벤트 함수 --------------------------------------------------------------------------
    private void Awake()    // 이 스크립트(컴포넌트)가 생성 완료 되었을 때
    {
        inputActions = new BirdInputActions();      // 인풋액션 객체 생성
        rigid = GetComponent<Rigidbody2D>();        // 리지드바디 컴포넌트 찾기
    }

    private void Start()    // 첫번째 Update 함수가 실행되기 직전
    {
        isDead = false;                             // 우선 살아있다고 표시
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

    //private void Update()   // 매 프레임마다 호출
    //{        
    //}

    private void FixedUpdate()  // 물리 업데이트 주기 마다(고정된 시간)
    {
        if (!isDead)    // 살아있을 때만 아래 코드 실행
        {
            //rigid.velocity.y;   // +이면 새가 위로 올라가고 있다. -면 새가 아래로 내려가고 있다.
            float velocityY = Mathf.Clamp(rigid.velocity.y, -jumpPower, jumpPower); // jumpPower만큼 최대/최소값 지정
            float angle = (velocityY / jumpPower) * pitchMaxAngle;  // 올라가거나 내려가는 정도에 따라 각도 설정

            rigid.MoveRotation(angle);                              // 설정된 각도로 회전
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)  // 다른 컬라이더와 충돌했을 때 실행
    {
        Die(collision.GetContact(0));       // 사망처리하면서 충돌한 지점에 대한 정보 전달
    }

    // 내부 함수들 ---------------------------------------------------------------------------------
    /// <summary>
    /// 사망처리 함수
    /// </summary>
    /// <param name="contact">사망했을 때 충돌 정보</param>
    void Die(ContactPoint2D contact)
    {
        Vector2 dir = (contact.point - (Vector2)transform.position).normalized; // 새가 충돌 지점으로 가는 방향
        Vector2 reflect = Vector2.Reflect(dir, contact.normal);                 // dir이 반사된 벡터
        rigid.velocity = reflect * 10.0f + Vector2.left * 5.0f;                 // 반사되는 방향에 왼쪽으로 가는 힘 추가
        rigid.angularVelocity = 1000.0f;                                        // 회전 추가(초당 1000도)

        // 처음 죽었을 때만 처리하는 코드
        if (!isDead)    
        {
            // 아직 살아있다고 표시될 때만 실행
            onDead?.Invoke();               // 사망 알림용 델리게이트 실행
            inputActions.Bird.Disable();    // 죽었을 때 입력 막기
            isDead = true;                  // 죽었다고 표시
        }        
    }

    // 입력 처리용 함수 ----------------------------------------------------------------------------
    /// <summary>
    /// 점프 입력이 들어왔을 때 실행되는 함수
    /// </summary>
    /// <param name="_">사용안함</param>
    private void OnJump(InputAction.CallbackContext _)
    {
        rigid.velocity = Vector2.up * jumpPower;    // 위쪽 방향으로 점프력만큼 velocity 변경
    }
}
