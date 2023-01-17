using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Network : TestBase
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        NetPlayer player = GameManager.Inst.Player;
        Animator anim = player.GetComponent<Animator>();
        anim.SetTrigger("Test");
    }
}
