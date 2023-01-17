using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System;
using Unity.Collections.LowLevel.Unsafe;

public class NetPlayer : NetworkBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 3.5f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 350f;

    // NetworkVariable
    // Netcode for GameObjects에서 네트웨크를 통해 데이터를 공유하기 위해 사용하는 데이터 타입.
    // NetworkVariable로 공유 가능한 데이터 타입은 unmanaged 타입 가능(대략적으로 값타입만 가능)
    // 생성자를 통해 읽기/쓰기 권한을 설정할 수 있다.

    /// <summary>
    /// 네트워크로 적용될 이동량
    /// </summary>
    NetworkVariable<Vector3> networkMoveDelta = new NetworkVariable<Vector3>();

    /// <summary>
    /// 네트워크로 적용될 회전량
    /// </summary>
    NetworkVariable<float> networkRotateDelta = new NetworkVariable<float>();

    /// <summary>
    /// 플레이어의 애니메이션 상태를 나타내는 enum
    /// </summary>
    public enum PlayerAnimState
    {
        Idle,       // 가만히 있는 상태
        Walk,       // 걷는 상태
        BackWalk    // 뒤로 걷는 상태
    }

    /// <summary>
    /// 현재 컴퓨터(로컬)의 애니메이션 상태
    /// </summary>
    PlayerAnimState animState = PlayerAnimState.Idle;

    /// <summary>
    /// 네트워크로 공유되는 애니메이션 상태
    /// </summary>
    NetworkVariable<PlayerAnimState> networkAnimState = new NetworkVariable<PlayerAnimState>();

    // 컴포넌트들
    CharacterController contoller;
    Animator anim;
    PlayerInputActions inputActions;

    private void Awake()
    {
        contoller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        inputActions = new PlayerInputActions();

        networkAnimState.OnValueChanged += OnAnimStateChange;   // networkAnimState가 변경될 때 OnAnimStateChange를 실행 시키도록 함수 등록
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        ClientMoveAndRotate();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)    // 내 NetPlayer가 스폰이 되었을 때만 
        {
            GameManager.Inst.VCamera.Follow = transform.GetChild(0);    // 카메라가 이 NetPlayer를 따라다니게 만들기            
        }
    }

    private void ClientMoveAndRotate()
    {
        //contoller.SimpleMove(moveDelta);
        //transform.Rotate(0, rotateDelta * Time.deltaTime, 0, Space.World);

        if( networkMoveDelta.Value != Vector3.zero)     // networkMoveDelta가 값이 변경되었을 때만 처리
        {
            contoller.SimpleMove(networkMoveDelta.Value);
        }
        if (networkRotateDelta.Value != 0.0f)           // networkRotateDelta가 값이 변경되었을 때만 처리
        {
            transform.Rotate(0, networkRotateDelta.Value * Time.deltaTime, 0, Space.World);
        }
    }

    /// <summary>
    /// 입력을 하거나 취소했을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (IsClient && IsOwner)    // 클라이언트이고 오너일 때만 처리(각 개별로만 처리)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            SetInputDir(moveInput);

            // 애니메이션 관련 처리
            if(moveInput.y > 0)
            {
                animState = PlayerAnimState.Walk;       // 앞으로 갈 때
            }
            else if(moveInput.y < 0)
            {
                animState = PlayerAnimState.BackWalk;   // 뒤로 갈 때 
            }
            else
            {
                animState = PlayerAnimState.Idle;       // 안움직일 때
            }
            if( animState != networkAnimState.Value)    // 네트워크로 공유되는 값과 현재 로컬 값이 서로 다를 때만 변경 요청
            {
                UpdateNetPlayerAnimStateServerRpc(animState);   // 서버에게 networkAnimState를 animState 값으로 바꾸도록 요청
            }
        }
    }

    //public void SetInputDir(ref Vector2 dir)
    public void SetInputDir(Vector2 dir)
    {
        Vector3 moveDelta = dir.y * moveSpeed * transform.forward;      // 이동 입력 저장하기
        float rotateDelta = dir.x * rotateSpeed;                        // 회전 입력 저장하기

        UpdateClientMoveAndRotateServerRpc(moveDelta, rotateDelta);     // 저장한 내용을 바탕으로 서버에 변경 요청
    }

    /// <summary>
    /// networkAnimState가 변경되었을 때 실행되는 함수
    /// </summary>
    /// <param name="previousValue">이전값</param>
    /// <param name="newValue">현재값</param>
    private void OnAnimStateChange(PlayerAnimState previousValue, PlayerAnimState newValue)
    {
        anim.SetTrigger($"{newValue}"); // enum 값으로 토대로 트리거 실행
    }

    // [ServerRpc] : 서버가 실행하는 함수라는 표시
    [ServerRpc]
    public void UpdateClientMoveAndRotateServerRpc(Vector3 moveDelta, float rotateDelta)
    {
        // 서버가 실행하는 코드
        // 클라이언트에서 이 함수를 호출해도 함수 내부의 코드는 실행하지 않고 서버쪽에서 실행한다.
        // NetworkVariable의 기본 설정이 서버만 쓰기 권한을 가지고 있기 때문이다.
        networkMoveDelta.Value = moveDelta;
        networkRotateDelta.Value = rotateDelta;
    }

    [ServerRpc]
    void UpdateNetPlayerAnimStateServerRpc(PlayerAnimState playerAnimState)
    {
        // 값 변경하기
        networkAnimState.Value = playerAnimState;        
    }
}
