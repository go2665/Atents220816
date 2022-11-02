using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBase
{
    Inventory inven;

    private void Start()
    {
        inven = new Inventory(10);
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        inven.AddItem(ItemIDCode.Ruby);
        inven.AddItem(ItemIDCode.Emerald);
        inven.AddItem(ItemIDCode.Sapphire);
        inven.AddItem(ItemIDCode.Emerald);
        inven.AddItem(ItemIDCode.Ruby);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        inven.PrintInventory();
    }
}
