using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Networking;

public class SpawnerManager : MonoBehaviour
{
    GridMap gridMap;
    Tilemap background;
    Tilemap obstacle;

    Spawner[] spawners;

    List<Slime> spawnedList;

    public GridMap GridMap => gridMap;

    public List<Slime> SpawnedList => spawnedList;

    private void Awake()
    {
        Transform grid = transform.parent;
        Transform child = grid.GetChild(0);
        background = child.GetComponent<Tilemap>();         // 타일맵 가져오기
        child = grid.GetChild(1);
        obstacle = child.GetComponent<Tilemap>();

        gridMap = new GridMap(background, obstacle);        // 그리드 맵 만들기

        spawners = GetComponentsInChildren<Spawner>();      // 자식으로 있는 스포너 가져오기

        foreach(var spawner in spawners)
        {
            spawner.onSpawned += (slime) =>
            {
                spawnedList.Add(slime);
                slime.onDie += () => spawnedList.Remove(slime);
            };
        }

        spawnedList = new List<Slime>();

        //StartCoroutine(GetSpawnerData());
    }   

    IEnumerator GetSpawnerData()
    {
        string url = "http://go26652.dothome.co.kr/HTTP_Data/SpawnerData.txt";
        //{"delay":1.5,"capacity":10}

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Web request Error!");
        }
        else
        {
            string json = www.downloadHandler.text;
            SpawnerData data = JsonUtility.FromJson<SpawnerData>(json);
            //Debug.Log($"delay : {data.delay}, capacity : {data.capacity}"); ;

            foreach (var spawner in spawners)
            {
                spawner.delay = data.delay;
                spawner.capacity = data.capacity;
            }
        }



        yield return null;
    }

    /// <summary>
    /// 스포너의 스폰 영역 중에서 벽이 아닌 노드들만 찾아서 돌려주는 함수
    /// </summary>
    /// <param name="spawner">계산할 스포너</param>
    /// <returns>스포너의 스폰 영역에 있는 벽이 아닌 노드들</returns>
    public List<Node> CalcSpawnArea(Spawner spawner)
    {
        List<Node> nodes = new List<Node>();

        Vector2Int min = gridMap.WorldToGrid(spawner.transform.position);                           // 그리드 좌표의 최소 값 계산
        Vector2Int max = gridMap.WorldToGrid(spawner.transform.position + (Vector3)spawner.size);   // 그리드 좌표의 최대 값 계산
        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                if (gridMap.IsSpawnable(x, y))          // 스폰 가능한 위치면
                {
                    nodes.Add(gridMap.GetNode(x,y));    // 기록해 놓기
                }
            }
        }
        return nodes;
    }
}
