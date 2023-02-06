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
        randomDeployment.onClick.AddListener(AutoShipDeployment);
         
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

    private void AutoShipDeployment()
    {
        // 아직 배치되지 않은 모든 함선을 배치

        // 후보지 : 함선이 배치될 수 있는 칸들                
        
        int maxCapacity = Board.BoardSize * Board.BoardSize;
        List<int> highPriority = new(maxCapacity);  // 우선순위가 높은 후보지
        List<int> lowPriority = new(maxCapacity);   // 우선순위가 낮은 후보지

        // 가장자리 부분은 우선 순위가 낮은 후보지에 추가
        for (int i=0;i<maxCapacity;i++)
        {
            if (i % Board.BoardSize == 0                        // 0,10,20,30,40,50,60,70,80,90
                || i % Board.BoardSize == Board.BoardSize - 1   // 9,19,29,39,49,59,69,79,89,99
                || (0 < i && i < Board.BoardSize - 1)           // 1,2,3,4,5,6,7,8
                || ((Board.BoardSize * (Board.BoardSize - 1)) < i && i < (Board.BoardSize * Board.BoardSize - 1)))  // 91,92,93,94,95,96,97,98
            {
                lowPriority.Add(i);     // 맵의 가장자리는 낮은 후보지에 추가
            }
            else
            {
                highPriority.Add(i);    // 그 외 지역은 높은 후보지에 추가
            }
        }

        // 각 후보지를 랜덤으로 섞기(후보지 별로 섞기)
        int[] temp = highPriority.ToArray();
        Util.Shuffle(temp);
        highPriority = new(temp);

        temp = lowPriority.ToArray();
        Util.Shuffle(temp);
        lowPriority = new(temp);

        // 배치된 함선이 있으면 그 함선에 대한 후보지들을 처리        
        foreach(var ship in testShips)
        {
            if(ship.IsDeployed)     // 배가 배치되어 있으면 
            {
                int[] shipIndice = new int[ship.Size];
                for(int i=0;i<ship.Size;i++)
                {
                    shipIndice[i] = Board.GridToIndex(ship.Positions[i]);   // 배의 위치를 전부 인덱스로 변경해서 저장
                }
                foreach(var index in shipIndice) 
                { 
                    highPriority.Remove(index); // 이미 배치된 곳은 high와 low 모두에서 저거
                    lowPriority.Remove(index);
                }

                List<int> toLow = GetShipAroundPositions(ship); // 함선 배치 지역 주변의 지역 구하기
                foreach(var index in toLow)
                {
                    highPriority.Remove(index); // high에서 제거하고 low에 추가하기
                    lowPriority.Add(index);
                }
            }
        }

        // 함선별 배치 작업 시작
        foreach (var ship in testShips)
        {
            if (!ship.IsDeployed)       // 배가 배치되어 있지 않은 것만 처리
            {
                ship.RandomRotate();    // 함선을 랜덤하게 회전 시키기
            }
        }

        // 높은 후보지에서 위치 하나 꺼내서 그 위치에 함선 배치 시도
        // 배치에 실패하면 다시 높은 후보지에서 위치를 새로 꺼내 다시 시도
        // 일정 회수 이상 실패하면 낮은 후보지에서 선택 시도
        // 낮은 후보지에서는 될때까지 반복 시도
        // 배치 위치를 성공적으로 찾았다면 함선 배치 실행

    }

    /// <summary>
    /// 함선의 주변 위치를 구해주는 함수
    /// </summary>
    /// <param name="ship">주변 위치를 구할 함선</param>
    /// <returns>함선 주변 위치의 인덱스를 저장한 리스트</returns>
    private List<int> GetShipAroundPositions(Ship ship)
    {
        List<int> toLowList = new List<int>(ship.Size * 2 + 6); // 리스트 생성. 함선의 양 옆(size*2) + 머리 + 꼬리 + 머리의 양옆 + 꼬리의 양옆
        if( ship.Direction == ShipDirection.North || ship.Direction == ShipDirection.South )
        {
            // 함선이 위 아래를 향하고 있을 때
            foreach(var tempPos in ship.Positions)
            {
                toLowList.Add(Board.GridToIndex(tempPos + Vector2Int.right));   // 함선의 양옆을 toLowList에 추가
                toLowList.Add(Board.GridToIndex(tempPos + Vector2Int.left));
            }
            Vector2Int head;
            Vector2Int tail;
            if(ship.Direction == ShipDirection.North)
            {
                head = ship.Positions[0] + Vector2Int.down;     // 머리 구하기
                tail = ship.Positions[^1] + Vector2Int.up;      // 꼬리 구하기. [^1]은 [lenght-1]과 같음
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.up;
                tail = ship.Positions[^1] + Vector2Int.down;
            }
            toLowList.Add(Board.GridToIndex(head));                     // 머리를 toLowList에 추가
            toLowList.Add(Board.GridToIndex(head + Vector2Int.right));  // 머리의 오른쪽을 toLowList에 추가
            toLowList.Add(Board.GridToIndex(head + Vector2Int.left));   // 머리의 왼쪽을 toLowList에 추가
            toLowList.Add(Board.GridToIndex(tail));                     // 꼬리를 toLowList에 추가
            toLowList.Add(Board.GridToIndex(tail + Vector2Int.right));  // 꼬리의 오른쪽을 toLowList에 추가
            toLowList.Add(Board.GridToIndex(tail + Vector2Int.left));   // 꼬리의 왼쪽을 toLowList에 추가
        }
        else
        {
            foreach (var tempPos in ship.Positions)
            {
                toLowList.Add(Board.GridToIndex(tempPos + Vector2Int.up));
                toLowList.Add(Board.GridToIndex(tempPos + Vector2Int.down));
            }
            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.East)
            {
                head = ship.Positions[0] + Vector2Int.right;
                tail = ship.Positions[^1] + Vector2Int.left;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.left;
                tail = ship.Positions[^1] + Vector2Int.right;
            }
            toLowList.Add(Board.GridToIndex(head));
            toLowList.Add(Board.GridToIndex(head + Vector2Int.up));
            toLowList.Add(Board.GridToIndex(head + Vector2Int.down));
            toLowList.Add(Board.GridToIndex(tail));
            toLowList.Add(Board.GridToIndex(tail + Vector2Int.up));
            toLowList.Add(Board.GridToIndex(tail + Vector2Int.down));
        }

        toLowList.RemoveAll((x) => x == Board.NOT_VALID);   // 보드 그리드 범위를 벗어난 것은 제거
        return toLowList;   // 최종 목록 리턴
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
