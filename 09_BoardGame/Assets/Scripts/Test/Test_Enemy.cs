using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_Enemy : TestBase
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

        player1.AutoShipDeployment(false);
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
        player1.Board.ResetBoard(player1.Ships);
        player1.RemoveAllHighCantidate();
        player2.Board.ResetBoard(player2.Ships);
        player2.RemoveAllHighCantidate();
    }


    private void OnTestClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = board2.WorldToGrid(worldPos);
        player1.Attack(gridPos);
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
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
    }
}
