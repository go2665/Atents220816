using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    protected override void Start()
    {
        base.Start();
        opponent = GameManager.Inst.EnemyPlayer; // 상대방 설정

    }
}
