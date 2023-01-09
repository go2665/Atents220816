using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public float alphaChangeSpeed = 2.0f;   // 1초에 2까지 변경되는 속도(0.5초면 1이 된다.)

    CanvasGroup canvasGroup;
    TextMeshProUGUI totalPlayTimeText;
    Button restartButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Transform child = transform.GetChild(1);
        totalPlayTimeText = child.GetComponent<TextMeshProUGUI>();
        restartButton = GetComponentInChildren<Button>();
        restartButton.onClick.AddListener(OnRestartClick);
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;

        player.onDie += OnPlayerDie;
    }

    private void OnPlayerDie(float totalPlayTime)
    {
        StartCoroutine(StartAlphaChange());

        totalPlayTimeText.text = $"Total Play Time\r\n< {Mathf.RoundToInt(totalPlayTime)} sec >";

    }

    IEnumerator StartAlphaChange()
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
    }

    private void OnRestartClick()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
