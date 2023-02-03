using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 보드의 가로 세로 길이(항상 정사각형)
    /// </summary>
    const int BoardSize = 10;

    /// <summary>
    /// 보드의 배 배치 정보. 2차원 대신 1차원으로 저장
    /// </summary>
    ShipType[] shipInfo = null;

    private void Awake()
    {
        shipInfo = new ShipType[BoardSize* BoardSize];  // shipInfo 초기화(none으로 초기화됨)
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
        return grid.x + grid.y * BoardSize;
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
        return Vector3Int.zero;
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
        foreach (var temp in ship.Positions)
        {
            shipInfo[GridToIndex(temp)] = ShipType.None;
        }
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
        Vector2Int grisPos = WorldToGrid(worldPos);
        return IsShipDeployment(ship, grisPos, out _);
    }


}
