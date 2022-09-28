using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    TextMeshProUGUI resultText; // 결과가 출력될 text
    Button button;              // 다음 스테이로 넘어가기 위한 버튼

    float clearTime = 0.0f;     // 클리어하는데 걸린 시간(타이머에서 받아온다.)
    public float ClearTime 
    { 
        get => clearTime;
        set
        {
            clearTime = value;
            resultText.text = $"클리어하는데 {clearTime:f2}초 걸렸습니다.";
        }
    }

    private void Awake()
    {
        resultText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        button = GetComponentInChildren<Button>();       
    }

    private void Start()
    {
        Goal goal = FindObjectOfType<Goal>();
        button.onClick.AddListener(goal.GoNextStage);   // 버튼에 함수 연결
    }
}
