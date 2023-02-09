using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    // 변수들 --------------------------------------------------------------------------------------
    
    /// <summary>
    /// 이 플레이어가 가지고 있는 보드.(자식으로 있음)
    /// </summary>
    protected Board board;

    /// <summary>
    /// 이 플레이어가 가지고 있는 함선들
    /// </summary>
    protected Ship[] ships;

    /// <summary>
    /// 아직 침몰하지 않은 함선의 수
    /// </summary>
    protected int remainShipCount;

    /// <summary>
    /// 이번턴 행동 여부. true면 행동 완료, false면 아직 행동하지 않음
    /// </summary>
    bool isActionDone = false;

    /// <summary>
    /// 대전 상대
    /// </summary>
    protected PlayerBase opponent;

    // 공격 관련 변수 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 후보지역을 표시하는데 사용하는 프리팹
    /// </summary>
    public GameObject highCandidatePrefab;

    /// <summary>
    /// 아직 공격하지 않은 모든 지점.
    /// </summary>
    List<int> attackCandiateIndices;

    /// <summary>
    /// 다음에 공격했을 때 성공확률이 높은 지점.(후보지역)
    /// </summary>
    List<int> attackHighCandidateIndices;

    /// <summary>
    /// 마지막에 공격을 성공한 위치
    /// </summary>
    Vector2Int lastAttackSuccessPos;

    /// <summary>
    /// invalid한 좌표 표시용.(이번턴에 공격을 성공하지 못한 것을 표시)
    /// </summary>
    readonly Vector2Int NOT_SUCCESS_YET = -Vector2Int.one;

    /// <summary>
    /// 후보지역 표시를 위한 게임 오브젝트 저장하는 딕셔너리
    /// </summary>
    Dictionary<int, GameObject> highCandidateMark = new Dictionary<int, GameObject>();


    // 프로퍼티들 ----------------------------------------------------------------------------------

    public Board Board => board;
    public Ship[] Ships => ships;
    public bool IsDpeat => remainShipCount < 1;


    // 델리게이트 들

    /// <summary>
    /// 플레이어의 공격이 실패했음을 알리는 델리게이트
    /// </summary>
    public Action<PlayerBase> onAttackFail;

    /// <summary>
    /// 플레이어의 행동이 끝났음을 알리는 델리게이트
    /// </summary>
    public Action onActionEnd;

    /// <summary>
    /// 플레이어가 패배했음을 알리는 델리게이트
    /// </summary>
    public Action<PlayerBase> onDefeat;


    // 유니티 이벤트 함수들 -------------------------------------------------------------------------
    protected virtual void Awake()
    {
        board = GetComponentInChildren<Board>();
        attackHighCandidateIndices = new List<int>();
    }

    protected virtual void Start()
    {
        int shipTypeCount = ShipManager.Inst.ShipTypeCount;
        ships = new Ship[shipTypeCount];
        for (int i = 0; i < shipTypeCount; i++)
        {
            ShipType shipType = (ShipType)(i + 1);
            ships[i] = ShipManager.Inst.MakeShip(shipType, transform);  // 타입에 맞춰 함선 생성
            ships[i].onSinking += OnShipDestroy;                        // 함선 침몰할 때 실행되는 델리게이트에 함수 등록
            board.onShipAttacked[shipType] = ships[i].OnAttacked;       // 보드에서 특정 타입의 배가 공격 당했을 때 실행될 델리게이트에 함수 등록
        }
        remainShipCount = shipTypeCount;    
                
        lastAttackSuccessPos = NOT_SUCCESS_YET;     // 직전 공격에서 성공하지 않았다.(시작이라 당연히 없음)

        PlayerBase[] players = FindObjectsOfType<PlayerBase>();
        if(players[0] != this)
        {
            opponent = players[0];  // players[0]이 나와 다르면 players[0]은 적이다.
        }
        else
        {
            opponent = players[1];  // players[0]이 나와 다르지 않다면 남은 것(players[1])이 적이다.
        }
        //Debug.Log($"{this.gameObject.name}의 상대방은 {opponent.gameObject.name}이다.");

    }

    // 턴 관리용 함수 ------------------------------------------------------------------------------
    public virtual void OnPlayerTurnStart(int turnNumber)
    {
        isActionDone = true;
    }

    public virtual void OnPlayerTurnEnd()
    {
    }

    // 공격용 함수 ---------------------------------------------------------------------------------

    // - 공격
    public void Attack(Vector2Int attackGridPos)
    {
        //if(!isActionDone)
        {
            bool result = opponent.Board.OnAttacked(attackGridPos);
            if( result )
            {
                AttackSuccessProcess(attackGridPos);
            }
            else
            {
                lastAttackSuccessPos = NOT_SUCCESS_YET; // 공격이 실패하면 무조건 lastAttackSuccessPos 비우가
                onAttackFail?.Invoke(this); // 공격 실패 알림(로그 출력용)
            }

            RemoveHighCandidate(Board.GridToIndex(attackGridPos));  // 성공이든 실패든 공격 지점이 후보지로 되어있으면 제거

            isActionDone = true;
        }
    }

    public void Attack(Vector3 worldPos)
    {
        Attack(opponent.Board.WorldToGrid(worldPos));
    }

    /// <summary>
    /// 공격이 성공했을 때 공격 성공 지점 주변을 후보지역에 추가하는 함수
    /// </summary>
    /// <param name="attackGridPos">공격한 지점</param>
    private void AttackSuccessProcess(Vector2Int attackGridPos)
    { 
        // 이전에 공격이 성공한 적이 있는지 확인
        if(lastAttackSuccessPos != NOT_SUCCESS_YET)
        {
            // 한턴 앞의 공격이 성공했었다.
            AddHighCandidataByTwoPosition(attackGridPos, lastAttackSuccessPos); // 직선으로 후보지역 추가
        }
        else
        {
            // 한턴 앞의 공격이 성공하지 못했다.
            AddNeighborToHighCanditate(attackGridPos);  // 공격한 지점의 주변을 후보지역으로 추가
        }
        lastAttackSuccessPos = attackGridPos;
    }
    // - 자동 공격


    // 후보지역 처리 함수 --------------------------------------------------------------------------
    private void AddHighCandidataByTwoPosition(Vector2Int now, Vector2Int last)
    {
        Debug.Log($"연속 공격 성공 : {now}");
        
        if(InSuccessLine(last, now, true))
        {
            // 가로 방향으로 붙어있다.
            // 연속으로 공격이 성공했는데 붙어있다 => 같은배로 예상

            // 가로선 밖의 후보지역은 제거
            List<int> dels = new List<int>();
            foreach(var index in attackHighCandidateIndices)
            {
                Vector2Int pos = Board.IndexToGrid(index);
                if(pos.y != now.y)
                {
                    dels.Add(index);        // 가로선을 벗어난 후보지역 모으기
                }
            }
            foreach(var del in dels)
            {
                RemoveHighCandidate(del);   // 가로선을 벗어난 후보지역들을 제거하기
            }

            // 가로 선상의 양끝부분에 후보지역 추가
            // -> 가로 선상으로 now의 x를 +-로 계속 증가시킴. 공격 실패나 보드 밖이 나오면 취소, 공격을 안한 지역이 나오면 후보지에 추가
            Vector2Int newPos = now;

            // 공격 지점의 왼쪽에 있는 후보지역 찾기
            for (int i = now.x - 1; i> -1;i--)                       
            {
                newPos.x = i;

                // 보드를 벗어나면 끝
                if (!Board.IsValidPosition(newPos)) 
                    break;

                // 공격 실패한 지점이면 후보지역을 추가하지 않고 break;
                if( opponent.Board.IsAttackFailPosition(newPos))
                    break;

                // 공격이 가능한 지점이면 후보지역에 추가하고 break;
                if( opponent.Board.IsAttackable(newPos))
                {
                    AddHighCandidate(Board.GridToIndex(newPos));
                    break;
                }                
            }

            // 공격 지점의 오른쪽에 있는 후보지역 찾기
            for (int i = now.x + 1; i < Board.BoardSize; i++)
            {
                newPos.x = i;

                // 보드를 벗어나면 끝
                if (!Board.IsValidPosition(newPos))
                    break;

                // 공격 실패한 지점이면 후보지역을 추가하지 않고 break;
                if (opponent.Board.IsAttackFailPosition(newPos))
                    break;

                // 공격이 가능한 지점이면 후보지역에 추가하고 break;
                if (opponent.Board.IsAttackable(newPos))
                {
                    AddHighCandidate(Board.GridToIndex(newPos));
                    break;
                }
            }
        }
        else if(InSuccessLine(last, now, false))
        {
            // 세로 방향으로 붙어있다.
            // 연속으로 공격이 성공했는데 붙어있다 => 같은배로 예상
        }
        else
        {
            // 서로 떨어져 있는데 공격이 성공했다. => 다른배
            AddNeighborToHighCanditate(now);  // 공격한 지점의 주변을 후보지역으로 추가
        }
    }

    /// <summary>
    /// 공격한 지점의 이웃을 후보지역으로 추가하기
    /// </summary>
    /// <param name="gridPos">공격한 지점</param>
    private void AddNeighborToHighCanditate(Vector2Int gridPos)
    {
        Debug.Log($"공격 성공 : {gridPos}");

        // gridPos의 주변 4방향 중 valid하고 이전에 공격을 하지 않았던 지역만 후보지역에 추가
        Vector2Int[] neighbors = { new(-1, 0), new(1, 0), new(0, -1), new(0, 1) };
        foreach(Vector2Int neighbor in neighbors )
        {
            Vector2Int pos = gridPos + neighbor;
            if( Board.IsValidPosition(pos) && opponent.Board.IsAttackable(pos) )
            {
                int index = Board.GridToIndex(pos);
                AddHighCandidate(index);
            }
        }
    }

    /// <summary>
    /// 후보지역 리스트에 인덱스를 추가하는 함수
    /// </summary>
    /// <param name="index">추가할 인덱스</param>
    private void AddHighCandidate(int index)
    {
        // attackHighCandidateIndices에 인덱스 추가
        if (!attackHighCandidateIndices.Exists( (x) => x == index ))    // 중복이 있으면 안함.
        {
            attackHighCandidateIndices.Insert(0, index);                // 추가할 때는 항상 맨 앞에 추가.

            // highCandidatePrefab을 이용해서 후보지역 표시하기
            GameObject obj = Instantiate(highCandidatePrefab, transform);
            obj.transform.position = opponent.board.IndexToWorld(index);
            obj.gameObject.name = $"HighCandidate_{index}";
            highCandidateMark[index] = obj;
        }

    }

    /// <summary>
    /// index에 있는 후보지역 삭제 함수
    /// </summary>
    /// <param name="index">삭제할 위치를 나타내는 인덱스</param>
    private void RemoveHighCandidate(int index)
    {
        if(attackHighCandidateIndices.Exists((x) => x == index ))   // index가 리스트에 있으면
        {
            attackHighCandidateIndices.Remove(index);   // attackHighCandidateIndices 리스트에서 index 제거
            Destroy(highCandidateMark[index]);          // 개발용으로 후보지역 오브젝트 만든것 삭제
            highCandidateMark[index] = null;            // 딕셔너리에서 저장하고 있던 value 정리
            highCandidateMark.Remove(index);            // 딕셔너리에서 해당 key도 제거
        }
    }

    /// <summary>
    /// start에서 end 한칸 앞까지 모두 공격 성공이었는지 체크하는 함수
    /// </summary>
    /// <param name="start">확인 시작지점</param>
    /// <param name="end">확인 종료지점</param>
    /// <param name="isHorizontal">true면 가로로 체크, false면 세로로 체크</param>
    /// <returns>true면 같은 줄이고 그 사이는 모두 공격 성공이었다. false면 다른 줄이거나 중간에 공격 실패가 있다.</returns>
    private bool InSuccessLine(Vector2Int start, Vector2Int end, bool isHorizontal)
    {
        bool result = true;             // 결과값이 들어갈 변수
        Vector2Int pos = start;         // start에서 end 앞까지 진행될 임시 변수
        int dir = 1;                    // 움직이는 방향(+냐 -냐)
        if(isHorizontal)
        {
            // 가로 방향
            if( start.y == end.y )      // 높이가 같은지 확인
            {
                if( start.x > end.x )   // 정방향인지 역방향인지 확인
                {   
                    dir = -1;           // 왼쪽->오른쪽으로 갈 때는 +1, 오른쪽->왼쪽으로 갈때는 -1, 
                }

                for (int i = start.x; i < end.x; i += dir)  // start에서 end로 진행
                {
                    pos.x = i;          // 임시 변수 갱신
                    if (!opponent.board.IsAttackSuccessPosition(pos))   
                    {
                        result = false; // 공격이 성공못한 지점이 나오면 실패
                        break;
                    }
                }
            }
            else
            {
                result = false;         // 가로인데 높이가 다르면 무조건 실패
            }
        }
        else
        {
            // 세로 방향
            if (start.x == end.x)       // 세로줄이 같은지 확인
            {
                if (start.y > end.y)
                {
                    dir = -1;           // 위쪽->아래쪽으로 갈 때는 +1, 아래쪽->위쪽으로 갈때는 -1, 
                }

                for (int i = start.y; i < end.y; i += dir)  // start에서 end로 진행
                {
                    pos.y = i;          // 임시 변수 위치 갱신
                    if (!opponent.board.IsAttackSuccessPosition(pos))
                    {
                        result = false; // 공격이 성공못한 지점이 나오면 실패
                        break;
                    }
                }
            }
            else
            {
                result = false;         // 세로인데 좌우위치가 다르면 무조건 실패
            }
        }

        return result;                  // 한줄로 계속 공격 성공위치가 나와야 true
    }

    // 함선 배치용 함수들 --------------------------------------------------------------------------

    /// <summary>
    /// 자동으로 함선을 배치하는 함수
    /// </summary>
    /// <param name="isShowShips">배를 보여 줄지 여부</param>
    public void AutoShipDeployment(bool isShowShips = false)
    {
        // 아직 배치되지 않은 모든 함선을 배치

        // 후보지 : 함선이 배치될 수 있는 칸들                

        int maxCapacity = Board.BoardSize * Board.BoardSize;
        List<int> highPriority = new(maxCapacity);  // 우선순위가 높은 후보지
        List<int> lowPriority = new(maxCapacity);   // 우선순위가 낮은 후보지

        // 가장자리 부분은 우선 순위가 낮은 후보지에 추가
        for (int i = 0; i < maxCapacity; i++)
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

        // 배치된 함선이 있으면 그 함선에 대한 후보지들을 처리        
        foreach (var ship in ships)
        {
            if (ship.IsDeployed)     // 배가 배치되어 있으면 
            {
                int[] shipIndice = new int[ship.Size];
                for (int i = 0; i < ship.Size; i++)
                {
                    shipIndice[i] = Board.GridToIndex(ship.Positions[i]);   // 배의 위치를 전부 인덱스로 변경해서 저장
                }
                foreach (var index in shipIndice)
                {
                    highPriority.Remove(index); // 이미 배치된 곳은 high와 low 모두에서 저거
                    lowPriority.Remove(index);
                }

                List<int> toLow = GetShipAroundPositions(ship); // 함선 배치 지역 주변의 지역 구하기
                foreach (var index in toLow)
                {
                    highPriority.Remove(index); // high에서 제거하고 low에 추가하기
                    lowPriority.Add(index);
                }
            }
        }

        // 각 후보지를 랜덤으로 섞기(후보지 별로 섞기)
        int[] temp = highPriority.ToArray();
        Util.Shuffle(temp);
        highPriority = new(temp);

        temp = lowPriority.ToArray();
        Util.Shuffle(temp);
        lowPriority = new(temp);

        // 함선별 배치 작업 시작
        foreach (var ship in ships)
        {
            if (!ship.IsDeployed)       // 배가 배치되어 있지 않은 것만 처리
            {
                ship.RandomRotate();    // 함선을 랜덤하게 회전 시키기

                bool failDeployment = true;     // 배치에 성공했는지 실패했는지 표시용 변수
                Vector2Int gridPos;             // 함선의 머리 부분이 배치될 위치
                Vector2Int[] shipPositions;     // 함선이 배치될 예정인 위치들
                int counter = 0;                // 무한 루프 방지용

                // highPriority 영역에 함선 배치 시도
                do
                {
                    int headIndex = highPriority[0];    // high에서 첫번째 인덱스 꺼내기
                    highPriority.RemoveAt(0);

                    gridPos = Board.IndexToGrid(headIndex); // 꺼낸 인덱스를 그리드 좌표로 변경

                    failDeployment = !board.IsShipDeployment(ship, gridPos, out shipPositions); // 배치 가능한지 확인 + 배치 가능하면 배치될 위치들 가져오기
                    if (failDeployment)
                    {
                        highPriority.Add(headIndex);    // 배치가 불가능하면 인덱스를 다시 high에 넣기
                    }
                    else
                    {
                        // 배치가 가능하면 배치될 예정 위치들이 high에 있는지 확인(모든 위치가 high에 있을 때만 배치 진행)
                        for (int i = 1; i < shipPositions.Length; i++)
                        {
                            int bodyIndex = Board.GridToIndex(shipPositions[i]);    // 몸통부분의 위치를 인덱스로 변경하고
                            if (!highPriority.Exists((x) => x == bodyIndex))        // highPriority에 있는지 확인
                            {
                                highPriority.Add(headIndex);    // 없으면 headIndex를 high에 되돌리기
                                failDeployment = true;          // 실패했다고 표시하고 for 취소
                                break;
                            }
                        }
                    }
                    counter++;  // 루프 횟수 카운팅

                    // 배치에 실패하고 루프 카운트 횟수가 10회 미만이고 highPriority에 인덱스가 있으면 루프 반복
                } while (failDeployment && counter < 10 && highPriority.Count > 0);

                // lowPriority 영역도 포함해서 함선 배치 시도
                counter = 0;
                while (failDeployment && counter < 1000)    // 성공할 때까지 1000번 반복하기(high에서 10번 이상 실패했거나 high가 비었을 때 실행됨)
                {
                    int headIndex = lowPriority[0];         // low에서 하나 꺼내서
                    lowPriority.RemoveAt(0);
                    gridPos = Board.IndexToGrid(headIndex); // 그리드 좌표로 변경하고

                    failDeployment = !board.IsShipDeployment(ship, gridPos, out shipPositions); // 배치 시도
                    if (failDeployment)
                    {
                        lowPriority.Add(headIndex);         // 실패하면 low에 다시 넣기
                    }
                    counter++;  // 카운터 증가
                }

                // 최종 실패(high도 실패하고 low도 1000번 이상 실패)
                if (failDeployment)
                {
                    // 여기로 들어오면 구조적으로 문제가 있다.(맵 크기를 늘리던가 함선 종류를 줄여야 한다.)
                    Debug.LogWarning("함선 자동 배치 실패!!!!!");
                    break;
                }

                // 배치할 위치가 결정됨. 함선 배치 시작
                board.ShipDeplyment(ship, gridPos);                     // 함선 배치
                ship.transform.position = board.GridToWorld(gridPos);   // 함선 오브젝트 위치 이동
                ship.gameObject.SetActive(isShowShips);                 // 함선 보여주고 싶으면 보여주고 아니면 안보여지게 하기

                // 함선이 배치된 지역을 high와 low에서 제거
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach (var tempPos in shipPositions)
                {
                    tempList.Add(Board.GridToIndex(tempPos));
                }
                foreach (var tempIndex in tempList)
                {
                    highPriority.Remove(tempIndex);
                    lowPriority.Remove(tempIndex);
                }

                // 함선 주변 위치를 low로 보내기
                List<int> toLow = GetShipAroundPositions(ship);     // 함선 배치 지역 주변의 지역 구하기
                foreach (var index in toLow)
                {
                    if (highPriority.Exists((x) => x == index))  // high에 있으면
                    {
                        highPriority.Remove(index);                 // high에서 제거한 후
                        lowPriority.Add(index);                     // low에 넣기
                    }
                }
            }
        }
    }

    /// <summary>
    /// 함선의 주변 위치를 구해주는 함수
    /// </summary>
    /// <param name="ship">주변 위치를 구할 함선</param>
    /// <returns>함선 주변 위치의 인덱스를 저장한 리스트</returns>
    private List<int> GetShipAroundPositions(Ship ship)
    {
        List<int> toLowList = new List<int>(ship.Size * 2 + 6); // 리스트 생성. 함선의 양 옆(size*2) + 머리 + 꼬리 + 머리의 양옆 + 꼬리의 양옆
        if (ship.Direction == ShipDirection.North || ship.Direction == ShipDirection.South)
        {
            // 함선이 위 아래를 향하고 있을 때
            foreach (var tempPos in ship.Positions)
            {
                toLowList.Add(Board.GridToIndex(tempPos + Vector2Int.right));   // 함선의 양옆을 toLowList에 추가
                toLowList.Add(Board.GridToIndex(tempPos + Vector2Int.left));
            }
            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.North)
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

    /// <summary>
    /// 모든 함선의 배치를 취소하는 함수
    /// </summary>
    public void UndoAllShipDeployment()
    {
        foreach (var ship in ships)
        {
            board.UndoShipDeplyment(ship);
        }
    }

    // 내 함선 파괴 및 패배처리용 함수 --------------------------------------------------------------
    private void OnShipDestroy(Ship ship)
    {
        remainShipCount--;  // 남은 함선 수 감소
        Debug.Log($"배가 {remainShipCount}척 남았습니다.");

        if (remainShipCount <= 0)
        {
            OnDefeat();
        }
    }

    private void OnDefeat()
    {
        Debug.Log($"{gameObject.name} 패배");
        onDefeat?.Invoke( this );
    }
}
