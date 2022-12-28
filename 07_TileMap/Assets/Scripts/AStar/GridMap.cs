using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public GridMap(Tilemap background, Tilemap obstacle)
    {
        // background의 크기를 기반으로 nodes 생성하기
        // 새로 생성하는 Node의 x,y좌표는 타일맵에서의 좌표와 같아야 한다.
        // 갈 수 없는 지역 표시(obstacle에 타일이 있는 부분은 Wall로 표시)

    }

    /// <summary>
    /// 그리드 맵에서 특정 그리드 좌표에 존재하는 노드 찾는 함수
    /// </summary>
    /// <param name="x">타일맵 기준 x 좌표</param>
    /// <param name="y">타일맵 기준 y 좌표</param>
    /// <returns>찾은 노드(없으면 null)</returns>
    public Node GetNode(int x, int y)
    {
        if(IsValidPosion(x, y))
        {
            return nodes[GridToIndex(x, y)];
        }
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
        foreach(var node in nodes)
        {
            node.ClearAStarData();
        }
    }

    /// <summary>
    /// 입력 받은 좌표가 맵 내부인지 확인하는 함수
    /// </summary>
    /// <param name="x">확인할 위치의 x</param>
    /// <param name="y">확인할 위치의 y</param>
    /// <returns>맵안이면 true. 아니면 false</returns>
    public bool IsValidPosion(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    /// <summary>
    /// 입력 받은 좌표가 맵 내부인지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치의 좌표</param>    
    /// <returns>맵안이면 true. 아니면 false</returns>
    public bool IsValidPosition(Vector2Int pos)
    {
        return IsValidPosion(pos.x, pos.y);
    }

    /// <summary>
    /// Grid좌표를 index로 변경하기 위한 함수. (GetNode에서 사용하기 위한 함수.)
    /// </summary>
    /// <param name="x">그리드 좌표 X</param>
    /// <param name="y">그리드 좌표 Y</param>
    /// <returns>그리드 좌표가 변경된 인덱스 값(nodes의 특정 노드를 얻기 위한 인덱스)</returns>
    private int GridToIndex(int x, int y)
    {
        return x + ((height - 1) - y) * width;  // 왼쪽 아래가 (0,0)이고 x+는 오른쪽, y+는 위쪽이기 때문에 이렇게 변환
    }
}
