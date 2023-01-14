using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

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

    NetworkVariable<Vector3> networkMoveDelta = new NetworkVariable<Vector3>();
    NetworkVariable<float> networkRotateDelta = new NetworkVariable<float>();

    /// <summary>
    /// 이번 프레임에 움직여야 할 이동량
    /// </summary>
    //Vector3 moveDelta;

    /// <summary>
    /// 이번 프레임에 회전해야 할 회전량
    /// </summary>
    //float rotateDelta;

    // 컴포넌트들
    CharacterController contoller;
    PlayerInputActions inputActions;

    private void Awake()
    {
        contoller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
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

    private void ClientMoveAndRotate()
    {
        //contoller.SimpleMove(moveDelta);
        //transform.Rotate(0, rotateDelta * Time.deltaTime, 0, Space.World);

        if( networkMoveDelta.Value != Vector3.zero)
        {
            contoller.SimpleMove(networkMoveDelta.Value);
        }
        if (networkRotateDelta.Value != 0.0f)
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
        if (IsClient && IsOwner)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDelta = moveInput.y * moveSpeed * transform.forward;    // 이동 입력 저장하기
            float rotateDelta = moveInput.x * rotateSpeed;                      // 회전 입력 저장하기

            UpdateClientMoveAndRotateServerRpc(moveDelta, rotateDelta);
        }
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
}
