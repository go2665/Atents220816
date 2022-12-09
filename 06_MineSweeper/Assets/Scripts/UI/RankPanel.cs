using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameClear += Open;
        gameManager.onGameOver += Open;
        gameManager.onGameReset += Close;
        Close();
    }

    void Open()
    {
        this.gameObject.SetActive(true);
    }

    void Close()
    {
        this.gameObject.SetActive(false);
    }
}
