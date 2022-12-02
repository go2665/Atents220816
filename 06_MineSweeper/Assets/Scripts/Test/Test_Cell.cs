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

    protected override void Test2(InputAction.CallbackContext _)
    {
        int[] array = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Board board = GameManager.Inst.Board;
        board.Shuffle(array);

        string output = "Array : ";
        foreach( var num in array)
        {
            output += $"{num}, ";
        }
        output += "끝";
        Debug.Log(output);
    }
}
