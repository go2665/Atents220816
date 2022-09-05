using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. EnemySpawner.cs를 수정하여 Enemy 프리팹을 지속적으로 생성하게 만들어보기.
//  1.1.반드시 코루틴으로 작성해야 한다.

public class EnemySpawner : MonoBehaviour
{
    // 필요한 변수가 무엇인가? -> Enemy 프리팹, 지속적으로 동작을 하는 시간 간격
    
    public GameObject spawnPrefab_Normal;  // 생성할 적의 프리팹
    public GameObject spawnPrefab_Special;
    public float interval = 0.5f;   // 생성할 시간 간격

    public float minY = -4.0f;     // 스폰이 일어나는 최저 높이
    public float maxY = 4.0f;      // 스폰이 일어나는 최고 높이


    // 필요한 기능은 무엇인가? -> Enemy 프리팹을 생성하는 코루틴
    private void Start()
    {
        StartCoroutine(Spawn());    // 코루틴 시작(종료는 없음)
    }

    protected virtual IEnumerator Spawn()
    {
        while (true)    // 무한 반복
        {
            GameObject prefab = spawnPrefab_Normal; // 기본적으로 생성하는 것는 spawnPrefab_Normal
            if ( Random.value < 0.1f )
            {
                prefab = spawnPrefab_Special;       // 10% 이하의 확률로 Special 적 생성
            }

            GameObject obj = Instantiate(prefab, transform);  // 생성하고 부모를 이 오브젝트로 설정
            obj.transform.Translate(0, Random.Range(minY, maxY), 0);    // 스폰 생성 범위 안에서 랜덤으로 높이 정하기
            yield return new WaitForSeconds(interval);  // interval만큼 대기
        }
    }

    // 개발용 정보를 항상 그리는 함수
    protected virtual void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1,0,0);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new(1, Mathf.Abs(maxY) + Mathf.Abs(minY) + 2, 1));
    }

    // 개발용 정보를 선택되었을 때만 그리는 함수
    //private void OnDrawGizmosSelected()
    //{        
    //}

}
