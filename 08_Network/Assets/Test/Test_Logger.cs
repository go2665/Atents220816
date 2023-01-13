
using UnityEngine.InputSystem;

public class Test_Logger : TestBase
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        GameManager.Inst.Logger.Log("123123\n");
    }
}

