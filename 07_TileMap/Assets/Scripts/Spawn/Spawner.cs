using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// 스포너가 배치되어 있는 맵
    /// </summary>
    //GridMap gridMap;

    /// <summary>
    /// 스포너가 배치되어 있는 맵을 확인하기 위한 프로퍼티
    /// </summary>
    //public GridMap GridMap => gridMap;

    SceneMonsterManager manager;

    private void Start()
    {
        manager = GetComponentInParent<SceneMonsterManager>();
        //gridMap = manager.GridMap;
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
                slime.onDie -= DecressCount;    // DecressCount가 누적되지 않게하기 위한 조치
                slime.onDie += DecressCount;

                Vector3 spawnPos = manager.GetRandomSpawnPosition(transform.position, size);
                //Debug.Log($"SpawnPos : ({spawnPos.x}, {spawnPos.y})");
                slime.Initialize(manager.GridMap, spawnPos);
            }
        }
        return slime;
    }

    void DecressCount()
    {
        count--;
    }

    
}
