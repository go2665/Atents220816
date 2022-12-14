using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSubPanel : MonoBehaviour
{
    RankLine[] rankLines;

    public enum RankType
    {
        Action = 0,
        Time
    }

    public RankType rankType = RankType.Action;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
    }

    private void Start()
    {
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
    }

    public void Refresh()
    {
        RankData rankData = GameManager.Inst.RankData;
        int index = 0; 
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
    }
}
