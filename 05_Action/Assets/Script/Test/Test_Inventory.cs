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

    protected override void Test3(InputAction.CallbackContext _)
    {
        inven.ClearItem(1);
        inven.ClearItem(3);
        inven.ClearItem(15);
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        inven.RemoveItem(0);
        inven.RemoveItem(1, 3);
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        inven.AddItem(ItemIDCode.Ruby, 9);
        inven.AddItem(ItemIDCode.Emerald, 8);
        inven.AddItem(ItemIDCode.Sapphire, 20);
    }
}
