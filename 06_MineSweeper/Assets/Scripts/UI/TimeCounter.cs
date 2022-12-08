using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    Timer timer;
    ImageNumber imageNumber;

    private void Awake()
    {
        timer = GetComponent<Timer>();
        imageNumber = GetComponent<ImageNumber>();

        timer.onTimeChange += Refresh;
    }

    private void Refresh(int count)
    {
        imageNumber.Number = count;
    }
}
