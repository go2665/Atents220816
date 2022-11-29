#define TEST_CODE

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Timer timer;

    private int flagCount = 0;
    private int timeCount = 0;

    public int FlagCount
    {
        get => flagCount;
        private set
        {
            flagCount = value;
            onFlagCountChange?.Invoke(flagCount);
        }
    }

    public int TimeCount
    {
        get => timeCount;
        private set
        {
            if (timeCount != value)
            {
                timeCount = value;
                onTimeCountChange?.Invoke(timeCount);
            }
        }
    }

    public Action<int> onFlagCountChange;
    public Action<int> onTimeCountChange;

    protected override void Initialize()
    {
        base.Initialize();
        timer = GetComponent<Timer>();
    }

    private void Update()
    {
        TimeCount = (int)timer.ElapsedTime;
    }

#if TEST_CODE
    public void TestTimer_Play()
    {
        timer.Play();
    }

    public void TestTimer_Stop()
    {
        timer.Stop();
    }

    public void TestTimer_Reset()
    {
        timer.TimerReset();
    }

    public void TestFlag_Increase()
    {
        FlagCount++;
    }

    public void TestFlag_Decrease()
    {
        FlagCount--;
    }
#endif
}
