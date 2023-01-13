using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Logger logger;
    public Logger Logger => logger;


    protected override void Initialize()
    {
        base.Initialize();
        logger = FindObjectOfType<Logger>();        
    }

}
