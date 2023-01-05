using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 게임의 전체 맵을 관리하는 매니저
    /// </summary>
    MapManager mapManager;

    public MapManager MapManager => mapManager;

    protected override void Initialize()
    {        
        mapManager = GetComponent<MapManager>();    // 맵 매니저 찾아서
        mapManager.Initialize();                    // 초기화 하기

        base.Initialize();
    }
}
