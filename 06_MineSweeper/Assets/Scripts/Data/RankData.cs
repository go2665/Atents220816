using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankData : MonoBehaviour
{
    public int rankCount = 5;

    List<int> actionRank;
    List<float> timeRank;

    public List<int> ActionRank => actionRank;
    public List<float> TimeRank => timeRank;

    private void Awake()
    {
        actionRank = new List<int>(rankCount + 1);
        timeRank = new List<float>(rankCount + 1);        
    }

    private void Start()
    {
        LoadData();

        GameManager gameManager = GameManager.Inst;
        gameManager.onGameClear += () =>
        {
            UpdateRank(gameManager.ActionCount, gameManager.PlayTime);
        };
    }

    /// <summary>
    /// ActionRank와 TimeRank 갱신 시도. 새로운 데이터를 랭크에 추가할지 판단 후 정리
    /// </summary>
    /// <param name="actionCount">새로 추가 시도하는 행동 횟수</param>
    /// <param name="playTime">새로 추가 시도하는 플레이 타임</param>
    void UpdateRank(int actionCount, float playTime)
    {
        Debug.Log($"UpdateRank : {actionCount}, {playTime}");
        // actionRank에 actionCount를 추가하고
        // timeRank에 playTime를 추가하고
        // 각각 소팅하고
        // 각각 마지막 노드를 제거

        
        SaveData();
    }


    /// <summary>
    /// 랭킹 정보를 파일로 저장
    /// </summary>
    void SaveData()
    {
        Debug.Log("데이터 세이브");
    }

    /// <summary>
    /// 랭킹 정보를 파일에서 불러오기
    /// </summary>
    void LoadData()
    {
        Debug.Log("데이터 로딩");
    }
}
