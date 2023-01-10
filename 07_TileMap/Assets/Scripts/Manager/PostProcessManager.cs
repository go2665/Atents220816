using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    Volume postProcessVolume;
    Vignette vignette;

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();                 // 볼륨 컴포넌트 가져오기
        postProcessVolume.profile.TryGet<Vignette>(out vignette);   // 볼륨 내부에 있는 비네트 항목 가져오기
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += OnLifeTimeChange;                // 플레이어 수명이 변경될 때 실행될 델리게이트에 함수 등록

        vignette.intensity.value = 0;                               // 비네트 값 초기화
    }

    /// <summary>
    /// 플레이어의 수명이 변경될 때 비네트 강도 변경하는 함수
    /// </summary>
    /// <param name="time">현재 플레이어의 수명</param>
    /// <param name="maxTime">플레이어의 최대 수명</param>
    private void OnLifeTimeChange(float time, float maxTime)
    {
        vignette.intensity.value = 1.0f - time/maxTime;             // 비네트의 intensity를 1 -> 0으로 점점 감소시킴
    }
}
