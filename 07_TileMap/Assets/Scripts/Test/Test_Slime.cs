using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime : TestBase
{
    public GameObject slimePrefab;
    Slime slime;
    bool onOff = false;

    protected override void Test1(InputAction.CallbackContext _)
    {
        GameObject obj = Instantiate(slimePrefab);
        slime = obj.GetComponent<Slime>();
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        slime?.Die();
        slime = null;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        onOff = !onOff;
        slime?.ShowOutline(onOff);
    }
}
