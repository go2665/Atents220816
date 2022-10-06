using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
    }

    private void Start()
    {
        GameManager.Inst.onRankChange += RankDataUpdate;
    }

    private void OnDisable()
    {
        GameManager temp = GameManager.Inst;
        if (temp != null)
        {
            temp.onRankChange -= RankDataUpdate;
        }
    }

    void RankDataUpdate()
    {
        for(int i=0;i<rankLines.Length;i++)
        {
            rankLines[i].SetData(GameManager.Inst.HighScores[i], GameManager.Inst.HighScorer[i]);
        }
    }
}
