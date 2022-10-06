using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI nameText;

    private void Awake()
    {
        scoreText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetData(int score, string name)
    {
        scoreText.text = score.ToString();
        nameText.text = name;
    }
}
