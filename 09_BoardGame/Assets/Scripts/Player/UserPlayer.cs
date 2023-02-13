using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    Action<Vector2>[] onClick;
    Action<Vector2>[] onMouseMove;
    Action<float>[] onMouseWheel;

    protected override void Awake()
    {
        base.Awake();

        int length = Enum.GetValues(typeof(GameState)).Length;
        onClick = new Action<Vector2>[length];
        onClick[(int)GameState.ShipDeployment] = OnClick_ShipDeployment;
        onClick[(int)GameState.ShipDeployment] = OnClick_Battle;
        onMouseMove = new Action<Vector2>[length];
        onMouseMove[(int)GameState.ShipDeployment] = OnMouseMove_ShipDeployment;
        onMouseWheel = new Action<float>[length];
        onMouseWheel[(int)GameState.ShipDeployment] = OnMouseWheel_ShipDeployment;
    }

    protected override void Start()
    {
        base.Start();
        opponent = GameManager.Inst.EnemyPlayer; // 상대방 설정

        GameManager.Inst.Input.onClick += OnClick;
        GameManager.Inst.Input.onMouseMove += OnMouseMove;
        GameManager.Inst.Input.onMouseWheel += OnMouseWheel;
    }

    private void OnClick(Vector2 screenPos)
    {
        onClick[(int)state]?.Invoke(screenPos);
    }

    private void OnMouseMove(Vector2 screenPos)
    {
        onMouseMove[(int)state]?.Invoke(screenPos);
    }

    private void OnMouseWheel(float wheelDelta)
    {
        onMouseWheel[(int)state]?.Invoke(wheelDelta);
    }

    // 상태별 입력 처리 함수들 ------------------------------------------------------------------------------
    private void OnClick_ShipDeployment(Vector2 screenPos)
    {
        throw new NotImplementedException();
    }

    private void OnClick_Battle(Vector2 screenPos)
    {
        throw new NotImplementedException();
    }

    private void OnMouseMove_ShipDeployment(Vector2 screenPos)
    {
        throw new NotImplementedException();
    }

    private void OnMouseWheel_ShipDeployment(float wheelDelta)
    {
        throw new NotImplementedException();
    }

}
