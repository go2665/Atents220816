using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Network : TestBase
{
    bool effectSwitch = true;

    protected override void Test1(InputAction.CallbackContext _)
    {
        NetPlayer player = GameManager.Inst.Player;
        Animator anim = player.GetComponent<Animator>();
        anim.SetTrigger("Test");
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        // 플레이어의 색상이 랜덤으로 변경.(다른 사람에게도 보임)
        NetPlayer player = GameManager.Inst.Player;
        NetPlayerDecoration deco = player.GetComponent<NetPlayerDecoration>();
        Color color = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        deco.SetPlayerColorServerRpc(color);
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        NetPlayer player = GameManager.Inst.Player;
        player.IsEffectOn = effectSwitch;
        effectSwitch = !effectSwitch;
    }
}
