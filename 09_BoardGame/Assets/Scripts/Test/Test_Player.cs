using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_Player : TestBase
{
    public Button reset;
    public Button randomDeployment;
    public Button resetRandom;
    public PlayerBase player;

    Board board;

    private void Start()
    {
        board = FindObjectOfType<Board>();

        reset.onClick.AddListener(OnResetClick);
        randomDeployment.onClick.AddListener(() => {
            player.AutoShipDeployment(false);
        });
        resetRandom.onClick.AddListener(() =>
        {
            OnResetClick();
            player.AutoShipDeployment(false);
        });
         
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
        // 배치되어 있는 모든 함선을 배치 취소

        player.UndoAllShipDeployment();
    }


    private void OnTestClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = board.WorldToGrid(worldPos);
        board.OnAttacked(gridPos);
    }

    private void OnTestRClick(InputAction.CallbackContext _)
    {
        // 마우스 위치에 있는 함선을 배치 취소
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        ShipType type = board.GetShipType(worldPos);
        Ship ship = player.Ships[(int)type - 1];
        board.UndoShipDeplyment(ship);
    }

    private void OnTestWheel(InputAction.CallbackContext context)
    {
    }

    private void OnTestMove(InputAction.CallbackContext context)
    {        
    }
}
