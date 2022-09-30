using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] numberImages = new Sprite[10];

    Image[] digits;

    private void Awake()
    {
        digits = new Image[transform.childCount];
        for(int i=0;i<transform.childCount;i++)
        {
            digits[i] = transform.GetChild(i).GetComponent<Image>();
        }
    }

    int number;
    public int Number
    {
        get => number;
        set
        {
            number = value;

            //123
            // (123 / 1) % 10 = 3
            // (123 / 10) % 10 = 2
            // (123 / 100) % 10 = 1

            int mod = number % 10;
            digits[0].sprite = numberImages[mod];
        }
    }
}
