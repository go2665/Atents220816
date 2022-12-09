using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InfoUI : MonoBehaviour
{
    TextMeshProUGUI actionCountText;
    TextMeshProUGUI findCountText;
    TextMeshProUGUI notFindCountText;

    private void Awake()
    {
        Transform tempTransform = transform.GetChild(3);
        actionCountText = tempTransform.GetComponent<TextMeshProUGUI>(); // 컴포넌트 찾기
        tempTransform = transform.GetChild(4);
        findCountText = tempTransform.GetComponent<TextMeshProUGUI>();
        tempTransform = transform.GetChild(5);
        notFindCountText = tempTransform.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onActionCountChange += OnActionCountChange; // 게임 메니저의 델리게이트에 함수 등록
        OnActionCountChange(0);                                 // 첫번째 UI 초기화

        gameManager.onGameOver += () => OnGameEnd(gameManager.Board.FoundMineCount, gameManager.Board.NotFoundMineCount);
        gameManager.onGameClear += () => OnGameEnd(gameManager.Board.FoundMineCount, gameManager.Board.NotFoundMineCount);
        gameManager.onGameReset += () =>
        {
            findCountText.text = "???";
            notFindCountText.text = "???";
        };
    }

    /// <summary>
    /// ActionCount UI에 표시되는 글자 변경하는 함수
    /// </summary>
    /// <param name="count">표시될 회수</param>
    private void OnActionCountChange(int count)
    {
        actionCountText.text = $"{count}";
    }

    private void OnGameEnd(int findCount, int notFindCount)
    {
        findCountText.text = findCount.ToString();
        notFindCountText.text = $"{notFindCount}";
    }
}
