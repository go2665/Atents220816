using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSubPanel : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저가 가지고 있는 랭킹 데이터
    /// </summary>
    RankData rankData;

    /// <summary>
    /// 랭킹 표시를 위한 하부 UI들
    /// </summary>
    RankLine[] rankLines;

    /// <summary>
    /// 어떤 종류의 랭킹을 표시할 것인지를 결정할 enum 타입
    /// </summary>
    public enum RankType
    {
        Action = 0,
        Time
    }

    /// <summary>
    /// 이 서브패널이 어떤 종류의 랭킹을 표시할 것인지를 결정하는 변수
    /// </summary>
    public RankType rankType = RankType.Action;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
    }

    private void Start()
    {
        // 랭킹 종류에 따라 숫자 뒤에 붙을 글자 변경
        string countWord;
        switch (rankType)
        {
            case RankType.Action:
                countWord = "회";
                break;
            case RankType.Time:
                countWord = "초";
                break;
            default:
                countWord = "";
                break;
        }
        foreach(var line in rankLines)
        {
            line.SetCountWord(countWord);
        }

        // 게임 메니저에서 델리게이트 받고 랭킹 데이터 가져오기
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameClear += Refresh;     // onGameClear 신호가 왔을 때 Refresh 함수 실행하도록 등록
        rankData = gameManager.RankData;
        Refresh();
    }

    /// <summary>
    /// 표시할 랭킹 정보를 UI에 갱신
    /// </summary>
    public void Refresh()
    {
        int index = 0; 
        // 랭킹 정보 표시
        switch (rankType)
        {
            case RankType.Action:                
                foreach(var data in rankData.ActionRank)
                {
                    rankLines[index].SetRankAndRecord(index + 1, data);
                    index++;
                }
                break;
            case RankType.Time:
                foreach (var data in rankData.TimeRank)
                {
                    rankLines[index].SetRankAndRecord(index + 1, data);
                    index++;
                }
                break;
            default:
                break;
        }
        // 랭킹이 없는 부분은 빈것으로 처리
        for(int i=index;i<rankLines.Length;i++)
        {
            rankLines[i].ClearLine();
        }
    }
}
