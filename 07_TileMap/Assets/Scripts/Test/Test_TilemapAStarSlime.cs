using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_TilemapAStarSlime : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Slime slime;

    GridMap map;
    public GridMap Map => map;

    protected override void Awake()
    {
        base.Awake();
        map = new GridMap(background, obstacle);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.LeftClick.performed += Test_LeftClick;
    }

    protected override void OnDisable()
    {
        inputActions.Test.LeftClick.performed -= Test_LeftClick;
        base.OnDisable();
    }

    private void Test_LeftClick(InputAction.CallbackContext obj)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);
        if (!map.IsWall(gridPos))
        {
            slime.SetDestination(gridPos);
        }
    }
}
