using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines;
    TMP_InputField inputField;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.gameObject.SetActive(false);
        //inputField.onEndEdit
    }

    private void Start()
    {
        GameManager.Inst.onRankRefresh += RankDataRefresh;  // 화면 갱신만
        GameManager.Inst.onRankUpdate += EnableNameInput;   // 이름을 입력 받을 수 있게 하기
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

    void RankDataRefresh()
    {
        for(int i=0;i<rankLines.Length;i++)
        {
            rankLines[i].SetData(GameManager.Inst.HighScores[i], GameManager.Inst.HighScorer[i]);
        }
    }

    private void EnableNameInput(int index)
    {
        inputField.transform.position = 
            new Vector3(inputField.transform.position.x, 
            rankLines[index].transform.position.y, 
            inputField.transform.position.z);
        inputField.gameObject.SetActive(true);
    }
}
