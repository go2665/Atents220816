using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    /// <summary>
    /// 숫자 스프라이트를 모아 놓은 배열
    /// </summary>
    public Sprite[] numbers;

    /// <summary>
    /// 숫자가 보여질 이미지 배열
    /// </summary>
    private Image[] numberImages;

    /// <summary>
    /// 표현할 숫자
    /// </summary>
    private int number;

    /// <summary>
    /// 숫자를 변경하고 적용할 프로퍼티
    /// </summary>
    public int Number
    {
        get => number;
        set 
        {
            if( number != value )                       // 값이 변경되면
            {
                number = Mathf.Clamp(value, -99, 999);  // 숫자 범위를 -99~999로 조정하고
                RefreshNumberImage();                   // 이미지 갱신
            }
        }
    }

    /// <summary>
    /// 가독성을 위해 0을 표시하는 스프라이트를 돌려주는 프로퍼티
    /// </summary>
    Sprite ZeroSprite => numbers[0];

    /// <summary>
    /// 가독성을 위해 -를 표시하는 스프라이트를 돌려주는 프로퍼티
    /// </summary>
    Sprite MinusSprite => numbers[10];

    /// <summary>
    /// 가독성을 위해 빈칸을 표시하는 스프라이트를 돌려주는 프로퍼티
    /// </summary>
    Sprite EmptySprite => numbers[11];

    private void Awake()
    {
        // 숫자를 가져올 컴포넌트 모두 가져오기
        numberImages = GetComponentsInChildren<Image>();
    }

    /// <summary>
    /// 표시하는 이미지를 number 변수에 맞게 변경하는 함수
    /// </summary>
    private void RefreshNumberImage()
    {
        int tempNum = Mathf.Abs(number);        // 부호 제거. 무조건 +로 변경.
        Queue<int> digitsQ = new Queue<int>(3); // 각 자리수 숫자를 저장할 큐 만들기
        while(tempNum > 0)                      // 각 자리수별로 숫자를 잘라서 digitsQ에 저장하기
        {
            digitsQ.Enqueue(tempNum % 10);      //  마지막 자리 수 찾기
            tempNum /= 10;                      //  마지막 자리 수 잘라내기
        }

        int index = 0;                                  // 적용할 자리수 확인용
        while (digitsQ.Count > 0)                       // 자리 수 별로 알맞은 이미지로 변경
        {
            int num = digitsQ.Dequeue();                // 순서대로 숫자 꺼내기
            numberImages[index].sprite = numbers[num];  // 꺼낸 숫자에 맞는 이미지로 변경하기

            index++;
        }

        for(int i=index; i<numberImages.Length;i++)     // 적용하고 남은 자리수들의 숫자를 빈칸으로 채우기
        {
            numberImages[i].sprite = EmptySprite;
        }

        if(number < 0)      // 음수였을 경우
        {
            numberImages[numberImages.Length - 1].sprite = MinusSprite; // 제일 왼쪽칸에 - 표시하기
        }
    }
}
