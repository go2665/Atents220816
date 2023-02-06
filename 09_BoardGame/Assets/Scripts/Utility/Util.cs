using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /// <summary>
    /// 피셔 예이츠 알고리즘을 통한 셔플 구현
    /// </summary>
    /// <typeparam name="T">int나 float같은 비교 가능한 값타입</typeparam>
    /// <param name="source">데이터의 순서를 섞을 배열</param>
    public static void Shuffle<T>(T[] source)
    {
        // 배열의 오른쪽 끝부터 랜덤으로 선택한 위치의 값과 교환
        // 오른쪽 끝의 교환이 완료된 칸은 랜덤으로 선택하지 않음
        for(int i=source.Length-1; i>-1; i--) 
        {
            int randomIndex = Random.Range(0, i);
            (source[randomIndex], source[i]) = (source[i], source[randomIndex]);
        }
    }    
}
