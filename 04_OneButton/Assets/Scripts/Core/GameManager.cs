using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public ImageNumber scoreUI;

    Bird player;
    PipeRotator pipeRotator;

    int score = 0;

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

    protected override void Initialize()
    {
        player = FindObjectOfType<Bird>();
        pipeRotator = FindObjectOfType<PipeRotator>();
        pipeRotator?.AddPipeSoredDelegate(AddScore);
    }

    void AddScore(int point)
    {
        Score += point;
    }

    public void TestSetScore(int newScore)
    {
        Score = newScore;
    }
}
