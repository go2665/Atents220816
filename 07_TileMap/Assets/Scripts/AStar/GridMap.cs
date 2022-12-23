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
                nodes[x + y * width] = new Node(x, y);  // 노드 전부 생성해서 배열에 넣기
            }
        }
    }
}
