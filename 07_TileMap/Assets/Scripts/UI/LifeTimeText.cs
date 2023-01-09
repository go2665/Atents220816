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

        textUI.text = $"{player.maxLifeTime:F2} Sec";
    }

    private void OnLifeTimeChange(float time, float _)
    {
        textUI.text = $"{time:F2} Sec";
    }
}
