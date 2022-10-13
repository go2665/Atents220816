using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 이동속도
    /// </summary>
    public float moveSpeed = 3.0f;

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

    private void Awake()
    {
        // 컴포넌트 만들어졌을 때 인풋 액션 인스턴스 생성
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // 인풋 액션에서 액션맵 활성화
        inputActions.Player.Enable();
        // 액션과 함수 연결
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        // 액션과 함수 연결 해제
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        // 액션맵 비활성화
        inputActions.Player.Disable();
    }

    private void Update()
    {
        // inputDir방향으로 초당 moveSpeed의 속도로 이동. 월드 스페이스 기준으로 이동
        transform.Translate(moveSpeed * Time.deltaTime * inputDir, Space.World);

        // transform.rotation에서 targetRotation으로 초당 1/turnSpeed씩 보간.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
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
                
        if (!context.canceled)
        {
            // 입력이 들어왔을 때만 실행되는 코드

            Quaternion cameraYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); // 카메라의 y축 회전만 분리
            // 카메라의 y축 회전을 inputDir에 곱한다. => inputDir과 카메라가 xz평면상에서 바라보는 방향을 일치시킴
            inputDir = cameraYRotation * inputDir;  

            targetRotation = Quaternion.LookRotation(inputDir); // inputDir 방향으로 바라보는 회전 만들기
        }
    }
}
