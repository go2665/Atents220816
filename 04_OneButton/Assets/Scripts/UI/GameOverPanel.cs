using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class GameOverPanel : MonoBehaviour
{
    ResultPanel resultPanel;
    RankPanel rankPanel;
    Button nextButton;
    Button rankButton;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        resultPanel = GetComponentInChildren<ResultPanel>();
        rankPanel = GetComponentInChildren<RankPanel>();
        nextButton = transform.GetChild(2).GetComponent<Button>();
        rankButton = transform.GetChild(3).GetComponent<Button>();

        nextButton.onClick.AddListener(OnClick_Next);
        rankButton.onClick.AddListener(OnClick_Rank);        
    }

    private void Start()
    {
        Close();
        GameManager.Inst.Player.onDead += Open;
    }

    void OnClick_Next()
    {
        if( !rankPanel.InputNameCompleted)
        {
            GameManager temp = GameManager.Inst;
            if (temp != null)
            {
                temp.SetHighScorerName(rankPanel.Rank, $"이름없음{Random.Range(0,10000)}");
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // 현재 열린 씬을 새로 열기
    }

    void OnClick_Rank()
    {
        rankPanel.Open();
    }

    void Open()
    {
        resultPanel.RefreshScore();
        StartCoroutine(OpenDelay());
    }

    void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    IEnumerator OpenDelay()
    {
        yield return new WaitForSeconds(0.1f);

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
