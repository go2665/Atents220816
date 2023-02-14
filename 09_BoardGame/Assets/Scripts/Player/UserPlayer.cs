using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserPlayer : PlayerBase
{
    /// <summary>
    /// 지금 배치하려고 하는 함선
    /// </summary>
    Ship selectedShip = null;

    /// <summary>
    /// 지금 배치하려고 하는 함선을 확인하고 설정할 수 있는 프로퍼티
    /// </summary>
    Ship SelectedShip
    {
        get => selectedShip;
        set
        {
            if (selectedShip != value)
            {
                if (selectedShip != null)                     // 이전 targetShip이 있으면 그것에 대한 처리
                {
                    selectedShip.SetMaterialType();           // 함선의 머티리얼 타입을 normal로 돌리기
                    selectedShip.gameObject.SetActive(false); // 비활성화 시켜서 안보이게 만들기
                }

                selectedShip = value;         // targetShip 변경

                if (selectedShip != null)                     // 새 targetShip이 있으면 그것에 대한 처리
                {
                    selectedShip.SetMaterialType(false);      // 함선의 머티리얼 타입을 deployMode로 변경

                    Vector2Int gridPos = board.GetMouseGridPosition();          // 마우스 커서의 그리드 좌표 계산해서
                    SelectedShip.transform.position = board.GridToWorld(gridPos); // 해당위치에 함선 배치
                    selectedShip.Direction = ShipDirection.West;

                    selectedShip.gameObject.SetActive(true);  // 활성화 시켜서 보이게 만들기
                }
            }
        }
    }

    // 입력 관련 델리게이트 --------------------------------------------------------------------------------

    /// <summary>
    /// 클릭되었을 때 실행될 델리게이트들(게임 상태별로 각각 가지고 있음, null이면 해당 상태에서 입력 받는 일이 없다는 것)
    /// </summary>
    Action<Vector2>[] onClick;

    /// <summary>
    /// 마우스가 움직였을 때 실행될 델리게이트들(게임 상태별로 각각 가지고 있음, null이면 해당 상태에서 입력 받는 일이 없다는 것)
    /// </summary>
    Action<Vector2>[] onMouseMove;

    /// <summary>
    /// 마우스 휠이 돌아갈 때 실행될 델리게이트들(게임 상태별로 각각 가지고 있음, null이면 해당 상태에서 입력 받는 일이 없다는 것)
    /// </summary>
    Action<float>[] onMouseWheel;

    protected override void Awake()
    {
        base.Awake();

        int length = Enum.GetValues(typeof(GameState)).Length;  // 상태 갯수 받아오기
        onClick = new Action<Vector2>[length];                  // 배열 크기 잡기
        onMouseMove = new Action<Vector2>[length];
        onMouseWheel = new Action<float>[length];

        onClick[(int)GameState.ShipDeployment] = OnClick_ShipDeployment;            // 함수들 등록
        onClick[(int)GameState.Battle] = OnClick_Battle;
        onMouseMove[(int)GameState.ShipDeployment] = OnMouseMove_ShipDeployment;
        onMouseWheel[(int)GameState.ShipDeployment] = OnMouseWheel_ShipDeployment;
    }

    protected override void Start()
    {
        base.Start();
        opponent = GameManager.Inst.EnemyPlayer; // 상대방 설정

        GameManager.Inst.Input.onClick += OnClick;              // 인풋 컨트롤러 쪽에 입력시 실행될 함수 등록
        GameManager.Inst.Input.onMouseMove += OnMouseMove;
        GameManager.Inst.Input.onMouseWheel += OnMouseWheel;

        if( !IsAllDeployed )            // 배치가 안되어있으면 자동 배치(테스트용)
        {
            AutoShipDeployment(true);
        }
    }

    /// <summary>
    /// 클릭 입력이 들어왔을 때 실행될 함수
    /// </summary>
    /// <param name="screenPos">클릭된 스크린 좌표</param>
    private void OnClick(Vector2 screenPos)
    {
        onClick[(int)state]?.Invoke(screenPos);         // 상태별 델리게이트 실행
    }

    /// <summary>
    /// 마우스 이동 입력이 들어왔을 때 실행될 함수
    /// </summary>
    /// <param name="screenPos">마우스 커서가 있는 스크린 좌표</param>
    private void OnMouseMove(Vector2 screenPos)
    {
        onMouseMove[(int)state]?.Invoke(screenPos);     // 상태별 델리게이트 실행
    }

    /// <summary>
    /// 마우스 휠이 돌아갈 때 실행될 함수
    /// </summary>
    /// <param name="wheelDelta">휠이 돌아간 방향과 크기</param>
    private void OnMouseWheel(float wheelDelta)
    {
        onMouseWheel[(int)state]?.Invoke(wheelDelta);   // 상태별 델리게이트 실행
    }

    // 상태별 입력 처리 함수들 ------------------------------------------------------------------------------

    /// <summary>
    /// 함선 배치 씬에서 실행될 마우스 클릭처리 함수
    /// </summary>
    /// <param name="screenPos">클릭된 스크린 좌표</param>
    private void OnClick_ShipDeployment(Vector2 screenPos)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);              // 스크린 좌표를 월드 좌표로 변경
        if( SelectedShip != null && board.ShipDeplyment(SelectedShip, world) )  // 선택된 함선이 있으면 보드에 배치
        {
            Ship temp = SelectedShip;           // 배치 성공하면
            SelectedShip = null;                // 선택된 함선을 null로 만들고
            temp.gameObject.SetActive(true);    // 배치한 함선을 보이게 만든다.
        }
    }

    private void OnClick_Battle(Vector2 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = opponent.Board.WorldToGrid(worldPos);
        Attack(gridPos);
    }

    /// <summary>
    /// 함선 배치 씬에서 실행될 마우스 이동처리 함수
    /// </summary>
    /// <param name="screenPos">마우스 커서가 있는 스크린 좌표</param>
    private void OnMouseMove_ShipDeployment(Vector2 screenPos)
    {
        if (SelectedShip != null && !SelectedShip.IsDeployed)               // 선택된 함선이 있고 아직 배치가 안됬으면
        {
            Vector2Int gridPos = board.GetMouseGridPosition();              // 마우스 커서 위치를 그리드로 계산
            SelectedShip.transform.position = board.GridToWorld(gridPos);   // 계산한 위치로 TargetShip 옮기기

            bool isSuccess = board.IsShipDeployment(SelectedShip, gridPos); // 배치 가능한지 확인해서
            ShipManager.Inst.SetDeployModeColor(isSuccess);                 // 머티리얼 업데이트
        }
    }

    /// <summary>
    /// 함선 배치 씬에서 실행될 마우스 휠처리 함수
    /// </summary>
    /// <param name="wheelDelta">휠이 돌아간 방향과 크기</param>
    private void OnMouseWheel_ShipDeployment(float wheelDelta)
    {
        //Debug.Log(delta);
        bool ccw = false;           // 기본적으로 시계방향
        if (wheelDelta > 0.0f)      // 휠을 올리면
        {
            ccw = true;             // 반시계 방향으로
        }

        if (SelectedShip != null)   // TargetShip이 있으면
        {
            SelectedShip.Rotate(ccw);   // 휠방향에 따라 회전
            bool isSuccess = board.IsShipDeployment(SelectedShip, SelectedShip.transform.position); // 배치 가능한지 확인해서
            ShipManager.Inst.SetDeployModeColor(isSuccess);     // 머티리얼 업데이트
        }
    }

    // 함선 배치용 함수 ----------------------------------------------------------------------------

    /// <summary>
    /// 특정 종류의 함선을 선택하는 함수
    /// </summary>
    /// <param name="shipType">선택할 함선의 종류</param>
    public void SelectShipToDeploy(ShipType shipType)
    {
        SelectedShip = ships[(int)shipType - 1];    // shipType이 가르키는 배를 선택
    }

    /// <summary>
    /// 특정 종류의 함선을 배치 취소하는 함수
    /// </summary>
    /// <param name="shipType">배치 취소할 함선</param>
    public void UndoShipDeploy(ShipType shipType)
    {
        Ship targetShip = ships[(int)shipType - 1];
        board.UndoShipDeplyment(targetShip);        // 배치 취소
        targetShip.gameObject.SetActive(false);     // 함선을 안보이게 만들기
    }
}
