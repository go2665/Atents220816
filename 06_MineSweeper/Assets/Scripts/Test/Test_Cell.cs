using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Cell : TestBase
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        Board board = GameManager.Inst.Board;
        board.Initialize(GameManager.Inst.boardWidth, GameManager.Inst.boardHeight, GameManager.Inst.mineCount);
    }
}
