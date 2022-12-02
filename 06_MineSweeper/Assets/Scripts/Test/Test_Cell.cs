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

    protected override void Test3(InputAction.CallbackContext _)
    {
        float startTime = Time.realtimeSinceStartup;    // 시간 측정용

        Board board = GameManager.Inst.Board;
        int[,] result = new int[10,10];                 // 결과 저장용 이중 배열

        for(int i=0;i<1000000;i++)                      // 100만번 수행
        {
            int[] array = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // 기준 배열
            board.Shuffle(array);                           // suffle 수행

            for(int j=0;j<array.Length;j++)                 // 수행 결과 저장하기
            {
                // 해당 되는 위치에 1증가. 열 : array[j], 행 : j
                result[array[j], j]++;
            }
        }

        string output = "";                             // 수행 결과 출력하기
        for (int y = 0; y < 10; y++)
        {
            output += $"숫자{y} : ";
            for (int x = 0; x < 10; x++)
            {
                output += $"{result[y, x]} ";
            }
            output += "\n";
        }
        Debug.Log(output);

        Debug.Log($"경과 시간 : {Time.realtimeSinceStartup-startTime}");
    }
}
