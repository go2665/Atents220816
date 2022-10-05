using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public ImageNumber scoreUI;
    
    PipeRotator pipeRotator;

    int score = 0;
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
