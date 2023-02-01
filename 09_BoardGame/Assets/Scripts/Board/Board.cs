using System.Collections;
using System.Collections.Generic;
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
    ShipType[] shipType;

    /// <summary>
    /// 배열의 인덱스 값을 그리드 좌표로 변환해주는 static 함수
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    static public Vector2Int IndexToGrid(int index)
    {
        return Vector2Int.zero;
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변환해주는 static 함수
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    static public int GridToIndex(Vector2Int grid)
    {
        return 0;
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변환해주는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector3 GridToWorld(int x, int y)
    {
        return Vector3.zero;
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
        return Vector2Int.zero;
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


    /// <summary>
    /// 함선 배치 함수
    /// </summary>
    /// <returns></returns>
    public bool ShipDeplyment()
    {
        return false;
    }

    /// <summary>
    /// 함선 배치 취소 함수
    /// </summary>
    public void UndoShipDeplyment()
    {
    }
}
