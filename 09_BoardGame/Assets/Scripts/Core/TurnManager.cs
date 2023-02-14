using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    /// <summary>
    /// 현재 턴 번호
    /// </summary>
    int turnNumber = 0;

    /// <summary>
    /// 현재 턴이 종료되었는지 여부(true면 종료. false면 아직 진행중)
    /// </summary>
    bool isTurnEnd = true;

    /// <summary>
    /// 타임 아웃으로 턴이 종료될 때 까지 남은 시간
    /// </summary>
    float turnRemainTime = 0.0f;

    /// <summary>
    /// 한 턴이 타임 아웃되는데 걸리는 시간
    /// </summary>
    public const float turnDurationTime = 5.0f;

    /// <summary>
    /// 유저 플레이어
    /// </summary>
    PlayerBase userPlayer;

    /// <summary>
    /// 적(CPU) 플레이어
    /// </summary>
    PlayerBase enemyPlayer;

    /// <summary>
    /// 턴이 시작될 때 실행될 델리게이트. 파라메터는 현재 턴 번호
    /// </summary>
    Action<int> onTurnStart;

    /// <summary>
    /// 턴이 종료될 때 실행될 델리게이트
    /// </summary>
    Action onTurnEnd;
    
    /// <summary>
    /// 초기화용 함수. 씬 로드가 완료 될 때마다 실행(Awake와 Start 사이에서 실행됨)
    /// </summary>
    protected override void ManagerDataReset()
    {
        // 필요 변수들 초기화
        turnNumber = 0;
        isTurnEnd = true;
        userPlayer = FindObjectOfType<UserPlayer>();
        enemyPlayer = FindObjectOfType<EnemyPlayer>();

        // 턴 시작/종료 델리게이트에 플레이어들의 턴 시작/종료 함수들 연결
        onTurnStart = null; 
        onTurnEnd = null;
        onTurnStart += userPlayer.OnPlayerTurnStart;
        onTurnStart += enemyPlayer.OnPlayerTurnStart;
        onTurnEnd += userPlayer.OnPlayerTurnEnd;
        onTurnEnd += enemyPlayer.OnPlayerTurnEnd;

        // 유저 플레이어의 행동이 완료되면 적 플레이어의 행동 완료 여부를 체크해서 적의 행동도 완료 되었으면 턴 종료 실행
        userPlayer.onActionEnd += () =>
        {
            if (enemyPlayer.IsActionDone && !userPlayer.IsDepeat)  // 추가로 유저가 살아있을 때만 턴 종료
            {
                OnTurnEnd();
            }
        };
        // 적 플레이어의 행동이 완료되면 유저 플레이어의 행동 완료 여부를 체크해서 유저의 행동도 완료 되었으면 턴 종료 실행
        enemyPlayer.onActionEnd += () =>
        {
            if (userPlayer.IsActionDone && !enemyPlayer.IsDepeat)   // 추가로 적이 살아있을 때만 턴 종료
            {
                OnTurnEnd();
            }
        }; ;
    }

    /// <summary>
    /// 턴이 시작될 때 실행되는 함수
    /// </summary>
    void OnTurnStart()
    {
        Debug.Log($"{turnNumber} 턴 시작");

        isTurnEnd = false;
        turnRemainTime = turnDurationTime;

        onTurnStart?.Invoke( turnNumber );
    }

    /// <summary>
    /// 턴이 종료될 때 실행되는 함수
    /// </summary>
    void OnTurnEnd()
    {
        // !(UserPlayer.IsDepeat || enemyPlayer.IsDepeat)
        if (!enemyPlayer.IsDepeat && !userPlayer.IsDepeat)
        {
            onTurnEnd?.Invoke();

            isTurnEnd = true;

            Debug.Log($"{turnNumber} 턴 종료");
            turnNumber++;
        }
        else
        {
            turnRemainTime = float.MaxValue;    // OnTurnEnd가 과하게 호출되는 것 방지
        }
    }

    private void Update()
    {
        // 턴의 타임아웃 체킹
        turnRemainTime -= Time.deltaTime;
        if(turnRemainTime < 0)
        {
            OnTurnEnd();        // 타임 아웃이 되면 턴 종료
        }

        // 현재 턴이 종료되었는지 확인
        if ( isTurnEnd )
        {
            OnTurnStart();      // 종료되었으면 다음 턴 시작
        }
    }
}
