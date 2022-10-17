using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]   // 필수적으로 필요한 컴포넌트가 있을 때 자동으로 넣어주는 유니티 속성(Attribute)
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    // 웨이포인트 관련 변수 -------------------------------------------------------------------------   
    /// <summary>
    /// 적이 순찰할 웨이포인트들
    /// </summary>
    public Waypoints waypoints;     
    
    /// <summary>
    /// 지금 적이 이동할 목표 지점의 트랜스폼
    /// </summary>
    Transform moveTarget;

    /// <summary>
    /// 지금 적이 이동하는 방향(바라보는 방향)
    /// MoveTarget이 갱신될 때 함께 갱신됨
    /// </summary>
    Vector3 lookDir;
    // --------------------------------------------------------------------------------------------

    // 이동 관련 변수 ------------------------------------------------------------------------------
    /// <summary>
    /// 적의 이동 속도
    /// </summary>
    public float moveSpeed = 3.0f;

    /// <summary>
    /// 초당 이동 속도
    /// </summary>
    float moveSpeedPerSecond;   // 계속 연산되는 부분이라 한번만 계산한 후 저장해놓는 용도
    // --------------------------------------------------------------------------------------------

    // 상태 관련 변수 ------------------------------------------------------------------------------
    EnemyState state;               // 현재 적의 상태(대기 상태냐 순찰 상태냐)
    public float waitTime = 1.0f;   // 목적지에 도착했을 때 기다리는 시간
    float waitTimer;                // 남아있는 기다려야 하는 시간
    // --------------------------------------------------------------------------------------------

    // 컴포넌트 캐싱용 변수 -------------------------------------------------------------------------
    Rigidbody rigid;
    Animator anim;
    // --------------------------------------------------------------------------------------------

    // 추가 데이터 타입 ----------------------------------------------------------------------------

    /// <summary>
    /// 적의 상태를 나타내기 위한 enum
    /// </summary>
    protected enum EnemyState
    {
        Wait = 0,   // 대기 상태
        Patrol      // 순찰 상태
    }
    // --------------------------------------------------------------------------------------------

    // 델리게이트 ----------------------------------------------------------------------------------
    
    /// <summary>
    /// 상태별 업데이터 함수를 가질 델리게이트
    /// </summary>
    Action stateUpdate;
    // --------------------------------------------------------------------------------------------

    // 프로퍼티 -----------------------------------------------------------------------------------

    /// <summary>
    /// 이동할 목적지를 나타내는 프로퍼티
    /// </summary>
    protected Transform MoveTarget
    {
        get => moveTarget;
        set
        {
            moveTarget = value;
            lookDir = (moveTarget.position - transform.position).normalized;    // lookDir도 함께 갱신
        }
    }

    /// <summary>
    /// 적의 상태를 나타내는 프로퍼티
    /// </summary>
    protected EnemyState State
    {
        get => state;
        set
        {
            //switch (state)  // 이전 상태(상태를 나가면서 해야 할 일 처리)
            //{
            //    case EnemyState.Wait:
            //        break;
            //    case EnemyState.Patrol:
            //        break;
            //    default:
            //        break;
            //}
            state = value;  // 새로운 상태로 변경
            switch (state)  // 새로운 상태(새로운 상태로 들어가면서 해야 할 일 처리)
            {
                case EnemyState.Wait:
                    waitTimer = waitTime;       // 타이머 초기화
                    anim.SetTrigger("Stop");    // 가만히 있는 애니메이션 재생
                    stateUpdate = Update_Wait;  // FixedUpdate에서 실행될 델리게이트 변경
                    break;
                case EnemyState.Patrol:
                    anim.SetTrigger("Move");    // 이동하는 애니메이션 재생
                    stateUpdate = Update_Patrol;// FixedUpdate에서 실행될 델리게이트 변경
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 남은 대기 시간을 나타내는 프로퍼티
    /// </summary>
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if( waitTimer < 0.0f )  // 남은 시간이 다 되면
            {   
                State = EnemyState.Patrol;  // Patrol 상태로 전환
            }
        }
    }
    // --------------------------------------------------------------------------------------------

    private void Awake()
    {
        // 컴포넌트 찾기
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        moveSpeedPerSecond = moveSpeed * Time.fixedDeltaTime;   // 자주 사용하는 연산 미리 계산해서 저장해 놓기

        // waypoints가 없을 때를 대비한 코드
        if (waypoints != null)
        {
            MoveTarget = waypoints.Current;
        }
        else
        {
            MoveTarget = transform;
        }

        // 값 초기화 작업      
        State = EnemyState.Wait;    // 기본 상태 설정(wait)
        anim.ResetTrigger("Stop");  // 트리거가 쌓이는 현상을 방지
    }

    private void FixedUpdate()
    {
        stateUpdate();
    }

    /// <summary>
    /// Patrol 상태일 때 실행될 업데이트 함수
    /// </summary>
    void Update_Patrol()
    {
        // 이동 처리
        rigid.MovePosition(transform.position + moveSpeedPerSecond * lookDir);  // 위치변경
        rigid.rotation = Quaternion.Slerp(rigid.rotation, Quaternion.LookRotation(lookDir), 0.2f);  // 이동하는 방향 바라보기

        // 도착 확인
        if ((transform.position - moveTarget.position).sqrMagnitude < 0.01f)
        {
            transform.position = moveTarget.position;   // 정확한 웨이포인트 지점에 이동 시키기 위해 강제 이동
            MoveTarget = waypoints.MoveNext();          // 다음 웨이포인트 지점을 MoveTarget으로 설정
            State = EnemyState.Wait;                    // 대기 상태로 변경
        }
    }

    /// <summary>
    /// Wait 상태일 때 실행될 업데이트 함수
    /// </summary>
    void Update_Wait()
    {
        WaitTimer -= Time.fixedDeltaTime;   // 시간 지속적으로 감소
    }
}
