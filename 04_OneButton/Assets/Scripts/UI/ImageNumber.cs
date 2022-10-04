using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public int expectedLength = 6;
    public GameObject digitPrefab;
    public Sprite[] numberImages = new Sprite[10];

    List<Image> digits; // 0번째가 1자리, 1번째가 10자리

    private void Awake()
    {
        digits = new List<Image>(transform.childCount);
        for(int i=0;i<transform.childCount;i++)
        {
            digits.Add(transform.GetChild(i).GetComponent<Image>());
        }
    }

    int number;
    public int Number
    {
        get => number;
        set
        {
            if (number != value)    // 값이 변경되었을 때만 실행하라.
            {
                int tempNum = value;
                List<int> remainders = new List<int>(expectedLength);    // 자료구조를 만들 때는 기본 크기를 잡아주는 편이 좋다.

                // 자리수별로 숫자 분리하기
                while(tempNum != 0)                 //123으로 시작했을 때
                {
                    remainders.Add(tempNum % 10);   // 3 -> 2 -> 1
                    tempNum /= 10;                  // 12 -> 1 -> 0
                }

                // 자리수에 맞게 digits 증가 또는 감소
                int diff = remainders.Count - digits.Count;
                if( diff > 0 )
                {
                    // digits 증가. 나머지들의 자리수가 더 기니까
                    for (int i = 0; i < diff; i++)
                    {
                        GameObject obj = Instantiate(digitPrefab, transform);
                        obj.name = $"Digit{Mathf.Pow(10, digits.Count)}";  // 1자리는 1, 10자리는 10, 100자리는 100이 이름에 들어가도록 완성하기
                        digits.Add(obj.GetComponent<Image>());
                    }
                }
                else if( diff < 0 )
                {
                    // digits 감소. 나머지들의 자리수가 더 작으니까
                    for(int i = digits.Count + diff ; i<digits.Count ; i++)
                    {
                        digits[i].gameObject.SetActive(false);
                    }
                }

                // 자리수별로 숫자 설정
                for(int i=0;i<remainders.Count;i++)
                {
                    digits[i].sprite = numberImages[remainders[i]];
                    digits[i].gameObject.SetActive(true);
                }

                number = value;
            }
        }
    }
}
