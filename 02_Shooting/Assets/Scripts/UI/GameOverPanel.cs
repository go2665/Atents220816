using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    bool isShow = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onLifeChange += OnGameOver;
    }

    private void Update()
    {
        if( isShow )
        {
            canvasGroup.alpha += Time.deltaTime;
        }
    }

    void OnGameOver(int lifeCount)
    {
        if( lifeCount <= 0 )
        {
            StartCoroutine(GameOverDelay());
        }
    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(1.0f);
        isShow = true;
    }

}
