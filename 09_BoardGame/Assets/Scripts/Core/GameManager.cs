using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 플레이어 ------------------------------------------------------------------------------------
    private UserPlayer userPlayer;
    private EnemyPlayer enemyPlayer;


    public UserPlayer UserPlayer => userPlayer;
    public EnemyPlayer EnemyPlayer => enemyPlayer;

    // 게임 상태 -----------------------------------------------------------------------------------
    
    /// <summary>
    /// 게임의 현재 상태. 각 씬의 Start에서 변경.
    /// </summary>
    private GameState gameState = GameState.Title;

    /// <summary>
    /// 게임의 상태를 확인하고 설정할 수 있는 프로퍼티
    /// </summary>
    public GameState GameState
    {
        get => gameState;
        set
        {
            if (gameState != value)                 // 변경이 있을 때만 실행
            {
                gameState = value;
                onStateChange?.Invoke(gameState);   // 상태 변경이 일어나면 플레이어들에게 알림
            }
        }
    }

    /// <summary>
    /// 게임의 상태가 변경될 때 실행될 델리게이트
    /// </summary>
    Action<GameState> onStateChange;

    /// <summary>
    /// 게임이 종료되었음을 확인할 수 있는 프로퍼티
    /// </summary>
    public bool IsGameEnd => UserPlayer.IsDepeat || enemyPlayer.IsDepeat;

    // 입력 -----------------------------------------------------------------------------------------
    InputController input;
    public InputController Input => input;

    // 함선 배치 정보 -----------------------------------------------------------------------------------
    private ShipDeployData[] shipDeployDatas;


    // 함수 -----------------------------------------------------------------------------------------

    /// <summary>
    /// 씬이 처음 로드 되었을 때만 실행되는 함수
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        input = GetComponent<InputController>();
    }

    /// <summary>
    /// 씬이 로드될 때마다 실행되는 데이터 리셋용 함수
    /// </summary>
    protected override void ManagerDataReset()
    {
        userPlayer = FindObjectOfType<UserPlayer>();
        enemyPlayer = FindObjectOfType<EnemyPlayer>();

        onStateChange = null;   // 씬이 새로 로딩이 되면 이전에 연결되어있던 플레이어 함수들은 사용할 수 없게 되므로 초기화 처리
        if (userPlayer != null)
        {
            onStateChange += userPlayer.OnStateChange;
        }
        if (enemyPlayer != null)
        {
            onStateChange += enemyPlayer.OnStateChange;
        }
    }

    // 함선 배치 정보 저장 및 로딩용 함수들 ----------------------------------------------------------------------

    public void SaveShipDeployData(PlayerBase targetPlayer)
    {
        shipDeployDatas = new ShipDeployData[targetPlayer.Ships.Length];
        for(int i=0;i<shipDeployDatas.Length;i++)
        {
            shipDeployDatas[i] = new ShipDeployData();
            shipDeployDatas[i].direction = targetPlayer.Ships[i].Direction;
            shipDeployDatas[i].position = targetPlayer.Ships[i].Positions[0];
        }
    }

    public bool LoadShipDeployData(PlayerBase targetPlayer)
    {
        bool result = false;
        if( shipDeployDatas != null )
        {
            targetPlayer.UndoAllShipDeployment();
            for(int i=0;i<shipDeployDatas.Length;i++)
            {
                Ship targetShip = targetPlayer.Ships[i];
                targetShip.Direction = shipDeployDatas[i].direction;
                targetPlayer.Board.ShipDeplyment(targetShip, shipDeployDatas[i].position);
                targetShip.gameObject.SetActive(true);
            }
        }

        return result;

    }
    
}
