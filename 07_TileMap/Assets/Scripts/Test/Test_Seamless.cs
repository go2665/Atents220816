using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Seamless : TestBase
{
    public int x = 0;
    public int y = 0;
    
    Player player;
    MapManager mapManager;

    private void Start()
    {
        mapManager = GameManager.Inst.MapManager;
        player = GameManager.Inst.Player;
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        mapManager.Test_LoadScene(x, y);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        mapManager.Test_UnloadScene(x, y);
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        Debug.Log(mapManager.WorldToGrid(player.transform.position));        
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        for(int i = 0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                mapManager.Test_LoadScene(i, j);
            }
        }
    }
}
