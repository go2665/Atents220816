using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    /// <summary>
    /// 페이즈가 진행되는 시간(스폰 후 딜레이)
    /// </summary>
    public float phaseDuration = 0.5f;

    /// <summary>
    /// 디졸브가 진행되는 시간(사망 이후에 실제로 사라질 때까지의 시간)
    /// </summary>
    public float dissolveDuration = 1.0f;

    /// <summary>
    /// 아웃라인의 두께
    /// </summary>
    const float Outline_Thickness = 0.005f;
        
    /// <summary>
    /// 쉐이더의 프로퍼티에 접근을 하기 위한 머티리얼
    /// </summary>
    Material mainMaterial;

    /// <summary>
    /// 페이즈가 끝났을 때 실행될 델리게이트
    /// </summary>
    public Action onPhaseEnd;

    /// <summary>
    /// 슬라임이 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action onDie;

    /// <summary>
    /// 이 오브젝트가 비활성화 될 때(디졸브가 다 끝난 이 후에) 실행될 델리게이트
    /// </summary>
    public Action onDisable;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        mainMaterial = renderer.material;       // 머티리얼 미리 찾아 놓기
    }

    private void OnEnable()
    {
        // 쉐이더 프로퍼티 값들 초기화
        ShowOutline(false);                             // 아웃라인 끄기
        mainMaterial.SetFloat("_Dissolve_Fade", 1.0f);  // 디졸브 진행 안된 상태로 만들기
        StartCoroutine(StartPhase());                   // 페이즈 시작
    }

    private void OnDisable()
    {
        onDisable?.Invoke();            // 비활성화 되었다고 알림(레디 큐에 다시 돌려주라는 신호를 보내는 것이 주 용도)
    }

    /// <summary>
    /// 이 슬라임을 죽일 때 실행할 함수
    /// </summary>
    public void Die()
    {
        // 디졸브 실행
        StartCoroutine(StartDissolve());

        // 죽었다고 신호보내기
        onDie?.Invoke();            
    }

    /// <summary>
    /// 페이즈를 실행하기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartPhase()
    {
        mainMaterial.SetFloat("_Phase_Thickness", 0.1f);    // 페이즈 진행 시 나올 중간 선의 두께 지정(보이게 만들기)
        mainMaterial.SetFloat("_Phase_Split", 0.0f);        // 페이즈 선의 위치 초기화

        float timeElipsed = 0.0f;                           // 측정 시간 초기화
        float phaseDurationNormalize = 1 / phaseDuration;   // split의 범위는 0~1이기 때문에 시간을 0~1 사이의 범위로 정규화 시키기 위한 용도

        while (timeElipsed < phaseDuration)     // 측정 시간이 목표시간에 도달할 때 까지 반복
        {
            timeElipsed += Time.deltaTime;      // 측정 시간을 프레임별로 누적시키기 

            mainMaterial.SetFloat("_Phase_Split", timeElipsed * phaseDurationNormalize);    // 측정 시간을 정규화 시켜 split 값으로 설정

            yield return null;                  // 다음 프레임까지 기다리기
        }

        mainMaterial.SetFloat("_Phase_Split", 1.0f);        // split을 최대치로 변경
        mainMaterial.SetFloat("_Phase_Thickness", 0.0f);    // 페이즈 진행 시 나올 중간 선의 두께 지정(안 보이게 만들기)
        onPhaseEnd?.Invoke();                               // 페이즈가 끝났다고 알림
    }

    /// <summary>
    /// 디졸브를 실행하기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDissolve()
    {
        mainMaterial.SetFloat("_Dissolve_Fade", 1.0f);      // 디졸브 진행 정도 초기화(디졸브가 전혀 진행되지 않게 설정)

        float timeElipsed = 0.0f;                                   // 측정 시간 초기화
        float dessolveDurationNormalize = 1 / dissolveDuration;     // fade의 범위는 0~1이기 때문에 시간을 0~1 사이의 범위로 정규화 시키기 위한 용도

        while (timeElipsed < dissolveDuration)      // 측정 시간이 목표시간에 도달할 때 까지 반복
        {
            timeElipsed += Time.deltaTime;          // 측정 시간을 프레임별로 누적시키기 

            mainMaterial.SetFloat("_Dissolve_Fade", 1 - timeElipsed * dessolveDurationNormalize); // 측정 시간을 정규화시켜 (1-정규화 된 값)을 fade 값으로 설정
                
            yield return null;                      // 다음 프레임까지 대기
        }

        this.gameObject.SetActive(false);           // 게임 오브젝트 비활성화(오브젝트 풀로 되돌리기)
    }

    /// <summary>
    /// 아웃라인 표시용 함수
    /// </summary>
    /// <param name="isShow">true면 아웃라인 표시, false면 아웃라인 끄기</param>
    public void ShowOutline(bool isShow)
    {
        if(isShow)
        {
            mainMaterial.SetFloat("_Outline_Thickness", Outline_Thickness); // 아웃라인에 두께 지정해서 보이게 만들기
        }
        else
        {
            mainMaterial.SetFloat("_Outline_Thickness", 0.0f);              // 아웃라인의 두께를 제거해서 안보이게 만들기
        }
    }
}
