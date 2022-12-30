using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AStar : TestBase
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        Test_Sort();
    }

    private static void Test_Sort()
    {
        Node node1 = new Node(0, 0);
        node1.G = 1;
        node1.H = 1;
        Node node2 = new Node(0, 0);
        node2.G = 5;
        node2.H = 5;
        Node node3 = new Node(0, 0);
        node3.G = 3;
        node3.H = 3;
        Node node4 = new Node(0, 0);
        node4.G = 4;
        node4.H = 4;
        Node node5 = new Node(0, 0);
        node5.G = 2;
        node5.H = 2;

        List<Node> nodeList = new List<Node>();
        nodeList.Add(node1);
        nodeList.Add(node2);
        nodeList.Add(node3);
        nodeList.Add(node4);
        nodeList.Add(node5);
        nodeList.Sort();


        //int i = 0;
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        GridMap gridMap = new GridMap(3, 3);
        Node node = gridMap.GetNode(1, 1);
        node.G = 1;
        node.H = 2;
        gridMap.ClearAStarData();
        //int i = 0;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        GridMap gridMap = new GridMap(4, 4);
        Node wall = gridMap.GetNode(0, 2);
        wall.gridType = Node.GridType.Wall;
        wall = gridMap.GetNode(2, 2);
        wall.gridType = Node.GridType.Wall;
        wall = gridMap.GetNode(3, 2);
        wall.gridType = Node.GridType.Wall;

        List<Vector2Int> list = AStar.PathFind(gridMap, new Vector2Int(0, 0), new Vector2Int(3, 3));
        string path = "Path : ";
        foreach (var node in list)
        {
            path += $" ( {node.x},{node.y} ) ->";
        }
        path += " 끝";
        Debug.Log(path);
        // 예상 결과
        // Path : (0,0) -> (1,1) -> (1,2) -> (1,3) -> (2,3) -> (3,3)
        
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        //Test_Fail_NoPath();
        Test_Fail_MapOut();
    }

    private static void Test_Fail_NoPath()
    {
        Debug.Log("Test_Fail_NoPath");

        GridMap gridMap = new GridMap(4, 4);
        Node wall = gridMap.GetNode(0, 2);
        wall.gridType = Node.GridType.Wall;
        wall = gridMap.GetNode(2, 2);
        wall.gridType = Node.GridType.Wall;
        wall = gridMap.GetNode(3, 2);
        wall.gridType = Node.GridType.Wall;
        wall = gridMap.GetNode(1, 2);
        wall.gridType = Node.GridType.Wall;

        List<Vector2Int> list = AStar.PathFind(gridMap, new Vector2Int(0, 0), new Vector2Int(3, 3));
        if (list != null)
        {
            string path = "Path : ";
            foreach (var node in list)
            {
                path += $" ( {node.x},{node.y} ) ->";
            }
            path += " 끝";
            Debug.Log(path);
        }
        else
        {
            Debug.Log("리스트가 비었음");
        }
    }

    private static void Test_Fail_MapOut()
    {
        Debug.Log("Test_Fail_MapOut");
        GridMap gridMap = new GridMap(4, 4);
        Node wall = gridMap.GetNode(0, 2);
        wall.gridType = Node.GridType.Wall;
        wall = gridMap.GetNode(2, 2);
        wall.gridType = Node.GridType.Wall;
        wall = gridMap.GetNode(3, 2);
        wall.gridType = Node.GridType.Wall;

        List<Vector2Int> list = AStar.PathFind(gridMap, new Vector2Int(10, 0), new Vector2Int(3, 3));
        if (list != null)
        {
            string path = "Path : ";
            foreach (var node in list)
            {
                path += $" ( {node.x},{node.y} ) ->";
            }
            path += " 끝";
            Debug.Log(path);
        }
        else
        {
            Debug.Log("리스트가 비었음");
        }
    }
}
