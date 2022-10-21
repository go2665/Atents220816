using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HP_Bar : MonoBehaviour
{
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onHealthChange += OnHealthChange;
    }

    private void OnHealthChange(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0, 1);
        slider.value = ratio;
    }
}
