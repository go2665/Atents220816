using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
    /// <summary>
    /// 걷는 이동 속도
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// 달리는 이동 속도
    /// </summary>
    public float runSpeed = 5.0f;

    /// <summary>
    /// 현재 이동속도
    /// </summary>
    float currentSpeed = 3.0f;

    /// <summary>
    /// 이동 상태를 나타내는 enum
    /// </summary>
    enum MoveMode
    {
        Walk = 0,
        Run
    }

    /// <summary>
    /// 현재 이동 상태
    /// </summary>
    MoveMode moveMode = MoveMode.Walk;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float turnSpeed = 10.0f;

    /// <summary>
    /// 입력으로 지정된 바라보는 방향
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 최종 회전 목표 
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    /// <summary>
    /// 인풋 액션 인스턴스
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 애니메이터 컴포넌트 캐싱용
    /// </summary>
    Animator anim;

    /// <summary>
    /// 캐릭터 컨트롤러 컴포넌트 캐싱용
    /// </summary>
    CharacterController cc;

    /// <summary>
    /// 플레이어 스크립트 캐싱용
    /// </summary>
    Player player;

    

    private void Awake()
    {
        // 컴포넌트 만들어졌을 때 인풋 액션 인스턴스 생성
        inputActions = new PlayerInputActions();

        // 컴포넌트 찾아오기
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        InventoryUI invenUI = GameManager.Inst.InvenUI;
        invenUI.onInventoryOpen += () => inputActions.Player.Disable();
        invenUI.onInventoryClose += () => inputActions.Player.Enable();
    }

    private void OnEnable()
    {
        // 인풋 액션에서 액션맵 활성화
        inputActions.Player.Enable();
        // 액션과 함수 연결
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChange;
        inputActions.Player.Attack.performed += OnAttack;
        inputActions.Player.Pickup.performed += OnPickup;
        inputActions.Player.LockOn.performed += OnLockOn;
    }

    private void OnDisable()
    {
        // 액션과 함수 연결 해제
        inputActions.Player.LockOn.performed -= OnLockOn;
        inputActions.Player.Pickup.performed -= OnPickup;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        // 액션맵 비활성화
        inputActions.Player.Disable();
    }

    private void Update()
    {
        if (player.IsAlive) // 살아있을 때에만 업데이트
        {
            // inputDir방향으로 초당 moveSpeed의 속도로 이동.
            cc.Move(currentSpeed * Time.deltaTime * inputDir);

            // 락온한 대상이 있으면 락온한 대상의 위치를 바라보게 만들기
            if(player.LockOnTransform != null)
            {
                targetRotation = Quaternion.LookRotation(player.LockOnTransform.position - player.transform.position);
            }

            // transform.rotation에서 targetRotation으로 초당 1/turnSpeed씩 보간.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Player액션맵의 Move액션이 performed되거나 canceled될 때 실행
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        // WASD 입력을 받아옴(+x:D, -x:A, +y:W, -y:s)
        Vector2 input = context.ReadValue<Vector2>();   
        //Debug.Log(input);
        inputDir.x = input.x;   // 입력받은 것을 3D xz 평면상의 방향으로 변경
        inputDir.y = 0.0f;
        inputDir.z = input.y;
                
        if (!context.canceled && player.IsAlive)    // 살아있을 때에만 입력 받기
        {
            // 입력이 들어왔을 때만 실행되는 코드

            Quaternion cameraYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); // 카메라의 y축 회전만 분리
            // 카메라의 y축 회전을 inputDir에 곱한다. => inputDir과 카메라가 xz평면상에서 바라보는 방향을 일치시킴
            inputDir = cameraYRotation * inputDir;  

            targetRotation = Quaternion.LookRotation(inputDir); // inputDir 방향으로 바라보는 회전 만들기

            if (moveMode == MoveMode.Walk)
            {
                anim.SetFloat("Speed", 0.3f);   // Walk모드면 걷는 애니메이션
            }
            else
            {
                anim.SetFloat("Speed", 1.0f);   // Run모드면 달리는 애니메이션
            }

            inputDir.y = -2.0f;     // 강제로 바닥에 내려가도록 처리
        }
        else
        {
            inputDir = Vector3.zero;
            anim.SetFloat("Speed", 0.0f);       // 입력이 안들어 왔으면 대기 애니메이션. 플레이어가 죽었을 때에도 실행
        }

    }

    /// <summary>
    /// 쉬프트 키를 눌렀을 때 실행
    /// </summary>
    /// <param name="_"></param>
    private void OnMoveModeChange(InputAction.CallbackContext _)
    {
        if( moveMode == MoveMode.Walk )
        {
            // Walk모드면 Run모드로 전환
            moveMode = MoveMode.Run;
            currentSpeed = runSpeed;            // 이동 속도도 달리는 속도로 변경
            if (inputDir != Vector3.zero)
            {
                anim.SetFloat("Speed", 1.0f);   // 움직이는 중일 때만 재생하는 애니메이션 변경
            }
        }
        else
        {
            // Run모드면 Walk모드로 전환
            moveMode = MoveMode.Walk;
            currentSpeed = walkSpeed;           // 이동 속도를 걷는 속도로 변경
            if (inputDir != Vector3.zero)
            {
                anim.SetFloat("Speed", 0.3f);   // 움직이는 중일 때만 재생하는 애니메이션 변경
            }
        }
    }

    /// <summary>
    /// 스페이스 키나 마우스 왼클릭 했을 때 실행
    /// </summary>
    /// <param name="_"></param>
    private void OnAttack(InputAction.CallbackContext _)
    {
        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime); // 현재 재생중인 애니메이션의 진행 상태를 알려줌(0~1)

        if (player.IsAlive)
        {
            int comboState = anim.GetInteger("ComboState"); // ComboState를 애니메이터에서 읽어와서 
            comboState++;   // 콤보 상태 1 증가 시키기        
            anim.SetInteger("ComboState", comboState);      // 애니메이터에 증가된 콤보 상태 설정
            anim.SetTrigger("Attack");                      // Attack 트리거 발동
        }
    }


    /// <summary>
    /// 아이템 획득 버튼을 눌렀을 때 실행
    /// </summary>
    /// <param name="_"></param>
    private void OnPickup(InputAction.CallbackContext _)
    {
        player.ItemPickup();
    }


    /// <summary>
    /// 몬스터 락온 버튼을 눌렀을 때 실행
    /// </summary>
    /// <param name="context"></param>
    private void OnLockOn(InputAction.CallbackContext context)
    {
        player.LockOnToggle();
    }
}
