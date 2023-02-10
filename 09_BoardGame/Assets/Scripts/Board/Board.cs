using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 보드의 가로 세로 길이(항상 정사각형)
    /// </summary>
    public const int BoardSize = 10;

    /// <summary>
    /// 보드의 배 배치 정보. 2차원 대신 1차원으로 저장
    /// </summary>
    ShipType[] shipInfo = null;

    /// <summary>
    /// 공격 당한 위치 정보. 공격을 했으면 true, 아직 안했으면 false
    /// </summary>
    bool[] bombInfo;

    /// <summary>
    /// 공격 당한 위치를 시각적으로 보여주는 클래스. (성공(O), 실패(X), 그냥 표시(검은구))
    /// </summary>
    BombMark bombMark;

    /// <summary>
    /// 개발용 배 배치 정보를 보여줄지 여부. true면 보여주고, false면 보여주지 않는다.
    /// </summary>
    public bool isShowShipDeploymentInfo = true;

    /// <summary>
    /// 개발용 배 배치 정보 생성용 클래스.
    /// </summary>
    ShipDeploymentInfoMaker shipDeploymentInfo = null;

    public const int NOT_VALID = -1;

    // 델리게이트 ----------------------------------------------------------------------------------
    public Dictionary<ShipType, Action> onShipAttacked;


    // 유니티 이벤트 함수들 -------------------------------------------------------------------------

    private void Awake()
    {
        shipInfo = new ShipType[BoardSize* BoardSize];  // shipInfo 초기화(none으로 초기화됨)
        bombInfo = new bool[BoardSize* BoardSize];

        bombMark = GetComponentInChildren<BombMark>();

        onShipAttacked = new Dictionary<ShipType, Action>(ShipManager.Inst.ShipTypeCount);
        onShipAttacked[ShipType.None] = null;           // ShipType.None 부분은 무조건 없음. 접근했을 때 터지는 것 방지
        onShipAttacked[ShipType.Carrier] = null;
        onShipAttacked[ShipType.Battleship] = null;
        onShipAttacked[ShipType.Destroyer] = null;
        onShipAttacked[ShipType.Submarine] = null;
        onShipAttacked[ShipType.PatrolBoat] = null;

        if ( isShowShipDeploymentInfo)
        {
            shipDeploymentInfo = GetComponentInChildren<ShipDeploymentInfoMaker>();
        }
    }

    // static 함수들 -------------------------------------------------------------------------------

    /// <summary>
    /// 배열의 인덱스 값을 그리드 좌표로 변환해주는 static 함수
    /// </summary>
    /// <param name="index">계산할 인덱스 값</param>
    /// <returns>변환된 그리드 좌표</returns>
    public static Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % BoardSize, index / BoardSize);
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변환해주는 static 함수
    /// </summary>
    /// <param name="grid">계산할 그리드 좌표</param>
    /// <returns>변환된 인덱스 값</returns>
    public static int GridToIndex(Vector2Int grid)
    {
        if(IsValidPosition(grid))
            return grid.x + grid.y * BoardSize;

        return NOT_VALID;
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변환해주는 static 함수
    /// </summary>
    /// <param name="x">계산할 그리드 x좌표</param>
    /// <param name="y">계산할 그리드 y좌표</param>
    /// <returns>변환된 인덱스 값</returns>
    public static int GridToIndex(int x, int y)
    {
        return x + y * BoardSize;
    }

    /// <summary>
    /// 특정 위치가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="gridPos">확인할 위치</param>
    /// <returns>true면 board안, false면 board밖</returns>
    public static bool IsValidPosition(Vector2Int gridPos)
    {
        return gridPos.x > -1 && gridPos.x < BoardSize && gridPos.y > -1 && gridPos.y < BoardSize;
    }

    

    // 유틸리티 함수들 -----------------------------------------------------------------------------

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변환해주는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x + 0.5f, 0, -(y + 0.5f));
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변환해주는 함수
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public Vector3 GridToWorld(Vector2Int grid)
    {
        return GridToWorld(grid.x, grid.y);
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변환해주는 함수
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        worldPos.y = 0;
        Vector3 diff = worldPos - transform.position;
        return new Vector2Int(Mathf.FloorToInt(diff.x), Mathf.FloorToInt(-diff.z));
    }

    /// <summary>
    /// 인덱스 값을 월드좌표로 변환해주는 함수
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector3 IndexToWorld(int index)
    {
        return GridToWorld(IndexToGrid(index));
    }

    /// <summary>
    /// 현재 마우스의 위치를 그리드 좌표로 변경해서 리턴
    /// </summary>
    /// <returns>현재 마우스의 그리드 좌표</returns>
    public Vector2Int GetMouseGridPosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();          // 현재 마우스 위치 가져오기
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);    // 마우스 위치를 월드 좌표로 변환
        
        return WorldToGrid(worldPos);   // 월드 좌표를 다시 그리드 좌표로 변경해서 돌려주기
    }

    /// <summary>
    /// 특정 월드 좌표에 어떤 종류의 배가 배치되어있는지 알려주는 함수
    /// </summary>
    /// <param name="worldPos">확인할 월드좌표</param>
    /// <returns>해당 위치에 있는 배의 종류</returns>
    public ShipType GetShipType(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);
        int index = GridToIndex(gridPos);
        return shipInfo[index];
    }

    /// <summary>
    /// 보드 리셋(함선도 자동 처리)
    /// </summary>
    public void ResetBoard(Ship[] ships)
    {
        // 함선 초기화
        foreach (var ship in ships)
        {
            UndoShipDeplyment(ship);    // 모든 함선 배치 취소
        }

        // 공격 표시 초기화
        for (int i = 0; i < bombInfo.Length; i++)
        {
            bombInfo[i] = false;        // 공격 여부는 모두 안한것으로 설정
        }
        bombMark.ResetBombMark();       // 폭탄 마크 리셋
    }

    // 확인용 함수들 -------------------------------------------------------------------------------

    /// <summary>
    /// 월드 좌표가 보드 안쪽인지 확인하는 함수
    /// </summary>
    /// <param name="worldPos">체크할 월드 좌표</param>
    /// <returns>보드 안쪽이면 true, 아니면 false</returns>
    public bool IsValidPosition(Vector3 worldPos)
    {
        Vector3 diff = worldPos - transform.position;
        return diff.x >= 0.0f && diff.x <= BoardSize && diff.z < 0.0f && diff.z > -BoardSize;
    }

    /// <summary>
    /// 특정 위치에 배가 있는지 확인하는 함수
    /// </summary>
    /// <param name="shipPos">확인할 위치</param>
    /// <returns>true면 배가 있다. false면 배가 없다.</returns>
    private bool IsShipDeployed(Vector2Int shipPos)
    {
        return shipInfo[GridToIndex(shipPos)] != ShipType.None;
    }

    /// <summary>
    /// 특정 위치가 공격 실패한 지점인지 확인하는 함수
    /// </summary>
    /// <param name="gridPos">확인할 위치</param>
    /// <returns>true면 공격이 실패한 지점. false면 공격이 성공한 지점</returns>
    public bool IsAttackFailPosition(Vector2Int gridPos)
    {
        int index = GridToIndex(gridPos);
        // 공격을 한 지점은 상대방도 배가 있는지 없는지 알 수 있으므로 shipInfo를 봐도 상관이 없다.
        return bombInfo[index] && (shipInfo[index] == ShipType.None);   // 공격을 했고 거기에 배가 없었다.
    }

    /// <summary>
    /// 특정 위치가 공격 성공한 지점인지 확인하는 함수
    /// </summary>
    /// <param name="gridPos">확인할 위치</param>
    /// <returns>true면 공격이 성공한 지점. false면 공격이 실패한 지점</returns>
    public bool IsAttackSuccessPosition(Vector2Int gridPos)
    {
        int index = GridToIndex(gridPos);
        // 공격을 한 지점은 상대방도 배가 있는지 없는지 알 수 있으므로 shipInfo를 봐도 상관이 없다.
        return bombInfo[index] && (shipInfo[index] != ShipType.None);   // 공격을 했고 거기에 배가 있었다.
    }

    // 함선 배치 관련 함수들 ------------------------------------------------------------------------
    /// <summary>
    /// 함선 배치 함수
    /// </summary>
    /// <param name="ship">배치할 함선</param>
    /// <param name="pos">배치할 그리드 좌표</param>
    /// <returns>성공하면 true, 아니면 false</returns>
    public bool ShipDeplyment(Ship ship, Vector2Int pos)
    {
        Vector2Int[] gridPositions;
        bool result = IsShipDeployment(ship, pos, out gridPositions);   // 배치 가능여부 확인
        if( result )
        {
            // 배치가 가능하면
            foreach(var temp in gridPositions)
            {
                shipInfo[GridToIndex(temp)] = ship.Type;    // shipInfo에 함선 배치 표시
            }

            Vector3 worldPos = GridToWorld(pos);
            ship.transform.position = worldPos;             // 힘선의 위치 옮기기
            ship.Deploy(gridPositions);                     // 함선 배치 처리

            // 개발용 오브젝트 추가 부분
            if( shipDeploymentInfo != null )                // 개발용 오브젝트를 만드는 클래스가 있으면
            {
                Vector3[] worlds = new Vector3[gridPositions.Length];
                for(int i = 0;i < worlds.Length;i++)
                {
                    worlds[i] = GridToWorld(gridPositions[i]);      // 그리드 좌표를 모두 월드 좌표로 바꿔서
                }
                shipDeploymentInfo.MarkShipDeploymentInfo(ship.Type, worlds);   // 표시하기
            }
        }

        return result;
    }

    /// <summary>
    /// 함선 배치 함수
    /// </summary>
    /// <param name="ship">배치할 함선</param>
    /// <param name="pos">배치할 월드좌표</param>
    /// <returns>성공하면 true, 아니면 false</returns>
    public bool ShipDeplyment(Ship ship, Vector3 pos)
    {
        Vector2Int gridPos = WorldToGrid(pos);
        return ShipDeplyment(ship, gridPos);
    }

    /// <summary>
    /// 함선 배치 취소 함수
    /// </summary>
    /// <param name="ship">배치를 취소할 배</param>
    public void UndoShipDeplyment(Ship ship)
    {
        // 개발용 오브젝트 추가 부분
        if (shipDeploymentInfo != null)                // 개발용 오브젝트를 만드는 클래스가 있으면
        {
            shipDeploymentInfo.UnMarkShipDeploymentInfo(ship.Type);
        }

        // board에 표시해 놓았던 것 해제
        if (ship.Positions != null)
        {
            foreach (var temp in ship.Positions)
            {
                shipInfo[GridToIndex(temp)] = ShipType.None;
            }
        }

        // 함선 배치 해제
        ship.UnDeploy();

    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배(크기와 방향)</param>
    /// <param name="pos">확인할 배의 위치(배의 머리 위치, 그리드 포지션)</param>
    /// <param name="gridPositions">배가 배치될 수 있다고 확인된 위치(결과가 true일 때)</param>
    /// <returns>true면 배치 가능, false면 배치 불가능</returns>
    public bool IsShipDeployment(Ship ship, Vector2Int pos, out Vector2Int[] gridPositions)
    {
        gridPositions = new Vector2Int[ship.Size]; 
        Vector2Int dir = Vector2Int.zero;
        switch (ship.Direction)         // 확인 방향 결정하기
        {
            case ShipDirection.North:
                dir = Vector2Int.up;    // (0,1). 북쪽을 바라보니까 꼬리로 갈 수록 y는 증가
                break;
            case ShipDirection.East:
                dir = Vector2Int.left;  // (-1,0). 꼬리로 갈 수록 x 감소
                break;
            case ShipDirection.South:
                dir = Vector2Int.down;  // (0,-1). 꼬리로 갈 수록 y 감소
                break;
            case ShipDirection.West:
                dir = Vector2Int.right; // (1,0). 꼬리로 갈 수록 x 증가
                break;
            default:
                break;
        }

        // 확인할 위치들 따로 뽑아 놓기
        for(int i=0;i<ship.Size;i++)
        {
            gridPositions[i] = pos + dir * i;
        }

        // 확인할 위치들을 한칸씩 확인
        bool result = true;
        foreach(var temp in gridPositions)
        {
            // 한칸이라도 보드를 벗어나거나 배가 배치되어있으면 실패
            if( !IsValidPosition(temp) || IsShipDeployed(temp) )
            {
                result = false;
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배(크기와 방향)</param>
    /// <param name="gridPos">확인할 배의 위치(배의 머리 위치, 그리드 포지션)</param>
    /// <returns>true면 배치 가능, false면 배치 불가능</returns>
    public bool IsShipDeployment(Ship ship, Vector2Int gridPos)
    {
        return IsShipDeployment(ship, gridPos, out _);
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배(크기와 방향)</param>
    /// <param name="worldPos">확인할 배의 위치(배의 머리 위치, 월드 좌표)</param>
    /// <returns>true면 배치 가능, false면 배치 불가능</returns>
    public bool IsShipDeployment(Ship ship, Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);
        return IsShipDeployment(ship, gridPos, out _);
    }


    // 피격용 --------------------------------------------------------------------------------------
    
    /// <summary>
    /// 상대 플레이어에게 공격을 받았을 때 실행되는 함수
    /// </summary>
    /// <param name="gridPos">공격 받은 위치(그리드 좌표)</param>
    /// <returns>true면 공격에 의해 함선에 명중했다. false면 실패했다.</returns>
    public bool OnAttacked(Vector2Int gridPos)
    {
        bool result = false;

        if( IsValidPosition(gridPos) )          // 보드 위인지 확인
        {
            int index = GridToIndex(gridPos);
            if(IsAttackable(index))             // 공격 했던 지점인지 아닌지 확인
            {
                bombInfo[index] = true;         // 공격 가능하면 공격했다고 표시

                if(shipInfo[index] != ShipType.None)    // 그곳에 배가 있으면
                {
                    result = true;                              // 공격으로 배가 명중됬다고 표시
                    onShipAttacked[shipInfo[index]]?.Invoke();  // 공격 당한 배의 델리게이트 실행
                }

                bombMark.SetBombMark(GridToWorld(gridPos.x, gridPos.y), result);    // 붐마크 표시
            }
        }

        return result;
    }

    public bool IsAttackable(Vector2Int gridPos)
    {
        return IsAttackable(GridToIndex(gridPos));
    }

    public bool IsAttackable(int index)
    {
        return !bombInfo[index];
    }
}
