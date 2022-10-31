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
        player.Defence(60);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        player.HP = 100;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        //GameManager.Inst.ItemData[0]
        //GameManager.Inst.ItemData.TestItemData[0]

        //GameManager.Inst.ItemData[ItemIDCode.Ruby];

        GameObject obj = ItemFactory.MakeItem(ItemIDCode.Ruby);
        GameObject obj2 = ItemFactory.MakeItem(ItemIDCode.Emerald, new Vector3(0,0,1.2f));
        GameObject obj3 = ItemFactory.MakeItem(ItemIDCode.Emerald, new Vector3(0,0,1.2f), true);

        GameObject[] obj4 = ItemFactory.MakeItem(ItemIDCode.Sapphire, 5);
        GameObject[] obj5 = ItemFactory.MakeItem(ItemIDCode.Sapphire, 5, new Vector3(0,0,2), true);
    }
}
