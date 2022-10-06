using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : Singleton<GameManager>
{
    ImageNumber scoreUI;

    Bird player;
    PipeRotator pipeRotator;

    int score = 0;
    int bestScore = 0;

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

    protected override void Initialize()
    {
        player = FindObjectOfType<Bird>();
        player.onDead += BestScoreUpdate;       // 새가 죽을 때 최고 점수 갱신 시도

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
            BestScore = Score;
            SaveGameData();
        }
    }

    public void TestSetScore(int newScore)
    {
        Score = newScore;
    }
}
