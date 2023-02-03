using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ShipDeployment : TestBase
{
    Board board;

    Ship targetShip = null;
    Ship TargetShip
    {
        get => targetShip;
        set
        {
            if( targetShip != value )
            {
                if(targetShip!= null)
                    targetShip.gameObject.SetActive( false );
                
                targetShip = value;
                
                if (targetShip != null)
                    targetShip.gameObject.SetActive( true );
            }
        }
    }

    Ship[] testShips = null;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        testShips = new Ship[ShipManager.Inst.ShipTypeCount];
        testShips[(int)ShipType.Carrier - 1] = ShipManager.Inst.MakeShip(ShipType.Carrier, this.transform);
        testShips[(int)ShipType.Battleship - 1] = ShipManager.Inst.MakeShip(ShipType.Battleship, this.transform);
        testShips[(int)ShipType.Destroyer - 1] = ShipManager.Inst.MakeShip(ShipType.Destroyer, this.transform);
        testShips[(int)ShipType.Submarine - 1] = ShipManager.Inst.MakeShip(ShipType.Submarine, this.transform);
        testShips[(int)ShipType.PatrolBoat - 1] = ShipManager.Inst.MakeShip(ShipType.PatrolBoat, this.transform);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.TestClick.performed += OnTestClick;
        inputActions.Test.Test_RClick.performed += OnTestRClick;
        inputActions.Test.TestWheel.performed += OnTestWheel;
        inputActions.Test.Test_MouseMove.performed += OnTestMove;
    }

    protected override void OnDisable()
    {
        inputActions.Test.Test_MouseMove.performed -= OnTestMove;
        inputActions.Test.TestWheel.performed -= OnTestWheel;
        inputActions.Test.Test_RClick.performed -= OnTestRClick;
        inputActions.Test.TestClick.performed -= OnTestClick;
        base.OnDisable();
    }

    private void OnTestClick(InputAction.CallbackContext _)
    {        
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        if(TargetShip != null )        
        {
            board.ShipDeplyment(TargetShip, world);
            TargetShip = null;
        }
    }

    private void OnTestRClick(InputAction.CallbackContext obj)
    {
        // testShip 배치 해제
        if(TargetShip != null)
        {
            board.UndoShipDeplyment(TargetShip);
            TargetShip = null;
        }
    }

    private void OnTestWheel(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();
        //Debug.Log(delta);
        bool ccw = false;   // 기본적으로 시계방향
        if (delta > 0.0f)    // 휠을 올리면
        {
            ccw = true;     // 반시계 방향으로
        }

        if (TargetShip != null)
        {
            TargetShip.Rotate(ccw);
        }
    }

    private void OnTestMove(InputAction.CallbackContext context)
    {
        if (TargetShip != null)
        {
            Vector2 moveDelta = context.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(moveDelta);
            worldPos.y = 0.0f;
            
            TargetShip.transform.position = board.GridToWorld(board.WorldToGrid(worldPos));
        }
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        Debug.Log("항공모함 선택");
        TargetShip = testShips[(int)ShipType.Carrier - 1];
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        Debug.Log("전함 선택");
        TargetShip = testShips[(int)ShipType.Battleship - 1];
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        Debug.Log("구축함 선택");
        TargetShip = testShips[(int)ShipType.Destroyer - 1];
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        Debug.Log("잠수함 선택");
        TargetShip = testShips[(int)ShipType.Submarine - 1];
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        Debug.Log("경비정 선택");
        TargetShip = testShips[(int)ShipType.PatrolBoat - 1];
    }
}
