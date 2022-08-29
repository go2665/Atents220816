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

    public GameObject bullet;
    public float speed = 1.0f;      // 플레이어의 이동 속도(초당 이동 속도)
    public float fireInterval = 0.5f;

    Vector3 dir;                    // 이동 방향(입력에 따라 변경됨)
    float boost = 1.0f;

    //bool isFiring = false;
    //float fireTimeCount = 0.0f;

    IEnumerator fireCoroutine;

    Rigidbody2D rigid;
    Animator anim;

    PlayerInputAction inputActions;
    // Awake -> OnEnable -> Start : 대체적으로 이 순서

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 생성된 직후에 호출
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerInputAction();
        rigid = GetComponent<Rigidbody2D>();    // 한번만 찾고 저장해서 계속 쓰기(메모리 더 쓰고 성능 아끼기)
        anim = GetComponent<Animator>();
        fireCoroutine = Fire();
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
        inputActions.Player.Boost.canceled -= OnBoostOff;
        inputActions.Player.Boost.performed -= OnBoostOn;
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Move.canceled -= OnMove;    // 연결해 놓은 함수 해제(안전을 위해)
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();  // 오브젝트가 사라질때 더 이상 입력을 받지 않도록 비활성화
    }

    /// <summary>
    /// 시작할 때. 첫번째 Update 함수가 실행되기 직전에 호출.
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// 매 프레임마다 호출.
    /// </summary>
    //private void Update()
    //{
    //    //transform.position += (speed * Time.deltaTime * dir);
    //    //transform.Translate(speed * Time.deltaTime * dir);
    //    //transform.Translate(speed * Time.deltaTime * dir.x, speed * Time.deltaTime * dir.y, 0);

    //    //transform.position = dir;
    //}

    /// <summary>
    /// 일정 시간 간격(물리 업데이트 시간 간격)으로 호출
    /// </summary>
    private void FixedUpdate()
    {
        //transform.Translate(speed * Time.fixedDeltaTime * dir);

        // 이 스크립트 파일이 들어 있는 게임 오브젝트에서 Rigidbody2D 컴포넌트를 찾아 리턴.(없으면 null)
        // 그런데 GetComponent는 무거운 함수 => (Update나 FixedUpdate처럼 주기적 또는 자주 호출되는 함수 안에서는 안쓰는 것이 좋다)
        // Rigidbody2D rigid = GetComponent<Rigidbody2D>();    

        // rigid.AddForce(speed * Time.fixedDeltaTime * dir); // 관성이 있는 움직임을 할 때 유용
        rigid.MovePosition(transform.position + boost * speed * Time.fixedDeltaTime * dir); // 관성이 없는 움직임을 처리할 때 유용

        //fireTimeCount += Time.fixedDeltaTime;
        //if ( isFiring && fireTimeCount > fireInterval )
        //{
        //    Instantiate(bullet, transform.position, Quaternion.identity);
        //    fireTimeCount = 0.0f;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D");    // Collider와 부딪쳤을 때 실행
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("OnCollisionExit2D");     // Collider와 접촉이 떨어지는 순간 실행
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D");      // 트리거에 들어갔을 때 실행
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("OnTriggerExit2D");       // 트리거에서 나갔을 때 실행
    }

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

    private void OnFireStart(InputAction.CallbackContext _)
    {
        //Debug.Log("발사!");
        //float value = Random.Range(0.0f, 10.0f);  // value에는 0.0 ~ 10.0의 랜덤값이 들어간다.
        //isFiring = true;
        StartCoroutine(fireCoroutine);
        
    }

    private void OnFireStop(InputAction.CallbackContext _)
    {
        //isFiring = false;
        //StopAllCoroutines();
        StopCoroutine(fireCoroutine);
    }

    IEnumerator Fire()
    {
        //yield return null;      // 다음 프레임에 이어서 시작해라
        //yield return new WaitForSeconds(1.0f);  // 1초 후에 이어서 시작해라

        while (true)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void OnBoostOn(InputAction.CallbackContext context)
    {
        boost *= 2.0f;
    }

    private void OnBoostOff(InputAction.CallbackContext context)
    {
        boost = 1.0f;
    }


}
