using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines;
    TMP_InputField inputField;
    CanvasGroup canvasGroup;

    int rank;

    bool inputNameCompleted = false;

    public bool InputNameCompleted => inputNameCompleted;
    public int Rank => rank;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.gameObject.SetActive(false);
        inputField.onEndEdit.AddListener(OnNameInputEnd);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        GameManager.Inst.onRankRefresh += RankDataRefresh;  // 화면 갱신만
        GameManager.Inst.onRankUpdate += EnableNameInput;   // 이름을 입력 받을 수 있게 하기
        Close();
        RankDataRefresh();
    }

    private void OnDisable()
    {
        GameManager temp = GameManager.Inst;
        if (temp != null)
        {
            temp.onRankRefresh -= RankDataRefresh;
            temp.onRankUpdate -= EnableNameInput;
        }
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void RankDataRefresh()
    {
        for(int i=0;i<rankLines.Length;i++)
        {
            rankLines[i].SetData(GameManager.Inst.HighScores[i], GameManager.Inst.HighScorer[i]);
        }
    }

    private void EnableNameInput(int index)
    {
        inputNameCompleted = false;

        Open();
        rank = index;
        inputField.transform.position = 
            new Vector3(inputField.transform.position.x, 
            rankLines[rank].transform.position.y, 
            inputField.transform.position.z);
        inputField.gameObject.SetActive(true);
    }

    private void OnNameInputEnd(string text)
    {
        //Debug.Log($"Input text : {text}");
        GameManager temp = GameManager.Inst;
        if (temp != null)
        {
            temp.SetHighScorerName(rank, text);
        }
        inputField.gameObject.SetActive(false);
        inputNameCompleted = true;
    }
}
