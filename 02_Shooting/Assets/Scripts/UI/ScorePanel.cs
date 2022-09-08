using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePanel : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    int targetScore = 0;            // 목표값
    float currentScore = 0.0f;      // 현재값

    private void Awake()
    {
        Transform panel = transform.GetChild(0);
        Transform point = panel.GetChild(1);
        scoreText = point.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onScoreChange += SetTargetScore;

        targetScore = 0;
        currentScore = 0.0f;
    }

    private void Update()
    {
        // 1 프레임 : 화면에 그릴 그림 한장만들어서 보여주는 단위
        if( currentScore < targetScore )
        {
            currentScore += Time.deltaTime;

            currentScore = Mathf.Min(currentScore, targetScore);    //currentScore가 targetScore보다 무조건 작거나 같도록 변경.
            scoreText.text = $"{currentScore:f0}";
        }
    }

    private void SetTargetScore(int newScore)
    {
        targetScore = newScore;
    }
}
