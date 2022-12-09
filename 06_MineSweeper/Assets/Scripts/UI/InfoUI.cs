using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InfoUI : MonoBehaviour
{
    TextMeshProUGUI actionCountText;

    private void Awake()
    {
        Transform actionCountTransform = transform.GetChild(3);
        actionCountText = actionCountTransform.GetComponent<TextMeshProUGUI>(); // 컴포넌트 찾기
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onActionCountChange += OnActionCountChange; // 게임 메니저의 델리게이트에 함수 등록
        OnActionCountChange(0);                                 // 첫번째 UI 초기화
    }

    /// <summary>
    /// ActionCount UI에 표시되는 글자 변경하는 함수
    /// </summary>
    /// <param name="count">표시될 회수</param>
    private void OnActionCountChange(int count)
    {
        actionCountText.text = $"{count} 회";
    }
}
