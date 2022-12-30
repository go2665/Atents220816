using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Spawner : MonoBehaviour
{
    /// <summary>
    /// 몬스터를 생성하는 시간 간격
    /// </summary>
    public float delay = 1.0f; 

    /// <summary>
    /// 최대로 유지 가능한 몬스터의 수
    /// </summary>
    public int capacity = 2; 

    /// <summary>
    /// 스폰 영역의 크기. (transform의 position에서 부터의 크기)
    /// </summary>
    public Vector2 size;

    /// <summary>
    /// 이전 몬스터 생성에서부터 경과한 시간
    /// </summary>
    float elapsed = 0.0f;

    /// <summary>
    /// 현재 생성된 몬스터의 수
    /// </summary>
    int count = 0;
        
    /// <summary>
    /// 스포너가 있는 씬의 몬스터 매니저
    /// </summary>
    SceneMonsterManager manager;

    /// <summary>
    /// 스폰 영역 중에서 벽이 아닌 지역
    /// </summary>
    List<Node> spawnAreaList;

    /// <summary>
    /// 슬라임이 스폰되면 실행되는 델리게이트
    /// </summary>
    public Action<Slime> onSpawned;

    private void Start()
    {
        manager = GetComponentInParent<SceneMonsterManager>();
        spawnAreaList = manager.CalcSpawnArea(this);    // 스폰 영역 중에서 벽이 아닌 위치들의 모음 가져오기
    }

    private void Update()
    {
        if (count < capacity)
        {
            elapsed += Time.deltaTime;
            if (elapsed > delay)
            {
                Spawn();
                elapsed = 0.0f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));

        Vector3 p0 = pos;
        Vector3 p1 = pos + new Vector3(size.x, 0);
        Vector3 p2 = pos + new Vector3(size.x, size.y);
        Vector3 p3 = pos + new Vector3(0, size.y);

        Handles.color = Color.yellow;
        Handles.DrawLine(p0, p1, 5);
        Handles.DrawLine(p1, p2, 5);
        Handles.DrawLine(p2, p3, 5);
        Handles.DrawLine(p3, p0, 5);
    }

    public Slime Spawn()
    {
        Slime slime = null;
        if (count < capacity)
        {
            slime = SlimeFactory.Inst.GetSlime();
            if (slime != null)
            {
                count++;
                slime.onDie += DecressCount;    // 죽을 때 스폰 갯수 감소
                slime.transform.SetParent(this.transform);              // 스폰되면 스포너의 자식으로 만들기
                slime.Initialize(manager.GridMap, GetSpawnPosition());  // 그리드맵 전달 + 스폰될 위치 전달

                onSpawned?.Invoke(slime);       // 생성 완료되면 델리게이트 실행
            }
        }
        return slime;
    }

    void DecressCount()
    {
        count--;
    }

    /// <summary>
    /// spawnAreaList에서 현재 몬스터가 없는 위치를 랜덤으로 찾는 함수
    /// </summary>
    /// <returns>몬스터가 없는 노드의 월드 좌표</returns>
    Vector3 GetSpawnPosition()
    {
        List<Node> spawns = new List<Node>();
        foreach(var node in spawnAreaList)              // 미리 찾아 놓은 spawnAreaList 뒤지기
        {
            if( node.gridType == Node.GridType.Plain )  // 평지일 경우 
            {
                spawns.Add(node);                       // spawns에 저장
            }
        }

        int index = UnityEngine.Random.Range(0, spawns.Count);
        Node target = spawns[index];                    // spawns 중에서 랜덤으로 하나 선택
        Vector2Int gridPos = new Vector2Int(target.x, target.y);    
        return manager.GridMap.GridToWorld(gridPos);    // 선택한 그리드 좌표를 월드좌표로 변경해서 리턴
    }

    
}
