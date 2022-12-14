using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI rank;
    TextMeshProUGUI record;
    TextMeshProUGUI countWord;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        rank = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        record = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        countWord = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 순위와 기록을 텍스트로 출력하는 함수
    /// </summary>
    /// <param name="rankData">순위</param>
    /// <param name="recordData">기록</param>
    public void SetRankAndRecord<T>(int rankData, T recordData)
    {
        rank.text = $"{rankData}등";
        record.text = $"{recordData:N0}";
        countWord.enabled = true;
    }

    /// <summary>
    /// 갯수를 나타내는 말을 변경하는 함수
    /// </summary>
    /// <param name="str">갯수를 나타내는 말. ex) 개, 초, 회 등등</param>
    public void SetCountWord(string str)
    {
        countWord.text = str;
    }

    /// <summary>
    /// 이 RankLine을 안보이게 만드는 함수
    /// </summary>
    public void ClearLine()
    {
        rank.text = "";
        record.text = "";
        countWord.enabled = false;
    }
}
