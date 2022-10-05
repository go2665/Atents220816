using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    ResultPanel resultPanel;
    Button nextButton;
    Button rankButton;

    private void Awake()
    {
        resultPanel = GetComponentInChildren<ResultPanel>();
        nextButton = transform.GetChild(2).GetComponent<Button>();
        rankButton = transform.GetChild(3).GetComponent<Button>();

        nextButton.onClick.AddListener(OnClick_Next);
        rankButton.onClick.AddListener(OnClick_Rank);        
    }

    private void Start()
    {
        gameObject.SetActive(false);
        GameManager.Inst.Player.onDead += Open;
    }

    void OnClick_Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // 현재 열린 씬을 새로 열기
    }

    void OnClick_Rank()
    {

    }

    void Open()
    {
        gameObject.SetActive(true);
    }
}
