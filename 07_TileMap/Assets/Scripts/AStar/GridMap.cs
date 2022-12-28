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
    /// 원점의 그리즈 좌표(맵 왼쪽 아래 끝 부분의 그리드 좌표)
    /// </summary>
    Vector2Int origin;

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
    /// 그리드 맵을 Tilemap을 사용해 생성하는 생성자
    /// </summary>
    /// <param name="background">그리드맵의 전체 크기를 결정할 타일맵(가로,세로,원점 결정)</param>
    /// <param name="obstacle">그리드맵에서 벽으로 설정될 타일을 가지는 타일맵(벽 위치 결정)</param>
    public GridMap(Tilemap background, Tilemap obstacle)
    {
        // background의 크기를 기반으로 nodes 생성하기
        width = background.size.x;      // background의 크기 받아와서 가로 세로 길이로 사용
        height = background.size.y;

        nodes = new Node[width * height];   // 전체 노드가 들어갈 배열 생성

        // 새로 생성하는 Node의 x,y좌표는 타일맵에서의 좌표와 같아야 한다.
        origin = (Vector2Int)background.origin; // 타일맵에 기록된 원점 저장
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(origin.x + x, origin.y + y);
                nodes[index] = new Node(origin.x + x, origin.y + y);  // 노드 전부 생성해서 배열에 넣기
            }
        }

        // 갈 수 없는 지역 표시(obstacle에 타일이 있는 부분은 Wall로 표시)
        for (int y = background.cellBounds.yMin; y < background.cellBounds.yMax; y++)       
        {
            for (int x = background.cellBounds.xMin; x < background.cellBounds.xMax; x++)
            {
                // background 영역위에 있는 obstacle만 확인
                TileBase tile = obstacle.GetTile(new(x, y));
                if (tile != null)   // 타일이 있으면 벽지역이다.
                {
                    Node node = GetNode(x, y);
                    node.gridType = Node.GridType.Wall; // 벽으로 표시
                }
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
        return x >= origin.x && y >= origin.y && x < (width + origin.x) && y < (height + origin.y);
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
        // (x,y) = x + y * 너비;              // 원점이 왼쪽위에 있을 때
        // (x,y) = x + (높이-1)-y) * 너비     // 원점이 왼쪽아래에 있을 때
        return (x - origin.x) + ((height - 1) - y + origin.y) * width;  // 왼쪽 아래가 (0,0)이고 x+는 오른쪽, y+는 위쪽이기 때문에 이렇게 변환        
    }
}
