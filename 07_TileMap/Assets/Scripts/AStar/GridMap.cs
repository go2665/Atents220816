using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    /// <summary>
    /// 이 맵에 있는 전체 노드들의 배열
    /// </summary>
    Node[] nodes;

    /// <summary>
    /// 맵의 가로 크기
    /// </summary>
    int width;

    /// <summary>
    /// 맵의 세로 크기
    /// </summary>
    int height;

    /// <summary>
    /// 그리드맵을 만들기 위한 생성자
    /// </summary>
    /// <param name="width">생성할 맵의 가로 크기</param>
    /// <param name="height">생성할 맵의 세로 크기</param>
    public GridMap(int width, int height)
    {
        // 축 기준 : 기본적으로 왼쪽 아래가 원점
        // 축 방향 : 오른쪽으로 갈 수록 x+, 위로 갈 수록 y+

        this.width = width;         // 가로 세로 길이 기록
        this.height = height;

        nodes = new Node[width * height];   // 노드 배열 생성

        for(int y = 0; y<height ; y++)
        {
            for(int x = 0; x < width ; x++)
            {
                int index = GridToIndex(x, y);
                nodes[index] = new Node(x, y);  // 노드 전부 생성해서 배열에 넣기
            }
        }
    }

    /// <summary>
    /// 그리드 맵에서 특정 그리드 좌표에 존재하는 노드 찾는 함수
    /// </summary>
    /// <param name="x">타일맵 기준 x 좌표</param>
    /// <param name="y">타일맵 기준 y 좌표</param>
    /// <returns>찾은 노드(없으면 null)</returns>
    public Node GetNode(int x, int y)
    {
        return null;
    }

    /// <summary>
    /// 그리드 맵에서 특정 그리드 좌표에 존재하는 노드 찾는 함수
    /// </summary>
    /// <param name="pos">타일맵 기준으로 한 좌표</param>
    /// <returns>찾은 노드(없으면 null)</returns>
    public Node GetNode(Vector2Int pos)
    {
        return GetNode(pos.x, pos.y);
    }

    /// <summary>
    /// 맵이 가지는 모든 노드들의 A* 데이터 초기화
    /// </summary>
    public void ClearAStarData()
    {

    }

    /// <summary>
    /// 입력 받은 좌표가 맵 내부인지 확인하는 함수
    /// </summary>
    /// <param name="x">확인할 위치의 x</param>
    /// <param name="y">확인할 위치의 y</param>
    /// <returns>맵안이면 true. 아니면 false</returns>
    private bool IsValidPosion(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    private int GridToIndex(int x, int y)
    {
        return x + ((height - 1) - y) * width;
    }
}
