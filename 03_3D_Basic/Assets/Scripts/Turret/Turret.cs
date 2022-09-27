using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // turnSpeed가 아무런 영향을 주지 않는다. (총구가 즉시 회전한다.)

    public float turnSpeed = 2.0f;
    public float sightRadius = 5.0f;

    public GameObject bulletPrefab;
    public float fireInterval = 0.5f;
    public float fireAngle = 10.0f;

    Transform fireTransform;
    IEnumerator fireCoroutine;
    WaitForSeconds waitFireInterval;
    bool isFiring = false;


    Transform target = null;
    Transform barrelBody;

    float currentAngle = 0.0f;
    //float targetAngle = 0.0f;
    Vector3 initialForward;

    private void Awake()
    {
        barrelBody = transform.GetChild(2);
        fireTransform = barrelBody.GetChild(1);

        fireCoroutine = PeriodFire();
    }

    private void Start()
    {
        initialForward = transform.forward;
        SphereCollider col = GetComponent<SphereCollider>();
        col.radius = sightRadius;

        waitFireInterval = new WaitForSeconds(fireInterval);
        //StartCoroutine(fireCoroutine);      // 코루틴을 자주 껏다 켰다 할 때는 코루틴을 변수에 저장하고 사용해야한다.
        //StartCoroutine(PeriodFire());     // 이 코드는 PeriodFire()를 1회용으로 사용한다. 그래서 가비지가 생성된다.
    }

    /// <summary>
    /// 인스펙터 창에서 값이 성공적으로 변경되었을 때 호출되는 함수
    /// </summary>
    private void OnValidate()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        if (col != null)
        {
            col.radius = sightRadius;
        }
    }

    private void Update()
    {
        LookTarget();
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
            FireStop();
        }
    }

    private void LookTarget()
    {
        if (target != null)
        {
            // 보간을 사용한 경우(감속하며 회전)
            //Vector3 dir = target.position - barrelBody.position;    // 총구에서 플레이어의 위치로 가는 방향 벡터 계산
            //dir.y = 0;      // 방향 벡터에서 y축의 영향을 제거 => xz 평면상의 방향만 남음

            //// turnSpeed초에 걸처서 0->1로 변경된다. (시작점에서 도착점까지 걸리는 시간이 trunSpeed초)
            //dir = Vector3.Lerp(barrelBody.forward, dir, turnSpeed * Time.deltaTime);    

            //barrelBody.rotation = Quaternion.LookRotation(dir);     // 최종적인 방향을 바라보는 회전을 만들어서 총몸에 적용


            // 각도를 사용하는 경우(등속도로 회전)
            Vector3 barrelToPlayerDir = target.position - barrelBody.position;    // 총구에서 플레이어의 위치로 가는 방향 벡터 계산
            barrelToPlayerDir.y = 0;

            // 정방향일 때 0~180도. 역방향일 때 0~-180
            float betweenAngle = Vector3.SignedAngle(barrelBody.forward, barrelToPlayerDir, barrelBody.up);

            Vector3 resultDir;
            if( Mathf.Abs(betweenAngle) > 0.1f )   // 사이각이 일정 각도 이하인지 체크
            {
                // 사이각이 충분히 벌어진 경우

                float rotateDirection = 1.0f;   // 일단 +방향(시계방향)으로 설정
                if (betweenAngle < 0)
                {
                    rotateDirection = -1.0f;    // betweenAngle이 -면 rotateDirection도 -1로
                }

                // 초당 turnSpeed만큼 회전하는데, rotateDirection로 시계방향으로 회전할지 반시계 방향으로 회전할지 결정
                currentAngle += (rotateDirection * turnSpeed * Time.deltaTime);

                resultDir = Quaternion.Euler(0, currentAngle, 0) * initialForward;
            }
            else
            {
                // 사이각이 거의 0인 경우
                resultDir = barrelToPlayerDir;
            }
            barrelBody.rotation = Quaternion.LookRotation(resultDir);

            // 플레이어가 발사각 안에 있으면 공격 시작. 없으면 중지
            if (!isFiring && IsInFireAngle())
            {
                FireStart();
            }
            if (isFiring && !IsInFireAngle())
            {
                FireStop();
            }
        }
    }

    bool IsInFireAngle()
    {                
        Vector3 dir = target.position - barrelBody.position;
        dir.y = 0.0f;
        return Vector3.Angle(barrelBody.forward, dir) < fireAngle;
    }

    private void Fire()
    {
        // 총알을 발사한다.
        // 총알 프리팹. 총알이 발사될 방향과 위치. 총알이 발사되는 주기. 
        Instantiate(bulletPrefab, fireTransform.position, fireTransform.rotation);
    }

    IEnumerator PeriodFire()
    {
        while (true)
        {
            Fire();
            yield return waitFireInterval;  // 가비지를 줄이는 방식
        }
    }

    private void FireStart()
    {
        isFiring = true;
        StartCoroutine(fireCoroutine);
    }

    private void FireStop()
    {
        StopCoroutine(fireCoroutine);
        isFiring = false;
    }

    private void OnDrawGizmos()
    {
        
    }
}
