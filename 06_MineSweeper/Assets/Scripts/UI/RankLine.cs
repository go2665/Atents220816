using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI rank;
    TextMeshProUGUI record;
    TextMeshProUGUI countWord;
    
    /// <summary>
    /// 순위와 기록을 텍스트로 출력하는 함수
    /// </summary>
    /// <param name="rank">순위</param>
    /// <param name="record">기록</param>
    public void SetRankAndRecord(int rank, int record)
    {

    }

    /// <summary>
    /// 순위와 기록을 텍스트로 출력하는 함수
    /// </summary>
    /// <param name="rank">순위</param>
    /// <param name="record">기록</param>
    public void SetRankAndRecord(int rank, float record)
    {

    }

    /// <summary>
    /// 갯수를 나타내는 말을 변경하는 함수
    /// </summary>
    /// <param name="str">갯수를 나타내는 말. ex) 개, 초, 회 등등</param>
    public void SetCountWord(string str)
    {

    }
}
