using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    public Sprite[] medalSprits;

    ImageNumber score;
    ImageNumber bestScore;
    Image newMarkImage;
    Image medalImage;

    private void Awake()
    {
        score = transform.GetChild(0).GetComponent<ImageNumber>();
        bestScore = transform.GetChild(1).GetComponent<ImageNumber>();
        newMarkImage = transform.GetChild(2).GetComponent<Image>();
        medalImage = transform.GetChild(3).GetComponent<Image>();
    }

    public void RefreshScore()
    {
        int playerScore = GameManager.Inst.Score;
        score.Number = playerScore;                     // 현재 점수 설정
        bestScore.Number = GameManager.Inst.BestScore;  // 최고 점수 설정(새가 죽을 때 최고점수는 자동으로 갱신된다.)

        if ( playerScore >= 400 )
        {
            medalImage.sprite = medalSprits[0];
            medalImage.color = Color.white;
        }
        else if( playerScore >= 300 )
        {
            medalImage.sprite = medalSprits[1];
            medalImage.color = Color.white;
        }
        else if( playerScore >= 200 )
        {
            medalImage.sprite = medalSprits[2];
            medalImage.color = Color.white;
        }
        else if( playerScore >= 100 )
        {
            medalImage.sprite = medalSprits[3];
            medalImage.color = Color.white;
        }
        else
        {
            medalImage.color = Color.clear;
        }
    }
}
