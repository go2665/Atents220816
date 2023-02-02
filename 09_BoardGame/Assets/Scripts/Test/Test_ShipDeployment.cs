using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Test_ShipDeployment : TestBase
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        int length = Enum.GetValues(typeof(ShipType)).Length - 1;
        for(int i=0;i<length;i++)
        {
            Ship ship = ShipManager.Inst.MakeShip((ShipType)(i+1), transform);
            ship.transform.Translate(Vector3.right * (i * 2.0f));
        }
    }
}
