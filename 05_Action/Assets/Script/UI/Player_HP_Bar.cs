using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_HP_Bar : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI HP_Text;
    string maxHP_Text;
    float maxHP;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        HP_Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;    // 게임 메니저가 가지고 있는 플레이어 가져오기
        maxHP = player.MaxHP;                       // 최대 HP 가져오기
        maxHP_Text = $"/{maxHP:f0}";                // 최대 HP 표시용 글자 만들기
        slider.value = 1;                           // 슬라이더 최대치 만들기
        HP_Text.text = $"{maxHP}/{maxHP}";          // 슬라이더 글자 최대치로 찍기
        player.onHealthChange += OnHealthChange;    // HP가 변경될 때 실행되는 델리게이트에 함수 연결
    }

    /// <summary>
    /// HP가 변경되면 글자를 수정하는 함수
    /// </summary>
    /// <param name="ratio">HP 비율</param>
    private void OnHealthChange(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0, 1);           // 숫자가 넘지는 것을 방지
        slider.value = ratio;                       // 비율에 맞춰 슬라이더 조절

        float hp = maxHP * ratio;                   // 비율을 이용해서 현재 HP 계산
        HP_Text.text = $"{hp:f0}{maxHP_Text}";      // HP 출력
    }
}
