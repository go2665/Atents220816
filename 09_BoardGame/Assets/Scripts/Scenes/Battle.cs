using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    private void Start()
    {
        GameManager manager = GameManager.Inst;

        UserPlayer player = manager.UserPlayer;
        if(!manager.LoadShipDeployData(player))
        {
            player.AutoShipDeployment();
        }
        manager.GameState = GameState.Battle;
    }
}
