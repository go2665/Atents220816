using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyPanelUI : MonoBehaviour
{
    TextMeshProUGUI moneyText;

    private void Awake()
    {
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// 플레이어의 돈이 변경될 때 실행될 함수
    /// </summary>
    /// <param name="money">플레이어가 현재 가지고 있는 돈</param>
    public void Refresh(int money)
    {
        moneyText.text = $"{money:N0}";     // N0로 자동으로 3자리마다 콤마 찍기
    }
}
