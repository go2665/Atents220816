using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// 타이머가 시작된 이후의 경과 시간
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// 타이머가 재생 중인지 여부
    /// </summary>
    bool isPlay = false;

    /// <summary>
    /// 타이머가 측정한 시간 읽기용 프로퍼티
    /// </summary>
    public float ElapsedTime => elapsedTime;

    /// <summary>
    /// 실제로 보여지는 시간. ElapsedTime의 자연수 부분이 변경되었는지 확인하기 위한 용도.
    /// </summary>
    int visibleTime = 0;

    /// <summary>
    /// 초단위로 시간이 변경되었을 때 실행될 델리게이트
    /// </summary>
    public Action<int> onTimeChange;

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameStart += TimerReset;
        gameManager.onGameStart += Play;
        gameManager.onGameClear += Stop;
        gameManager.onGameOver += Stop;
        gameManager.onGameReset += TimerReset;
    }

    private void Update()
    {
        if(isPlay)  // 플레이 상태일 때만
        {
            elapsedTime += Time.deltaTime;  // 시간 누적하기

            if(visibleTime != (int)elapsedTime)
            {
                visibleTime = (int)elapsedTime;
                onTimeChange?.Invoke(visibleTime);
            }
        }
    }

    /// <summary>
    /// 타이머 시간 측정 시작
    /// </summary>
    public void Play()
    {
        isPlay = true;
    }

    /// <summary>
    /// 타이머 시간 측정 정지
    /// </summary>
    public void Stop()
    {
        isPlay = false;
    }

    /// <summary>
    /// 타이머 초기화 후 정지
    /// </summary>
    public void TimerReset()
    {
        elapsedTime = 0.0f;
        onTimeChange?.Invoke(0);
        isPlay = false;
    }
}
