using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class GameData : Singleton<GameData>
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;
    public BackgroundScroller ground;
    public Spawner spawner;
    public RunningMan player;

    public float baseSpeed = 8.0f;

    float increasSpeed = 0.5f;  // 1초에 속도가 0.5만큼 증가
    float currentSpeed = 0.0f;

    float score = 0;
    int hiScore = 0;
    public float scorePerSec = 10.0f;

    private void Start()
    {
        LoadGame();
        player.onDead += GameOver;
        currentSpeed = baseSpeed;
        score = 0;
    }

    private void GameOver()
    {
        Time.timeScale = 0.0f;
        if( score > hiScore)
        {
            SaveGame();
        }
    }

    private void Update()
    {
        score += scorePerSec * Time.deltaTime;
        scoreText.text = $"{score:f0}";

        currentSpeed += increasSpeed * Time.deltaTime;

        ground.scrollingSpeed = currentSpeed;
        spawner.UpdateSpeed(currentSpeed);
    }

    void SaveGame()
    {
        // Serializable로 되어 있는 클래스 만들기        
        RunningSaveData saveData = new();        
        saveData.highScore = (int)score;

        string json = JsonUtility.ToJson(saveData);     // 해당 클래스를 json형식의 문자열로 변경

        string path = $"{Application.dataPath}/Save/";  // 파일을 저장할 폴더를 지정
        if (!Directory.Exists(path))            // 해당 폴더가 없으면
        {
            Directory.CreateDirectory(path);    // 해당 폴더를 새로 만든다.
        }

        string fullPath = $"{path}SaveRunning.json";   // 폴더이름과 파일이름을 합쳐서
        File.WriteAllText(fullPath, json);      // 파일에 json형식의 문자열로 변경한 내용을 저장        
    }

    void LoadGame()
    {
        string path = $"{Application.dataPath}/Save/";      // 경로 확인용
        string fullPath = $"{path}SaveRunning.json";        // 전체 경로 확인용

        if (Directory.Exists(path) && File.Exists(fullPath)) // 해당 폴더가 있고 파일도 있으면
        {
            string json = File.ReadAllText(fullPath);       // json형식의 데이터 읽기
            RunningSaveData loadData = JsonUtility.FromJson<RunningSaveData>(json);   //SaveData 타입에 맞게 파싱
            hiScore = loadData.highScore;                   // 읽어온 데이터로 최고점수 기록 변경
        }
        else
        {
            hiScore = 0;
        }

        hiScoreText.text = $"{hiScore}";
    }
}
