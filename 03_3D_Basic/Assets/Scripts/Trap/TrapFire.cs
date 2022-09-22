using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : TrapBase
{
    ParticleSystem ps;

    private void Awake()
    {
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    protected override void TrapActivate(GameObject target)
    {
        //ps.Simulate(0);   // 파티클 시스템 재생 위치를 0초로 돌리는 작업
        ps.Play();      // 파티클 시스템 재생 시작
        StartCoroutine(EffectStop());   // 파티클 시스템 끝내기 예약

        IDead deadTarget = target.GetComponent<IDead>();
        if (deadTarget != null)
        {
            deadTarget.Die();   // 죽이는 함수 호출
        }
    }

    IEnumerator EffectStop()
    {
        yield return new WaitForSeconds(1); // 1초 후에
        ps.Stop();  // 파티클 시스템 종료
    }
}
