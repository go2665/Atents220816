using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI timeText;

    float currentTime = 0.0f;
    bool isStart = false;

    float CurrentTime
    {
        get => currentTime;
        set
        {
            currentTime = value;
            timeText.text = $"{currentTime:f2} 초";
        }
    }

    public float ResultTime { get => currentTime; }

    private void Awake()
    {
        timeText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Goal goal = FindObjectOfType<Goal>();
        goal.onGoalIn += StopTimer;

        CurrentTime = 0.0f;
        GameManager.Inst.onGameStart += StartTimer;

        //StartTimer();
    }

    private void Update()
    {
        if( isStart )
        {
            CurrentTime += Time.deltaTime;
        }
    }

    void StartTimer()
    {
        isStart = true;
        CurrentTime = 0.0f;
    }

    void StopTimer()
    {
        isStart = false;
    }
}
