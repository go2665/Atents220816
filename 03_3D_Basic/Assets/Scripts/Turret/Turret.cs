using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // turnSpeed가 아무런 영향을 주지 않는다. (총구가 즉시 회전한다.)

    public float turnSpeed = 360.0f;
    public float sightRadius = 5.0f;

    Transform target = null;
    Transform barrelBody;

    private void Awake()
    {
        barrelBody = transform.GetChild(2);
    }

    private void Start()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.radius = sightRadius;
    }

    /// <summary>
    /// 인스펙터 창에서 값이 성공적으로 변경되었을 때 호출되는 함수
    /// </summary>
    private void OnValidate()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.radius = sightRadius;
    }

    private void Update()
    {
        if (target != null)
        {
            // 보간을 어떻게 적용할 것인가?

            // 총구를 플레이어쪽으로 돌려야 함
            Vector3 dir = target.position - barrelBody.position;    // 총구에서 플레이어의 위치로 가는 방향 벡터 계산
            dir.y = 0;      // 방향 벡터에서 y축의 영향을 제거 => xz 평면상의 방향만 남음
            barrelBody.rotation = Quaternion.LookRotation(dir);     // 최종적인 방향을 바라보는 회전을 만들어서 총몸에 적용
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }
}
