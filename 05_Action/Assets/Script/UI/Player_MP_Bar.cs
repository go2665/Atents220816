using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_MP_Bar : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI MP_Text;
    string maxMP_Text;
    float maxMP;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        MP_Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;    // 게임 메니저가 가지고 있는 플레이어 가져오기
        maxMP = player.MaxMP;                       // 최대 MP 가져오기
        maxMP_Text = $"/{maxMP:f0}";                // 최대 MP 표시용 글자 만들기
        slider.value = 1;                           // 슬라이더 최대치 만들기
        MP_Text.text = $"{maxMP}/{maxMP}";          // 슬라이더 글자 최대치로 찍기
        player.onManaChange += OnManaChange;        // MP가 변경될 때 실행되는 델리게이트에 함수 연결
    }

    /// <summary>
    /// MP가 변경되면 글자를 수정하는 함수
    /// </summary>
    /// <param name="ratio">HP 비율</param>
    private void OnManaChange(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0, 1);           // 숫자가 넘지는 것을 방지
        slider.value = ratio;                       // 비율에 맞춰 슬라이더 조절

        float mp = maxMP * ratio;                   // 비율을 이용해서 현재 HP 계산
        MP_Text.text = $"{mp:f0}{maxMP_Text}";      // MP 출력
    }
}
