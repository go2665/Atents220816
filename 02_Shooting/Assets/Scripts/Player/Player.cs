//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //public delegate void DelegateName();    // 이런 종류의 델리게이트가 있다 (리턴없고 파라메터도 없는 함수를 저장하는 델리게이트)
    //public DelegateName del;      // DelegateName 타입으로 del이라는 이름의 델리게이트를 만듬
    //Action del2;                  // 리턴타입이 void, 파라메터도 없는 델리게이트 del2를 만듬
    //Action<int> del3;             // 리턴타입이 void, 파라메터는 int 하나인 델리게이트 del3을 만듬
    //Func<int, float> del4;        // 리턴타입이 int고 파라메터는 float 하나인 델리게이트 del4를 만듬
    
    // public 변수(필드) ------------------------------------------------------------------------------
    [Header("플레이어 스텟")]
    /// <summary>
    /// 플레이어의 이동 속도(초당 이동 속도)
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// 총알 발사 시간 간격
    /// </summary>
    public float fireInterval = 0.5f;

    [Header("게임 기본 설정")]
    /// <summary>
    /// 초기 생명 개수
    /// </summary>
    public int initialLife = 3;

    /// <summary>
    /// 피격시 무적 시간
    /// </summary>
    public float invincibleTime = 1.0f;

    [Header("각종 프리팹")]
    /// <summary>
    /// 총알용 프리팹
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// 비행기 폭팔 이팩트용 프리팹
    /// </summary>
    public GameObject explosionPrefab;

    // private 변수(필드) -----------------------------------------------------------------------------

    // 생존 관련 ------------------------------------------------------------------
    /// <summary>
    /// 현재 생명수
    /// </summary>
    private int life;

    /// <summary>
    /// 플레이어의 사망여부(true면 사망, false면 살아있음)
    /// </summary>
    private bool isDead = false;

    // 피격 관련 ------------------------------------------------------------------
    /// <summary>
    /// 무적 상태인지 표시용(true면 무적상태, false 일반상태)
    /// </summary>
    private bool isInvincibleMode = false;

    /// <summary>
    /// 무적상태에 들어간 후의 경과 시간(의 30배). 일종의 타이머
    /// </summary>
    private float timeElapsed = 0.0f;

    // 이동 관련 ------------------------------------------------------------------
    /// <summary>
    /// 입력된 이동 방향
    /// </summary>
    private Vector3 dir;

    /// <summary>
    /// 부스트 속도(부스트 상태에 들어가면 2, 보통 상태일 때는 1)
    /// </summary>
    private float boost = 1.0f;

    /// <summary>
    /// InputSystem용 입력 액션맵
    /// </summary>
    private PlayerInputAction inputActions;

    // 공격 관련 ------------------------------------------------------------------
    /// <summary>
    /// 총알이 발사될 위치와 회전을 가지고 있는 트랜스폼
    /// </summary>
    private Transform firePositionRoot;

    /// <summary>
    /// 총알이 발사될 때 보일 플래시 이팩트 게임 오브젝트
    /// </summary>
    private GameObject flash;

    /// <summary>
    /// 총알이 한번에 어려발 발사될 때 총알간의 사이 각도
    /// </summary>
    private float fireAngle = 30.0f;

    /// <summary>
    /// 파워업 아이템을 획득한 갯수(최대값은 3)
    /// </summary>
    private int power = 0;

    /// <summary>
    /// 플레이어가 획득한 점수
    /// </summary>
    public int totalScore = 0;

    private int extraPowerBonus = 1000;

    /// <summary>
    /// 총알 연사용 코루틴
    /// </summary>
    private IEnumerator fireCoroutine;    

    // 컴포넌트들 --------------------------------------------------------------------------------------
    private Rigidbody2D rigid;
    private Animator anim;
    private Collider2D bodyCollider;
    private SpriteRenderer spriteRenderer;
    private AudioSource shootAudio;

    // 델리게이트 --------------------------------------------------------------------------------------
    public Action<int> onLifeChange;
    public Action<int> onScoreChange;

    // 프로퍼티 ---------------------------------------------------------------------------------------
    /// <summary>
    /// 생명갯수 용 프로퍼티. 0~3 사이의 값을 가진다.
    /// </summary>
    private int Life
    {
        //get
        //{
        //    return life;
        //}
        get => life;    // 위의 4줄과 같은 코드
        set
        {
            // value는 지금 set하는 값
            if (life != value && !isDead)  // 값에 변경이 일어났다. 그리고 살아있다.
            {
                if (life > value)
                {
                    // life가 감소한 상황( 새로운 값(value)이 옛날 값(life)보다 작다 => 감소했다 )

                    Power--;
                    StartCoroutine(EnterInvincibleMode());
                }

                life = value;
                if (life <= 0)  // 비교범위는 가능한 크게 잡는 쪽이 안전하다.
                {
                    life = 0;
                    Dead();     // life 0보다 작거나 같으면 죽는다.
                }

                // (변수명)?. : 왼쪽 변수가 null이면 null. null이 아니면 (변수명) 맴버에 접근
                onLifeChange?.Invoke(life);  // 라이프가 변경될 때 onLifeChange 델리게이트에 등록된 함수들을 실행시킨다.
            }
        }
        //int i = Life;   // i에다가 Life의 값을 가져와서 넣어라 => Life의 get이 실행된다. i = life; 와 같은 실행 결과가 된다.
        //Life = 3;       // Life에 3을 넣어라 => Life의 set이 실행된다. life = 3;과 같은 실행결과        
    }

    /// <summary>
    /// 공격력 용 프로퍼티. 1~3 사이의 값을 가진다. 한번에 발사하는 총알의 숫자와 같다.
    /// </summary>
    private int Power
    {
        get => power;
        set
        {
            power = value;  // 들어온 값으로 파워 설정
            if (power > 3)  // 파워가 3을 벗어나면 3을 제한
                AddScore(extraPowerBonus);
            //if( power < 1)
            //    power = 1;
            power = Mathf.Clamp(power, 1, 3);

            // 기존에 있는 파이어 포지션 제거
            while (firePositionRoot.childCount > 0)
            {
                Transform temp = firePositionRoot.GetChild(0);  // firePositionRoot의 첫번째 자식을
                temp.parent = null;         // 부모 제거하고
                Destroy(temp.gameObject);   // 삭제 시키기
            }

            // 파워 등급에 맞기 새로 배치
            for (int i = 0; i < power; i++)
            {
                GameObject firePos = new GameObject();  // 빈 오브젝트 생성하기
                firePos.name = $"FirePosition_{i}";
                firePos.transform.parent = firePositionRoot;        // firePositionRoot의 자식으로 추가
                firePos.transform.localPosition = Vector3.zero;     // 로컬 위치를 (0,0,0)으로 변경. 아래줄과 같은 기능
                //firePos.transform.position = firePositionRoot.transform.position;

                // 파워가 1 일때  : 0도
                // 파워가 2 일때  : -15도, +15도
                // 파워가 3 일때  : -30도, 0도, +30도
                firePos.transform.rotation = Quaternion.Euler(0, 0, (power - 1) * (fireAngle * 0.5f) + i * -fireAngle);
                firePos.transform.Translate(1.0f, 0, 0);

            }
        }
    }


    // 함수(메서드) ------------------------------------------------------------------------------------    
    public void AddScore(int score)
    {
        totalScore += score;

        onScoreChange?.Invoke(totalScore);

        // 1. 이벤트가 발생하는 곳(델리게이트 작성) -> 신호만 보내기
        // 2. 실제 액션이 일어나는 곳(델리게이트에 함수 등록)
    }

    /// <summary>
    /// 플레이어가 죽었을 때 실행될 일들
    /// </summary>
    private void Dead()
    {
        isDead = true;  // 죽었다고 표시
        bodyCollider.enabled = false;   // 더 이상 충돌 안일어나게 만들기
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어도 플레이어로 원상복구
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);  // 폭팔 이팩트 생성
        InputDisable();                 // 입력 막고
        rigid.gravityScale = 1.0f;      // 중력으로 떨어지게 만들기
        rigid.freezeRotation = false;   // 회전 막아놓은 것 풀기
        StopCoroutine(fireCoroutine);   // 총을 쏘던 중이면 더이상 쏘지 않게 처리
    }

    /// <summary>
    /// 입력 막기. 액션맵을 비활성화하고 입력 이벤트에 연결된 함수들 제거
    /// </summary>
    private void InputDisable()
    {
        inputActions.Player.Boost.canceled -= OnBoostOff;
        inputActions.Player.Boost.performed -= OnBoostOn;
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Move.canceled -= OnMove;    // 연결해 놓은 함수 해제(안전을 위해)
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();  // 오브젝트가 사라질때 더 이상 입력을 받지 않도록 비활성화
    }

    // 코루틴용 함수 ------------------------------------------------------------------------------------

    /// <summary>
    /// 충돌 막고 무적 모드 설정, 타이머 초기화를 진행한 후 invincibleTime초 후에 다시 원상 복구
    /// </summary>
    /// <returns></returns>
    IEnumerator EnterInvincibleMode()
    {
        //bodyCollider.enabled = false;       // 충돌이 안일어나게 만들기
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        isInvincibleMode = true;            // 무적모드 켜기
        timeElapsed = 0.0f;                 // 타이머 초기화

        yield return new WaitForSeconds(invincibleTime);    // 무적시간 동안 대기

        spriteRenderer.color = Color.white; // 원래 색으로 되돌리기
        isInvincibleMode = false;           // 무적모드 끄기        
        gameObject.layer = LayerMask.NameToLayer("Player");
        //bodyCollider.enabled = !isDead;     // 살아있을 때만 충돌이 다시 발생하게 만들기.
    }

    /// <summary>
    /// 총알 연사용 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Fire()
    {
        //yield return null;      // 다음 프레임에 이어서 시작해라
        //yield return new WaitForSeconds(1.0f);  // 1초 후에 이어서 시작해라

        while (true)    // 무한하게 작업 반복
        {
            for (int i = 0; i < firePositionRoot.childCount; i++)
            {
                // bullet이라는 프리팹을 firePosition[i]의 위치에 firePosition[i]의 회전으로 만들어라
                Instantiate(bulletPrefab, firePositionRoot.GetChild(i).position, firePositionRoot.GetChild(i).rotation);

                // Instantiate(생성할 프리팹);    // 프리팹이 (0,0,0)위치에 (0,0,0)회전에 (1,1,1)스케일로 만들어짐 
                // Instantiate(생성할 프리팹, 생성할 위치, 생성될 때의 회전)
            }
            shootAudio.Play();
            flash.SetActive(true);      // flash 켜고
            StartCoroutine(FlashOff()); // 0.1초 후에 flash를 끄는 코루틴 실행

            yield return new WaitForSeconds(fireInterval);  // 총알 발사 시간 간격만큼 대기
        }
    }

    /// <summary>
    /// 0.1초 후에 flash를 끄는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashOff()
    {
        yield return new WaitForSeconds(0.1f);  // 0.1초 대기
        flash.SetActive(false); // flash 끄기
    }

    // 입력 처리용 함수 ----------------------------------------------------------------------------------

    /// <summary>
    /// 이동 입력 처리용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        // Exception : 예외 상황( 무엇을 해야 할지 지정이 안되어있는 예외 일때 )
        //throw new NotImplementedException();    // NotImplementedException 을 실행해라. => 코드 구현을 알려주기 위해 강제로 죽이는 코드

        //Debug.Log("이동 입력");
        dir = context.ReadValue<Vector2>();    // 어느 방향으로 움직여야 하는지를 입력받음

        //dir.y > 0   // W를 눌렀다.
        //dir.y == 0  // W,S 중 아무것도 안눌렀다.
        //dir.y < 0   // S를 눌렀다.
        anim.SetFloat("InputY", dir.y);
    }

    /// <summary>
    /// 총알 발사 시작 입력 처리용(Space를 눌렀을 때)
    /// </summary>
    /// <param name="_"></param>
    private void OnFireStart(InputAction.CallbackContext _)
    {
        //Debug.Log("발사!");
        StartCoroutine(fireCoroutine);  // 코루틴 실행
    }

    /// <summary>
    /// 총알 발사 중지 입력 처리용(Space를 땠을 때)
    /// </summary>
    /// <param name="_"></param>
    private void OnFireStop(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine);   // 코루틴 정지
    }

    /// <summary>
    /// 이동 부스트 발동용 입력 처리 함수(Shift눌렀을 때)
    /// </summary>
    /// <param name="context"></param>
    private void OnBoostOn(InputAction.CallbackContext context)
    {
        boost *= 2.0f;  // 이동 속도 계산에 들어가는 계수를 2로 변경
    }

    /// <summary>
    /// 이동 부스트 발동 해제용 입력처리 함수(Shift 땠을 때)
    /// </summary>
    /// <param name="context"></param>
    private void OnBoostOff(InputAction.CallbackContext context)
    {
        boost = 1.0f;   // 이동 속도 계산에 들어가는 계수를 1로 복귀
    }

    // 유니티 이벤트 함수들 --------------------------------------------------------------------------------
    // Awake -> OnEnable -> Start : 대체적으로 이 순서

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 생성된 직후에 호출
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerInputAction();     // 액션맵 인스턴스 생성

        // 컴포넌트 한번만 찾고 저장해서 계속 쓰기(메모리 더 쓰고 성능 아끼기)
        // GetComponent는 무거운 함수
        // => (Update나 FixedUpdate처럼 주기적 또는 자주 호출되는 함수 안에서는 안쓰는 것이 좋다)
        // => Awake나 Start에서 한번만 하는 것이 좋다.
        rigid = GetComponent<Rigidbody2D>();    
        anim = GetComponent<Animator>();
        bodyCollider = GetComponent<Collider2D>();  // CapsuleCollider2D가 Collider2D의 자식이라서 가능
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootAudio = GetComponent<AudioSource>();

        firePositionRoot = transform.GetChild(0);   // 발사 트랜스폼 찾기
        flash = transform.GetChild(1).gameObject;   // flash 가져오기
        flash.SetActive(false);                     // flash 비활성화

        fireCoroutine = Fire(); // 연사용 코루틴 저장

        
    }

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 활성화 되었을때 호출
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();   // 오브젝트가 생성되면 입력을 받도록 활성화
        inputActions.Player.Move.performed += OnMove;   // Move액션이 performed 일 때 OnMove 함수 실행하도록 연결
        inputActions.Player.Move.canceled += OnMove;    // Move액션이 canceled 일 때 OnMove 함수 실행하도록 연결
        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireStop;
        inputActions.Player.Boost.performed += OnBoostOn;
        inputActions.Player.Boost.canceled += OnBoostOff;
    }

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 비활성화 되었을 때 호출
    /// </summary>
    private void OnDisable()
    {
        InputDisable(); // 입력도 비활성화
    }

    /// <summary>
    /// 시작할 때. 첫번째 Update 함수가 실행되기 직전에 호출.
    /// </summary>
    private void Start()
    {
        Power = 1;          // 시작할 때 파워를 1로 설정(발사 위치 갱신용)
        Life = initialLife; // 생명숫자도 초기화
        totalScore = 0;     // 점수 초기화
        AddScore(0);        // UI 갱신용
    }

    /// <summary>
    /// 매 프레임마다 호출.
    /// </summary>
    private void Update()
    {
        if( isInvincibleMode )  // 무적 상태용 코드
        {
            timeElapsed += Time.deltaTime * 30.0f;                  // 시간의 30배 누적
            float alpha = (Mathf.Cos(timeElapsed) + 1.0f) * 0.5f;   // cos의 결과를 1~0으로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha);       // 알파값 변경
        }
    }

    /// <summary>
    /// 일정 시간 간격(물리 업데이트 시간 간격)으로 호출
    /// </summary>
    private void FixedUpdate()
    {
        if (!isDead)
        {
            // rigid.AddForce(boost * speed * Time.fixedDeltaTime * dir); // 관성이 있는 움직임을 할 때 유용
            
            // 관성이 없는 움직임을 처리할 때 유용
            rigid.MovePosition(transform.position + boost * speed * Time.fixedDeltaTime * dir); 
        }
        else
        {
            // 죽었을 때의 연출용. 뒤로 돌면서 튕겨나가기
            rigid.AddForce(Vector2.left * 0.1f, ForceMode2D.Impulse);   
            rigid.AddTorque(10.0f);
        }
    }

    /// <summary>
    /// 충돌이 발생했을 때 실행.(충돌한 순간)
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.CompareTag("PowerUp") )
        {
            // 파워업 아이템을 먹었으면
            Power++;                        // 파워 증가 시키고
            Destroy(collision.gameObject);  // 파워업 아이템 삭제
        }

        if( collision.gameObject.CompareTag("Enemy") )
        {
            // 적이랑 부딪치면 life가 1 감소한다.
            Life--;
            
            //Debug.Log($"플레이어의 Life는 {life}");
        }
    }
}
