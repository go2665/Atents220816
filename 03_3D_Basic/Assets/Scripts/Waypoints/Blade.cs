using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public Waypoints waypoints;
    public float moveSpeed = 1.0f;

    Rigidbody rigid;

    Transform target;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetTarget(waypoints.CuurentWaypoint);
    }

    private void FixedUpdate()
    {
        transform.LookAt(target);

        // 이번 fixedUpdate때 움직일 벡터 구하기
        Vector3 moveDelta = moveSpeed * Time.fixedDeltaTime * transform.forward;

        // 새로운 위치구하기
        Vector3 newPos = rigid.position + moveDelta;

        // 위치 최종 결정
        rigid.MovePosition(newPos);

        // 새로운 위치가 도착지점에 거의 근접하면
        if ((target.position - newPos).sqrMagnitude < 0.0025f)
        {
            target = waypoints.MoveToNextWaypoint();
        }        
    }

    void SetTarget(Transform target)
    {
        this.target = target;
        transform.LookAt(target);
    }

}
