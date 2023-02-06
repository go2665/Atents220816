using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_ShipDeployment_Random : TestBase
{
    public Button reset;
    public Button randomDeployment;

    Board board;

    Ship targetShip = null;
    Ship TargetShip
    {
        get => targetShip;
        set
        {
            if (targetShip != value)
            {
                if (targetShip != null)                     // 이전 targetShip이 있으면 그것에 대한 처리
                {
                    targetShip.SetMaterialType();           // 함선의 머티리얼 타입을 normal로 돌리기
                    targetShip.gameObject.SetActive(false); // 비활성화 시켜서 안보이게 만들기
                }

                targetShip = value;         // targetShip 변경

                if (targetShip != null)                     // 새 targetShip이 있으면 그것에 대한 처리
                {
                    targetShip.SetMaterialType(false);      // 함선의 머티리얼 타입을 deployMode로 변경

                    Vector2Int gridPos = board.GetMouseGridPosition();          // 마우스 커서의 그리드 좌표 계산해서
                    TargetShip.transform.position = board.GridToWorld(gridPos); // 해당위치에 함선 배치

                    targetShip.gameObject.SetActive(true);  // 활성화 시켜서 보이게 만들기
                }
            }
        }
    }

    Ship[] testShips = null;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        testShips = new Ship[ShipManager.Inst.ShipTypeCount];
        testShips[(int)ShipType.Carrier - 1] = ShipManager.Inst.MakeShip(ShipType.Carrier, this.transform);
        testShips[(int)ShipType.Battleship - 1] = ShipManager.Inst.MakeShip(ShipType.Battleship, this.transform);
        testShips[(int)ShipType.Destroyer - 1] = ShipManager.Inst.MakeShip(ShipType.Destroyer, this.transform);
        testShips[(int)ShipType.Submarine - 1] = ShipManager.Inst.MakeShip(ShipType.Submarine, this.transform);
        testShips[(int)ShipType.PatrolBoat - 1] = ShipManager.Inst.MakeShip(ShipType.PatrolBoat, this.transform);

        reset.onClick.AddListener(OnResetClick);
        randomDeployment.onClick.AddListener(OnRandomDeployment);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.TestClick.performed += OnTestClick;
        inputActions.Test.Test_RClick.performed += OnTestRClick;
        inputActions.Test.TestWheel.performed += OnTestWheel;
        inputActions.Test.Test_MouseMove.performed += OnTestMove;
    }

    protected override void OnDisable()
    {
        inputActions.Test.Test_MouseMove.performed -= OnTestMove;
        inputActions.Test.TestWheel.performed -= OnTestWheel;
        inputActions.Test.Test_RClick.performed -= OnTestRClick;
        inputActions.Test.TestClick.performed -= OnTestClick;
        base.OnDisable();
    }

    private void OnResetClick()
    {
        // 배치되어 있는 모든 함선을 배치 취소
    }

    private void OnRandomDeployment()
    {
        // 아직 배치되지 않은 모든 함선을 배치

        // 후보지 : 함선이 배치될 수 있는 칸들

        // 변수 생성
        // 우선순위가 높은 후보지
        // 우선순위가 낮은 후보지

        // 맵의 가장자리는 낮은 후보지에 추가
        // 그 외 지역은 높은 후보지에 추가

        // 각 후보지를 랜덤으로 섞기(후보지 별로 섞기)

        // 배치된 함선이 있으면 그 함선에 대한 후보지를 처리
        // 함선의 위치는 양 후보지에서 제거
        // 함선의 위치의 주변위치는 전부 낮은 후보지로 이동

        // 함선별 배치 작업 시작
        // 배를 랜덤으로 회전 시기기
        // 높은 후보지에서 위치 하나 꺼내서 그 위치에 함선 배치 시도
        // 배치에 실패하면 다시 높은 후보지에서 위치를 새로 꺼내 다시 시도
        // 일정 회수 이상 실패하면 낮은 후보지에서 선택 시도
        // 낮은 후보지에서는 될때까지 반복 시도
        // 배치 위치를 성공적으로 찾았다면 함선 배치 실행
    }

    private void OnTestClick(InputAction.CallbackContext _)
    {        
    }

    private void OnTestRClick(InputAction.CallbackContext _)
    {        
        // 마우스 위치에 있는 함선을 배치 취소
    }

    private void OnTestWheel(InputAction.CallbackContext context)
    {
    }

    private void OnTestMove(InputAction.CallbackContext context)
    {        
    }
}
