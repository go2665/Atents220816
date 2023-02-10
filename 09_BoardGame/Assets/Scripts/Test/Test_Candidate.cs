using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_Candidate : TestBase
{
    public Button reset;
    public Button randomDeployment;
    public Button resetRandom;

    public PlayerBase player1;  // 나라고 가정
    public PlayerBase player2;  // 적이라고 가정

    Board board1;
    Board board2;

    protected override void Awake()
    {
        base.Awake();
        GameObject canvas = GameObject.Find("Canvas");
        reset = canvas.transform.GetChild(0).GetComponent<Button>();
        randomDeployment = canvas.transform.GetChild(1).GetComponent<Button>();
        resetRandom = canvas.transform.GetChild(2).GetComponent<Button>();
    }

    private void Start()
    {
        board1 = player1.GetComponentInChildren<Board>();
        board2 = player2.GetComponentInChildren<Board>();

        reset.onClick.AddListener(OnResetClick);
        randomDeployment.onClick.AddListener(() => {
            player1.AutoShipDeployment(false);
            player2.AutoShipDeployment(false);
        });
        resetRandom.onClick.AddListener(() =>
        {
            OnResetClick();
            player1.AutoShipDeployment(false);
            player2.AutoShipDeployment(false);
        });

        Pattern1();
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

    private void OnResetClick()
    {
        // 진행 중이던 게임 리셋
        player1.Clear();
        player2.Clear();
    }


    private void OnTestClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos1 = board1.WorldToGrid(worldPos);
        Vector2Int gridPos2 = board2.WorldToGrid(worldPos);
        if( Board.IsValidPosition(gridPos1) )
        {
            player2.Attack(gridPos1);
        }
        if( Board.IsValidPosition(gridPos2) )
        { 
            player1.Attack(gridPos2); 
        }
    }

    private void OnTestRClick(InputAction.CallbackContext _)
    {
        // 마우스 위치에 있는 함선을 배치 취소
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        ShipType type = board1.GetShipType(worldPos);
        Ship ship = player1.Ships[(int)type - 1];
        board1.UndoShipDeplyment(ship);
    }

    private void OnTestWheel(InputAction.CallbackContext context)
    {
    }

    private void OnTestMove(InputAction.CallbackContext context)
    {        
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        Pattern1();
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        Pattern2();
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        Pattern5();
    }

    private void Pattern1()
    {
        OnResetClick();
        for (int i = 0; i < ShipManager.Inst.ShipTypeCount; i++)
        {
            Ship ship = player1.Ships[i];
            board1.ShipDeplyment(ship, new Vector2Int(i * 2, 0));

            ship = player2.Ships[i];
            board2.ShipDeplyment(ship, new Vector2Int(i * 2, 0));
        }
    }

    private void Pattern2()
    {
        OnResetClick();
        for (int i = 0; i < ShipManager.Inst.ShipTypeCount; i++)
        {
            Ship ship = player1.Ships[i];
            ship.Direction = ShipDirection.West;
            board1.ShipDeplyment(ship, new Vector2Int(0, i * 2));

            ship = player2.Ships[i];
            ship.Direction = ShipDirection.West;
            board2.ShipDeplyment(ship, new Vector2Int(0, i * 2));
        }
    }

    private void Pattern5()
    {
        OnResetClick();

        Ship ship = null;
        ship = player1.Ships[0];
        ship.Direction = ShipDirection.North;
        board1.ShipDeplyment(ship, new Vector2Int(1, 1));

        ship = player1.Ships[1];
        ship.Direction = ShipDirection.West;
        board1.ShipDeplyment(ship, new Vector2Int(5, 1));

        ship = player1.Ships[2];
        ship.Direction = ShipDirection.North;
        board1.ShipDeplyment(ship, new Vector2Int(4, 1));

        ship = player1.Ships[3];
        ship.Direction = ShipDirection.North;
        board1.ShipDeplyment(ship, new Vector2Int(4, 4));

        ship = player1.Ships[4];
        ship.Direction = ShipDirection.North;
        board1.ShipDeplyment(ship, new Vector2Int(1, 7));

        ship = player2.Ships[0];
        ship.Direction = ShipDirection.North;
        board2.ShipDeplyment(ship, new Vector2Int(1, 1));

        ship = player2.Ships[1];
        ship.Direction = ShipDirection.West;
        board2.ShipDeplyment(ship, new Vector2Int(5, 1));

        ship = player2.Ships[2];
        ship.Direction = ShipDirection.North;
        board2.ShipDeplyment(ship, new Vector2Int(4, 1));

        ship = player2.Ships[3];
        ship.Direction = ShipDirection.North;
        board2.ShipDeplyment(ship, new Vector2Int(4, 4));

        ship = player2.Ships[4];
        ship.Direction = ShipDirection.North;
        board2.ShipDeplyment(ship, new Vector2Int(1, 7));


    }
}
