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
            UpdateActionRank(gameManager.ActionCount);
            UpdateTimeRank(gameManager.PlayTime);
        };
    }

    /// <summary>
    /// ActionRank 갱신 시도. 새로운 데이터(행동횟수)를 랭크에 추가할지 판단 후 정리
    /// </summary>
    /// <param name="data">새로 추가 시도하는 행동 횟수</param>
    void UpdateActionRank(int data)
    {
        // actionRank에 data를 추가하고 소팅한 다음에 마지막 노드를 제거
        Debug.Log($"UpdateActionRank : {data}");

        // 랭킹에 변화가 있으면 
        SaveData();
    }

    /// <summary>
    /// TimeRank 갱신 시도. 새로운 데이터(클리어 시간)을 랭크에 추가할지 판단 후 정리
    /// </summary>
    /// <param name="data">새로 추가 시도하는 클리어 시간</param>
    void UpdateTimeRank(float data)
    {
        // timeRank에 data를 추가하고 소팅한 다음에 마지막 노드를 제거
        Debug.Log($"UpdateTimeRank : {data}");

        // 랭킹에 변화가 있으면 
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
