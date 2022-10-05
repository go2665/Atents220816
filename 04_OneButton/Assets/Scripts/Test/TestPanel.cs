using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TestPanel : MonoBehaviour
{
    TMP_InputField inputField;
    Button dieButton;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        dieButton = GetComponentInChildren<Button>();

        inputField.onValueChanged.AddListener(OnInputValueChage);
        dieButton.onClick.AddListener(OnDieButtonClick);
    }

    private void OnInputValueChage(string text)
    {
        // text에 맞게 점수가 변경되게 작성하라.
        int score = 0;
        if(text != "")
        {
            score = int.Parse(text);
        }
        GameManager.Inst.TestSetScore(score);
    }

    private void OnDieButtonClick()
    {
        // 눌러지면 새가 죽는다.
        GameManager.Inst.Player.TestDie();
    }
}
