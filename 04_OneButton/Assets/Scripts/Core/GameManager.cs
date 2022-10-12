using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GameManager : Singleton<GameManager>
{
    ImageNumber scoreUI;

    Bird player;
    PipeRotator pipeRotator;

    int score = 0;
    
    const int RankCount = 5;
    int[] highScores = new int[RankCount];              //0번째 1등, 4번째 꼴등
    string[] highScorerNames = new string[RankCount];

    public Action onBestScoreChange;        // 최고 점수 갱신했을 때 실행될 델리게이트
    public Action onRankRefresh;            // 랭크 화면 갱신 요청
    public Action<int> onRankUpdate;        // 새 기록 추가 알림
    public Action onGameStart;

    public Bird Player => player;
    //public Bird Player { get => player};  // 위와 같은 코드

    public int Score
    {
        get => score;
        private set
        {
            score = value;
            scoreUI.Number = score;
        }
    }

    public int BestScore => highScores[0];

    public int[] HighScores => highScores;
    public string[] HighScorer => highScorerNames;

    protected override void Initialize()
    {
        //Debug.Log("GameManager Initialize");
        onGameStart = null;

        player = FindObjectOfType<Bird>();        
        player.onDead += RankUpdate;            // 새가 죽을 때 랭크 갱신

        pipeRotator = FindObjectOfType<PipeRotator>();
        pipeRotator?.AddPipeScoredDelegate(AddScore);

        scoreUI = GameObject.FindGameObjectWithTag("Score").GetComponent<ImageNumber>();
        Score = 0;

        LoadGameData();
    }

    void AddScore(int point)
    {
        Score += point;
    }

    /// <summary>
    /// 최고 점수와 득점자 이름 저장하기
    /// </summary>
    void SaveGameData()
    {
        // Serializable로 되어 있는 클래스 만들기        
        SaveData saveData = new();                  // 해당 클래스의 인스턴스 만들기
        saveData.highScores = highScores;           // 인스턴스에 데이터 기록
        saveData.highScorerNames = highScorerNames;

        string json = JsonUtility.ToJson(saveData);     // 해당 클래스를 json형식의 문자열로 변경

        string path = $"{Application.dataPath}/Save/";  // 파일을 저장할 폴더를 지정
        if (!Directory.Exists(path))            // 해당 폴더가 없으면
        {
            Directory.CreateDirectory(path);    // 해당 폴더를 새로 만든다.
        }

        string fullPath = $"{path}Save.json";   // 폴더이름과 파일이름을 합쳐서
        File.WriteAllText(fullPath, json);      // 파일에 json형식의 문자열로 변경한 내용을 저장        
    }

    /// <summary>
    /// 최고 점수와 득점자 이름 불러오기
    /// </summary>
    void LoadGameData()
    {
        string path = $"{Application.dataPath}/Save/";      // 경로 확인용
        string fullPath = $"{path}Save.json";               // 전체 경로 확인용
                
        if (Directory.Exists(path) && File.Exists(fullPath)) // 해당 폴더가 있고 파일도 있으면
        {
            string json = File.ReadAllText(fullPath);       // json형식의 데이터 읽기
            SaveData loadData = JsonUtility.FromJson<SaveData>(json);   //SaveData 타입에 맞게 파싱
            highScores = loadData.highScores;               // 읽어온 데이터로 최고점수 기록 변경
            highScorerNames = loadData.highScorerNames;     // 이름들도 가져오기
        }
        else
        {
            highScores = new int[] { 0, 0, 0, 0, 0 };
            highScorerNames = new string[] { "임시 이름1", "임시 이름2", "임시 이름3", "임시 이름4", "임시 이름5" };
        }        
    }

    public void RankUpdate()
    {
        // 뉴마크 표시할지 안할지 결정
        if (BestScore < Score)
        {
            onBestScoreChange?.Invoke();    // 점수가 갱신되면 델리게이트에 연결된 함수들 실행(뉴마크 보이기)
        }

        for (int i=0;i<RankCount;i++)
        {
            if(highScores[i] < Score)   // 한 단계씩 비교해서 Score가 더 크면
            {
                for (int j = RankCount - 1; j > i; j--) // 그 아래 단계는 하나씩 뒤로 밀고
                {
                    highScores[j] = highScores[j - 1];
                    highScorerNames[j] = highScorerNames[j - 1];
                }
                highScores[i] = Score;  // 새 Score 넣기
                //highScorerNames[i] = $"이름 {DateTime.Now.ToString("HH:mm:ss")}";                
                onRankUpdate?.Invoke(i);
                break;
            }
        }        
        onRankRefresh?.Invoke();        // UI 표시 갱신
    }

    public void SetHighScorerName(int rank, string name)
    {
        highScorerNames[rank] = name;   // 실제 데이터 변경
        onRankRefresh?.Invoke();        // UI 표시 갱신
        SaveGameData();                 // 갱신한 점수로 저장
    }

    public void GameStart()
    {
        onGameStart?.Invoke();      // 게임이 시작되었음을 알림   
    }

    public void TestSetScore(int newScore)
    {
        Score = newScore;
    }
}
