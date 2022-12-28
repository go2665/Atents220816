using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test_TilemapAStar : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Tilemap test;

    void Start()
    {
        GridMap map = new GridMap(background, obstacle);
        int i=0;

        //background.size.x;  // 타일맵의 가로 크기
        //background.size.y;  // 타일맵의 세로 크기
        //background.origin.x;
        //background.origin.y;
        //obstacle.GetTile(new (10, 5));

    }

}
