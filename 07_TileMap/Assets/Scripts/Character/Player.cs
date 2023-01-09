using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 수명 관련 변수들 ---------------------------------------------------------------------------

    /// <summary>
    /// 플레이어의 최대 수명
    /// </summary>
    public float maxLifeTime = 10.0f;

    /// <summary>
    /// 플레이어의 현재 수명
    /// </summary>
    float lifeTime;

    /// <summary>
    /// 전체 플레이 시간
    /// </summary>
    float totalPlayTime;

    /// <summary>
    /// 플레이어의 생존 여부 표시용
    /// </summary>
    bool isDead = false;

    /// <summary>
    /// 플레이어의 수명을 확인하거나 설정할 때의 여러 처리를 하는 프로퍼티
    /// </summary>
    public float LifeTime
    {
        get => lifeTime;
        set 
        {
            lifeTime = value;                   // 우선 값 변경
            if(lifeTime < 0.0f && !isDead)      // 아직 안죽었는데 수명이 0 이하면
            {
                Die();                          // 사망 처리
                onLifeTimeChange?.Invoke(lifeTime, maxLifeTime);    // 수명이 변경되었다고 알림(죽은 직후에 한번만 실행)
            }
            else
            {
                lifeTime = Math.Clamp(value, 0.0f, maxLifeTime);    // 아니면 수명을 (0~최대값)으로 설정
                onLifeTimeChange?.Invoke(lifeTime, maxLifeTime);    // 수명이 변경되었다고 알림
            }
        }
    }

    /// <summary>
    /// 플레이어의 수명이 변경되었을 때 실행될 델리게이트
    /// 비네트, 슬라이더, 남은 시간 표시용으로 사용
    /// </summary>
    public Action<float, float> onLifeTimeChange;

    /// <summary>
    /// 플레이어가 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action onDie;


    // 이동 관련 변수들 ---------------------------------------------------------------------------
    
    /// <summary>
    /// 플레이어 이동 속도
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 현재 입력된 이동 방향
    /// </summary>
    Vector2 inputDir;

    /// <summary>
    /// 공격 이후에 복구 용도로 임시 저장해 놓은 이전 입력 방향
    /// </summary>
    Vector2 oldInputDir;

    /// <summary>
    /// 현재 이동 중인지 표시하는 변수
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// 맵 매니저
    /// </summary>
    MapManager mapManager;

    /// <summary>
    /// 현재 위치하고 있는 맵(의 그리드 좌표)
    /// </summary>
    Vector2Int currentMap;

    /// <summary>
    /// 현재 위치하고 있는 맵을 확인하고 변경할 수 있는 프로퍼티
    /// </summary>
    Vector2Int CurrentMap
    {
        get => currentMap;
        set
        {
            if (currentMap != value)            // 맵이 변경이 될 때만
            {
                currentMap = value;             // 실제로 변경
                onMapMoved?.Invoke(currentMap); // 델리게이트로 변경되었음을 알림
            }
        }
    }

    /// <summary>
    /// 맵 변경시 실행될 델리게이트(파라메터는 변경된 맵의 그리드 좌표)
    /// </summary>
    public Action<Vector2Int> onMapMoved;

    // 공격 관련 변수들 ---------------------------------------------------------------------------

    /// <summary>
    /// 플레이어의 공격 쿨타임
    /// </summary>
    public float attackCoolTime = 1.0f;

    /// <summary>
    /// 플레이어의 현재 남아있는 쿨타임
    /// </summary>
    float currentAttackCoolTime = 0.0f;

    /// <summary>
    /// 공격 영역의 중심(축)
    /// </summary>
    Transform attackAreaCenter;

    /// <summary>
    /// 플레이어가 공격하면 죽을 슬라임들
    /// </summary>
    List<Slime> attackTarget;

    /// <summary>
    /// 공격유효기간 표시. true면 슬라임을 죽일 수 있다. false면 못 죽이는 상황
    /// </summary>
    bool isAttackValid = false;

    // 기타 변수들 --------------------------------------------------------------------------------

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

    
    // 함수들 -------------------------------------------------------------------------------------

    private void Awake()
    {
        // 컴포넌트 찾고 
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        // 객체 생성
        inputActions = new PlayerInputActions();

        // 공격 대상 저장용 리스트 생성
        attackTarget = new List<Slime>(2);
        attackAreaCenter = transform.GetChild(0);   // 공격지점의 중심점 찾기
        AttackArea attackArea = attackAreaCenter.GetComponentInChildren<AttackArea>();  // 공격지점 찾기
        attackArea.onTarget += (slime) =>
        {
            // slime이 공격 지점안에 들어왔을 때의 처리
            attackTarget.Add(slime);    // 리스트에 추가
            slime.ShowOutline(true);    // 아웃라인 표시
        };
        attackArea.onTargetRelease += (slime) =>
        {
            // slime이 공격지점 밖으로 나갔을 때의 처리
            attackTarget.Remove(slime); // 리스트에서 제거
            slime.ShowOutline(false);   // 아웃라인 끄기
        };
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

    private void Start()
    {
        mapManager = GameManager.Inst.MapManager;   // 맵매니저 가져오기
        
        lifeTime = maxLifeTime;
    }

    private void Update()
    {
        totalPlayTime += Time.deltaTime;
        LifeTime -= Time.deltaTime;

        // 아무 조건 없이 계속 공격 쿨타임 감소
        currentAttackCoolTime -= Time.deltaTime;    

        if(isAttackValid)
        {
            while(attackTarget.Count > 0)   // 공격 대상이 있으면 다 없어질 때까지 처리
            {
                attackTarget[0].Die();      // Die가 실행되면 컬라이더가 비활성화 되면서 attackTarget에서 자동으로 제거됨
            }
        }
    }

    private void FixedUpdate()
    {
        // 이동 처리
        rigid.MovePosition(rigid.position + Time.deltaTime * speed * inputDir);

        // 이동 후에 어떤맵에 있는지 표시
        if(mapManager != null)
            CurrentMap = mapManager.WorldToGrid(transform.position); 
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // 이동 입력이 들어왔을 때
        inputDir = context.ReadValue<Vector2>();    // 입력 이동 방향 저장하고
        oldInputDir = inputDir;                     // 이후 복원을 위해 입력 이동 방향 저장
        anim.SetFloat("InputX", inputDir.x);        // 애니메이터 파라메터 변경
        anim.SetFloat("InputY", inputDir.y);

        isMove = true;                              // 이동한다고 표시하고 
        anim.SetBool("IsMove", isMove);             // 이동 애니메이션 재생

        // 입력 방향에 따라 공격지점 위치 변경(중심축 회전으로 처리)
        if(inputDir.y > 0)
        {
            // 위로 갈 때(위, 아래가 우선 순위가 높음)
            attackAreaCenter.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if(inputDir.y < 0)
        {
            // 아래로 갈 때
            attackAreaCenter.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (inputDir.x > 0)
        {
            // 오른쪽으로 갈 때
            attackAreaCenter.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (inputDir.x < 0)
        {
            // 왼쪽으로 갈 때
            attackAreaCenter.rotation = Quaternion.Euler(0, 0, 270);
        }
        else
        {
            // 있을 수 없음. 혹시나 싶어서 작성한 코드
            attackAreaCenter.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        isMove = false;                 // 이동 정지로 표시하고
        anim.SetBool("IsMove", isMove); // Idle 애니메이션 재생
        inputDir = Vector2.zero;        // 입력 이동 방향 제거
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        if (currentAttackCoolTime < 0)
        {
            oldInputDir = inputDir;         // 이후 복원을 위해 입력 이동 방향 저장
            inputDir = Vector2.zero;        // 입력 이동 방향 초기화
            anim.SetTrigger("Attack");      // 공격 애니메이션 실행

            currentAttackCoolTime = attackCoolTime; // 쿨타임 리셋
        }
    }

    /// <summary>
    /// 공격 애니메이션 state가 끝날 때 실행되는 함수
    /// </summary>
    public void RestoreInputDir()
    {
        if( isMove == true )            // 이동 중일 때만 
            inputDir = oldInputDir;     // 입력 이동 방향 복원
    }

    /// <summary>
    /// 공격이 효과적으로 보일 때 실행되는 함수
    /// </summary>
    public void AttackValid()
    {
        isAttackValid = true;
    }

    /// <summary>
    /// 공격이 효과적으로 보이는 기간이 끝날 때 실행될 함수
    /// </summary>
    public void AttackNotValid()
    {
        isAttackValid = false;
    }

    /// <summary>
    /// 타임오버로 사망시 실행될 함수
    /// </summary>
    void Die()
    {
        lifeTime = 0.0f;    // 비네트, 슬라이더, 남은 시간을 깔끔하게 표시하기 위해 0으로 설정
        isDead = true;      // 여러번 호출되지 않도록하기 위치 설정
        onDie?.Invoke();    // 죽었다고 알리기
    }
}
