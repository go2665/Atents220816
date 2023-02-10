using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private UserPlayer userPlayer;
    private EnemyPlayer enemyPlayer;

    public UserPlayer UserPlayer => userPlayer;
    public EnemyPlayer EnemyPlayer => enemyPlayer;

    protected override void Initialize()
    {
        base.Initialize();
        userPlayer = FindObjectOfType<UserPlayer>();
        enemyPlayer = FindObjectOfType<EnemyPlayer>();
    }
}
