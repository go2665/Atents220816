using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 길을 그리기 위한 타일 클래스(자동으로 적절한 스프라이트로 변경해주는 클래스)
public class RoadTile : Tile
{
    /// <summary>
    /// 주변 어느 위치에 RoadTile이 있는지 표시하기 위한 enum.
    /// </summary>
    [Flags]             // 이 enum은 비트플래그로 사용하겠다.
    enum AdjTilePosition : byte     // 8bit 크기의 enum
    {
        None = 0,       // 0000 0000. 주변에 RoadTile이 없다.
        North = 1,      // 0000 0001. 북쪽에 RoadTile이 있다.
        East = 2,       // 0000 0010. 동쪽에 RoadTile이 있다.
        South = 4,      // 0000 0100. 남쪽에 RoadTile이 있다.
        West = 8,       // 0000 1000. 서쪽에 RoadTile이 있다.
        All = North | East | South | West   // 0000 1111. 모든 방향에 RoadTile이 있다.
    }

    // 논리 연산 : true/false, ||,  &&
    // 비트 연산 : 1/0,         |(둘 중 하나만 1이면 1),  &(둘 다 1이여야 1)



    /// <summary>
    /// 타일이 배치될 때 주변 타일 상황에 따라 자동으로 선택되어 보여질 스프라이트
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    /// 타일이 그려질때 자동으로 호출이 되는 함수.
    /// (타일이 타일맵에 배치되면 타일에서 선택한 스프라이트를 그리는데 그 때 자동으로 호출됨)
    /// (지금 표시할 스프라이트에 맞게 다시 그리라고 신호를 보내는 역할)
    /// </summary>
    /// <param name="position">타일맵에서 타일이 그려지는 위치</param>
    /// <param name="tilemap">이 타일이 그려질 타일 맵</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for(int y=0;y<2;y++)
        {
            for(int x=0;x<2;x++)
            {
                Vector3Int location = new(position.x + x, position.y + y, position.z);  // 주변 8방향의 위치
                if(HasThisTile(tilemap, location))  // 주변타일이 나와 같은지 확인
                {
                    tilemap.RefreshTile(location);  // 같은 종류면 갱신 시킨다.
                }
            }
        }
    }

    /// <summary>
    /// 타일에 대한 타일 랜더링 데이터(tileData)를 찾아서 전달
    /// (실제로 그려질 스프라이트를 결정)
    /// </summary>
    /// <param name="position">타일맵에서 타일 데이터를 가져올 타일의 위치</param>
    /// <param name="tilemap">타엘 데이터를 가져올 타일맵</param>
    /// <param name="tileData">가져온 타일 데이터의 참조(읽기, 쓰기 가능)</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        AdjTilePosition mask = AdjTilePosition.None;

        // mask에 주변 타일의 상황을 기록하기
        // ex) 북쪽에 RoadTile이 있으면 mask에는 AdjTilePosition.North가 들어가야 한다.
        //     북동쪽에 RoadTile이 있으면 mask에는 (AdjTilePosition.North|AdjTilePosition.East)가 들어가야 한다.


    }

    /// <summary>
    /// 타일맵에서 지정된 위치가 같은 종류의 타일인지 확인
    /// </summary>
    /// <param name="tilemap">확인할 타일맵</param>
    /// <param name="position">확인할 위치</param>
    /// <returns>true면 같은 종류의 타일이다. false면 다른 종류의 타일이다.</returns>
    bool HasThisTile(ITilemap tilemap, Vector3Int position)
    {
        // 타일맵에서 타일을 가져온 후 나와 같은지 확인
        return tilemap.GetTile(position) == this;
    }
}
