using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapJumppad : TrapBase
{
    public float power = 5.0f;

    protected override void TrapActivate(GameObject target)
    {
        IFly fly = target.GetComponent<IFly>();
        fly.Fly(transform.up * power);
    }
}
