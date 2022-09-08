using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidSpawner : EnemySpawner
{
    private Transform destination;

    private void Awake()
    {
        // 오브젝트가 생성된 직후 => 이 오브젝트 안에 있는 것들을 초기화 할 때 사용
        //      이 오브젝트안에 있는 모든 컴포넌트가 생성이 완료되었다.
        //      그리고 이 오브젝트의 자식 오브젝트들도 모두 생성이 완료되었다.
        
        //destination = transform.Find("DestinationArea");    // "DestinationArea"라는 이름을 가진 자식 찾기
        destination = transform.GetChild(0);    // 첫번째 자식 찾기
    }

    //private void Start()
    //{
    //    // 첫번째 업데이트 실행 직전 호출
    //    // 나와 다른 오브젝트를 가져와야 할 때 사용
    //}

    protected override IEnumerator Spawn()
    {
        while (true)    // 무한 반복
        {
            GameObject obj = Instantiate(spawnPrefab_Normal, transform);       // 생성하고 부모를 이 오브젝트로 설정
            obj.transform.Translate(0, Random.Range(minY, maxY), 0);    // 스폰 생성 범위 안에서 랜덤으로 높이 정하기

            Vector3 destPosition = destination.position + new Vector3(0.0f, Random.Range(minY, maxY), 0.0f);    // 목적지 위치 결정

            Asteroid asteroid = obj.GetComponent<Asteroid>();
            if(asteroid != null)
            {
                // 운석이 destPosition로 가는 방향벡터를 구하고
                // direction을 방향벡터로 만들어 준다.
                asteroid.direction = (destPosition - asteroid.transform.position).normalized;  
            }

            yield return new WaitForSeconds(interval);  // interval만큼 대기
        }
    }

    // 개발용 정보를 항상 그리는 함수
    protected override void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1,0,0);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new(1, Mathf.Abs(maxY) + Mathf.Abs(minY) + 2, 1));

        if( destination == null )
        {
            destination = transform.GetChild(0);
        }
        Gizmos.DrawWireCube(destination.position, new(1, Mathf.Abs(maxY) + Mathf.Abs(minY) + 2, 1));
    }
}
