using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int[] highScores;            // 최고 득점들. 0번째가 1등, 4번째가 꼴등(5등)
    public string[] highScorerNames;    // 최득점자 이름. 순서는 위와 같음
}
