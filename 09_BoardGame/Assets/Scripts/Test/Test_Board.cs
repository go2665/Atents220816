using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Board : TestBase
{
    Board board;

    private void Start()
    {
        board = FindObjectOfType<Board>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.TestClick.performed += OnTestClick;
    }

    protected override void OnDisable()
    {
        inputActions.Test.TestClick.performed -= OnTestClick;
        base.OnDisable();
    }

    private void OnTestClick(InputAction.CallbackContext _)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);
        Vector2Int grid = board.WorldToGrid(world);
        Debug.Log($"클릭 그리드 : ({grid.x}, {grid.y})");
        Vector3 GtoW = board.GridToWorld(grid);
        Debug.Log($"클릭 월드 : ({GtoW.x}, {GtoW.z})");
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        if( Board.GridToIndex(1,1) == 11 
            && Board.GridToIndex(1, 2) == 21
            && Board.GridToIndex(0, 9) == 90)
        {
            Debug.Log("정상 - GridToIndex");
        }
        else
        {
            Debug.LogError("비정상 - GridToIndex");
        }
        
        if( Board.IndexToGrid(11) == new Vector2Int(1,1)
            && Board.IndexToGrid(21) == new Vector2Int(1, 2)
            && Board.IndexToGrid(90) == new Vector2Int(0, 9) )
        {
            Debug.Log("정상 - IndexToGrid");
        }
        else
        {
            Debug.LogError("비정상 - IndexToGrid");
        }

        if( board.WorldToGrid(board.transform.position + new Vector3(3.5f, 0, -2.1f)) == new Vector2Int(3,2) 
            && board.WorldToGrid(board.transform.position + new Vector3(1.5f, 0, -7.1f)) == new Vector2Int(1, 7)
            && board.WorldToGrid(board.transform.position + new Vector3(5.9f, 0, -3.1f)) == new Vector2Int(5, 3))
        {
            Debug.Log("정상 - WorldToGrid");
        }
        else
        {
            Debug.LogError("비정상 - WorldToGrid");
        }

        if( board.GridToWorld(0,0) == new Vector3(board.transform.position.x + 0.5f, board.transform.position.y, board.transform.position.z-0.5f)
            && board.GridToWorld(1, 1) == new Vector3(board.transform.position.x + 0.5f + 1, board.transform.position.y, board.transform.position.z - 0.5f -1)
            && board.GridToWorld(5, 3) == new Vector3(board.transform.position.x + 0.5f + 5, board.transform.position.y, board.transform.position.z - 0.5f -3))
        {
            Debug.Log("정상 - GridToWorld");
        }
        else
        {
            Debug.LogError("비정상 - GridToWorld");
        }
    }
}
