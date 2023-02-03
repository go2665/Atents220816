using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ShipDeployment : TestBase
{
    Board board;

    ShipType targetShip = ShipType.PatrolBoat;
    Ship[] testShips = null;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        testShips = new Ship[ShipManager.Inst.ShipTypeCount];
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.TestClick.performed += OnTestClick;
        inputActions.Test.Test_RClick.performed += OnTestRClick;
    }


    protected override void OnDisable()
    {
        inputActions.Test.Test_RClick.performed -= OnTestRClick;
        inputActions.Test.TestClick.performed -= OnTestClick;
        base.OnDisable();
    }

    private void OnTestClick(InputAction.CallbackContext _)
    {        
        int index = (int)(targetShip - 1);
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        if(testShips[index] == null )        
        {
            testShips[index] = ShipManager.Inst.MakeShip(targetShip, this.transform);
            board.ShipDeplyment(testShips[index], world);
        }
    }

    private void OnTestRClick(InputAction.CallbackContext obj)
    {
        // testShip 배치 해제
        int index = (int)(targetShip - 1);
        if(testShips[index] != null)
        {
            board.UndoShipDeplyment(testShips[index]);
            Destroy(testShips[index].gameObject);
            testShips[index] = null;
        }
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        targetShip = ShipType.Carrier;
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        targetShip = ShipType.Battleship;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        targetShip = ShipType.Destroyer;
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        targetShip = ShipType.Submarine;
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        targetShip = ShipType.PatrolBoat;
    }
}
