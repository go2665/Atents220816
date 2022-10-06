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
    int bestScore = 0;

    const int RankCount = 5;
    int[] highScores = new int[RankCount];              //0번째 1등, 4번째 꼴등
    string[] highScorerNames = new string[RankCount];

    public Action onBestScoreChange;        // 최고 점수 갱신했을 때 실행될 델리게이트
    public Action onRankChange;

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

    public int BestScore
    {
        get => bestScore;
        private set => bestScore = value;        
    }

    public int[] HighScores => highScores;
    public string[] HighScorer => highScorerNames;

    protected override void Initialize()
    {
        player = FindObjectOfType<Bird>();
        player.onDead += BestScoreUpdate;       // 새가 죽을 때 최고 점수 갱신 시도
        player.onDead += RankUpdate;            // 새가 죽을 때 랭크 갱신

        pipeRotator = FindObjectOfType<PipeRotator>();
        pipeRotator?.AddPipeSoredDelegate(AddScore);

        scoreUI = GameObject.FindGameObjectWithTag("Score").GetComponent<ImageNumber>();

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
        SaveData saveData = new();              // 해당 클래스의 인스턴스 만들기
        saveData.bestScore = BestScore;         // 인스턴스에 데이터 기록
        saveData.name = "임시 이름";

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

        BestScore = 0;  // 기본 값 설정
        if (Directory.Exists(path) && File.Exists(fullPath)) // 해당 폴더가 있고 파일도 있으면
        {
            string json = File.ReadAllText(fullPath);       // json형식의 데이터 읽기
            SaveData loadData = JsonUtility.FromJson<SaveData>(json);   //SaveData 타입에 맞게 파싱
            BestScore = loadData.bestScore;                 // 읽어온 데이터로 BestScore 변경
        }
    }

    void BestScoreUpdate()
    {
        if(BestScore < Score)
        {
            BestScore = Score;              // 점수 갱신
            onBestScoreChange?.Invoke();    // 점수가 갱신되면 델리게이트에 연결된 함수들 실행
            SaveGameData();                 // 갱신한 점수로 저장
        }
    }

    public void RankUpdate()
    {
        for(int i=0;i<RankCount;i++)
        {
            if(highScores[i] < Score)   // 한 단계씩 비교해서 Score가 더 크면
            {
                for (int j = RankCount - 1; j > i; j--) // 그 아래 단계는 하나씩 뒤로 밀고
                {
                    highScores[j] = highScores[j - 1];
                    highScorerNames[j] = highScorerNames[j - 1];
                }
                highScores[i] = Score;  // 새 Score 넣기
                highScorerNames[i] = "";

                onRankChange?.Invoke();
                break;
            }
        }        
    }

    public void TestSetScore(int newScore)
    {
        Score = newScore;
    }
}
