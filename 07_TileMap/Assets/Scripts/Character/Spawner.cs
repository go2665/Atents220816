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
    /// 스폰 영역의 크기(transform의 position에서 부터의 크기)
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
            }

            Vector3 pos = transform.position + new Vector3(Random.Range(0, size.x), Random.Range(0, size.y), 0);
            slime.transform.position = pos;
        }
        return slime;
    }

    void DecressCount()
    {
        count--;
    }

    
}
