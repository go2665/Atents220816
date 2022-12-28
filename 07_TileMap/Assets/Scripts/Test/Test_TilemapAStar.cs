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

    public LineRenderer lineRenderer;
    public Transform start;
    public Transform goal;
    
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
        path = AStar.PathFind(map, WorldToGrid(start.position), WorldToGrid(goal.position));
        string pathStr = "Path : ";
        foreach (var node in path)
        {
            pathStr += $" ( {node.x},{node.y} ) ->";
        }
        pathStr += " 끝";
        Debug.Log(pathStr);

        lineRenderer.positionCount = path.Count;
        int index = 0;
        foreach(var node in path)
        {
            Vector2 worldPos = GridToWorld(node);
            lineRenderer.SetPosition(index, new(worldPos.x - lineRenderer.transform.position.x,
                worldPos.y - lineRenderer.transform.position.y, 1));
            index++;
        }
    }

    Vector2Int WorldToGrid(Vector3 pos)
    {
        return (Vector2Int)background.WorldToCell(pos);
    }

    Vector2 GridToWorld(Vector2Int gridPos)
    {
        return background.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f, 0.5f);
    }

    private void Test_LeftClick(InputAction.CallbackContext _)
    {
        // 시작지점 옮기기
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = WorldToGrid(worldPos);
        Vector2 finalPos = GridToWorld(gridPos);

        start.position = finalPos;
    }


    private void Test_RightClick(InputAction.CallbackContext _)
    {
        // 도착지점 옮기기
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = WorldToGrid(worldPos);
        Vector2 finalPos = GridToWorld(gridPos);

        goal.position = finalPos;
    }

}
