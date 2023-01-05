using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Seamless : TestBase
{
    public int x = 0;
    public int y = 0;

    MapManager mapManager;

    private void Start()
    {
        mapManager = GameManager.Inst.MapManager;
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        mapManager.Test_LoadScene(x, y);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        mapManager.Test_UnloadScene(x, y);
    }
}
