using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCounter : MonoBehaviour
{
    ImageNumber imageNumber;

    private void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameReset += () => Refresh(gameManager.FlagCount);
        gameManager.onFlagCountChange += Refresh;
        Refresh(gameManager.FlagCount);
    }

    private void Refresh(int count)
    {
        imageNumber.Number = count;
    }
}
