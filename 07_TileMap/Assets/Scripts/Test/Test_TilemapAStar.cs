using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_TilemapAStar : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Tilemap test;

    public Transform start;
    public Transform goal;
    public PathLineDraw pathLine;

    GridMap map;
    List<Vector2Int> path;

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.LeftClick.performed += Test_LeftClick;
        inputActions.Test.RighttClick.performed += Test_RightClick;
    }

    protected override void OnDisable()
    {
        inputActions.Test.LeftClick.performed -= Test_LeftClick;
        inputActions.Test.RighttClick.performed -= Test_RightClick;
        base.OnDisable();
    }

    void Start()
    {
        path = new List<Vector2Int>();
        map = new GridMap(background, obstacle);
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        path = AStar.PathFind(map, map.WorldToGrid(start.position), map.WorldToGrid(goal.position));
        string pathStr = "Path : ";
        foreach (var node in path)
        {
            pathStr += $" ( {node.x},{node.y} ) ->";
        }
        pathStr += " 끝";
        Debug.Log(pathStr);

        pathLine.DrawPath(map, path);
    }

    private void Test_LeftClick(InputAction.CallbackContext _)
    {
        // 시작지점 옮기기
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);
        if (!map.IsWall(gridPos))
        {
            Vector2 finalPos = map.GridToWorld(gridPos);
            start.position = finalPos;
        }
    }


    private void Test_RightClick(InputAction.CallbackContext _)
    {
        // 도착지점 옮기기
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);
        if (!map.IsWall(gridPos))
        {
            Vector2 finalPos = map.GridToWorld(gridPos);
            goal.position = finalPos;
        }
    }

}
