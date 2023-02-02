using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ShipDeployment : TestBase
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
        //Debug.Log($"클릭 그리드 : ({grid.x}, {grid.y})");
        //Vector3 GtoW = board.GridToWorld(grid);
        ////Debug.Log($"클릭 월드 : ({GtoW.x}, {GtoW.z})");

        //Ship ship = ShipManager.Inst.MakeShip(ShipType.Carrier, this.transform);
        //ship.transform.position = GtoW;
        //ship.gameObject.SetActive(true);

        //Debug.Log(board.IsValidPosition(world));
        Debug.Log(Board.IsValidPosition(grid));
    }
}
