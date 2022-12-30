using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneMonsterManager : MonoBehaviour
{
    GridMap gridMap;
    Tilemap background;
    Tilemap obstacle;

    Spawner[] spawners;

    public GridMap GridMap => gridMap;

    private void Awake()
    {
        Transform grid = transform.parent;
        Transform child = grid.GetChild(0);
        background = child.GetComponent<Tilemap>();         // 타일맵 가져오기
        child = grid.GetChild(1);
        obstacle = child.GetComponent<Tilemap>();

        gridMap = new GridMap(background, obstacle);        // 그리드 맵 만들기

        spawners = GetComponentsInChildren<Spawner>();      // 자식으로 있는 스포너 가져오기
    }

    /// <summary>
    /// 랜덤하게 스폰할 위치를 구하는 함수
    /// </summary>
    /// <param name="pos">스포너의 위치</param>
    /// <param name="size">스포너의 스폰 영역 크기</param>
    /// <returns>몬스터가 스폰될 위치</returns>
    public Vector3 GetRandomSpawnPosition(Vector3 pos, Vector2 size)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int min = gridMap.WorldToGrid(pos);                  // 그리드 좌표의 최소 값 계산
        Vector2Int max = gridMap.WorldToGrid(pos + (Vector3)size);  // 그리드 좌표의 최대 값 계산
        for (int y= min.y; y<max.y;y++)
        {
            for(int x = min.x; x<max.x;x++)
            {
                if (gridMap.IsSpawnable(x, y))                      // 스폰 가능한 위치면
                {
                    result.Add(new(x, y));                          // 기록해 놓기
                }
            }
        }
        // 기록한 위치중에서 하나를 랜덤으로 골라 월드 좌표로 변경해서 리턴
        return gridMap.GridToWorld(result[Random.Range(0, result.Count)]);  
    }
}
