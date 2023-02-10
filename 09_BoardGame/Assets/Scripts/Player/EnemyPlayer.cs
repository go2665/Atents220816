using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : PlayerBase
{
    public float thinkingTimeMin = 1.0f;
    public float thinkingTimeMax = TurnManager.turnDurationTime - 1.0f;

    protected override void Start()
    {
        base.Start();
        opponent = GameManager.Inst.UserPlayer; // 상대방 설정

        AutoShipDeployment();
    }

    public override void OnPlayerTurnStart(int _)
    {
        base.OnPlayerTurnStart(_);

        StartCoroutine(TurnStartDelay());
    }

    IEnumerator TurnStartDelay()
    {
        float delay = Random.Range(thinkingTimeMin,thinkingTimeMax);
        yield return new WaitForSeconds(delay);
        AutoAttack();
    }
}
