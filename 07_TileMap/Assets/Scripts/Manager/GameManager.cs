using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    /// <summary>
    /// 게임의 전체 맵을 관리하는 매니저
    /// </summary>
    MapManager mapManager;

    public MapManager MapManager => mapManager;

    protected override void Initialize()
    {
        base.Initialize();

        mapManager = GetComponent<MapManager>();    // 맵 매니저 찾아서
        mapManager.Initialize();                    // 초기화 하기
    }

    /// <summary>
    /// 씬이 로드 될 때마다 반복적으로 다시 설정해야 할 것들
    /// </summary>
    protected override void ManagerDataReset()
    {
        base.ManagerDataReset();
        player = FindObjectOfType<Player>();    // 씬을 나가면 플레이어가 사라지니 다시 찾기
        mapManager.MapDataReset();              // 데이터 초기화 및 플레이어 주변맵 로딩하기
    }
}
