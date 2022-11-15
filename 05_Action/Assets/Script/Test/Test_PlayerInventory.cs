using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerInventory : TestBase
{
    Player player;

    private void Start()
    {
        player = GameManager.Inst.Player;
        player.HP = 30;
        player.MP = 20;
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        player.ManaRegenerate(50, 3);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        player.MP += 10;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        player.MP -= 10;
    }
}
