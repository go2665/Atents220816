using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public Waypoints waypoints;         // 따라다닐 웨이포인트들을 가지고 있는 클래스
    public float moveSpeed = 1.0f;      // 칼날 이동 속도
    public float spinSpeed = 720.0f;

    Rigidbody rigid;

    Transform target;   // 목표로하는 웨이포인트의 트랜스폼
    Transform bladeObj;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        bladeObj = transform.GetChild(0);
    }

    private void Start()
    {
        SetTarget(waypoints.CurrentWaypoint);   // 첫 웨이포인트 지정
    }

    private void Update()
    {
        bladeObj.Rotate(spinSpeed * Time.deltaTime, 0, 0);
    }

    private void FixedUpdate()
    {
        transform.LookAt(target);   // 항상 목적지를 바라보도록 
                
        Vector3 moveDelta = moveSpeed * Time.fixedDeltaTime * transform.forward; // 이번에 움직일 정도 계산                
        Vector3 newPos = rigid.position + moveDelta;    // 새로운 위치구하기        
        rigid.MovePosition(newPos);                     // 새 위치로 이동        

        // 새로운 위치가 도착지점에 거의 근접하면
        if ((target.position - newPos).sqrMagnitude < 0.0025f)
        {
            SetTarget(waypoints.MoveToNextWaypoint());  // 다음 웨이포인트로 목적지 설정
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        IDead dieTarget = other.GetComponent<IDead>();
        if(dieTarget != null)
        {
            dieTarget.Die();
        }
    }

    /// <summary>
    /// 다음 목적지 지정하는 함수
    /// </summary>
    /// <param name="target">새 웨이포인트 트랜스폼</param>
    void SetTarget(Transform target)
    {
        this.target = target;       // 목적지 정하고
        transform.LookAt(target);   // 그쪽을 바라보게 만들기
    }

}
