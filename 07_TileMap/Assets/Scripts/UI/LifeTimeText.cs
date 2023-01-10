using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeTimeText : MonoBehaviour
{
    TextMeshProUGUI textUI;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += OnLifeTimeChange;

        textUI.text = $"{player.maxLifeTime:F2} Sec";   // 플레이어의 수명을 텍스트로 표시(소수점 둘째 자리까지만 출력)
    }

    private void OnLifeTimeChange(float time, float _)
    {
        textUI.text = $"{time:F2} Sec";                 // 수명 변경되면 텍스트 업데이트
    }
}
