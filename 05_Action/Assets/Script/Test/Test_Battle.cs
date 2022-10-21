using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Battle : TestBase
{
    Player player;

    private void Start()
    {
        player = GameManager.Inst.Player;
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        player.Defence(10);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        player.HP = 100;
    }
}
