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
    const float turnDurationTime = 5.0f;

    // 턴이 시작되고 일정 시간이 지나면 턴이 종료 된다
    // 두 플레이어가 모두 행동을 완료하면 턴이 종료된다.
    // 두 플레이어는 턴이 시작될 때와 종료될 때 해야할 일이 있다.

    protected override void Initialize()
    {
        base.Initialize();

        turnNumber = 0;
        isTurnEnd = true;
    }

    /// <summary>
    /// 턴이 시작될 때 실행되는 함수
    /// </summary>
    void OnTurnStart()
    {
        Debug.Log($"{turnNumber} 턴 시작");

        isTurnEnd = false;
        turnRemainTime = turnDurationTime;
    }

    /// <summary>
    /// 턴이 종료될 때 실행되는 함수
    /// </summary>
    void OnTurnEnd()
    {
        isTurnEnd = true;

        Debug.Log($"{turnNumber} 턴 종료");
        turnNumber++;
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
