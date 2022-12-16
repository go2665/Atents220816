using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        for(int y=-1;y<2;y++)
        {
            for(int x=-1;x<2;x++)
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

        //if(HasThisTile(tilemap, position + new Vector3Int(0,1,0)))
        //{
        //    //mask = mask | AdjTilePosition.North;
        //    mask |= AdjTilePosition.North;
        //}
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePosition.North : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePosition.East: 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePosition.West : 0;

        // mask값에 따라 어떤 스프라이트를 보여 줄 것인지 결정
        int index = GetIndex(mask);
        if( index > -1 )
        {
            tileData.sprite = sprites[index];                       // index번째의 스프라이트로 변경
            tileData.color = Color.white;                           // 색상은 기본 흰색
            Matrix4x4 m = tileData.transform;                       // transform 매트릭스 받아와서
            // mask값에 따라 얼마만큼 회전 시킬 것인지를 결정
            m.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one); // 계산한 회전대로 매트릭스 설정
            tileData.transform = m;                                 // 매트릭스 변경한 것으로 적용
            tileData.flags = TileFlags.LockTransform;               // 다른 타일이 회전을 변경 못하도록
            tileData.colliderType = ColliderType.None;              // 길이니까 컬라이더 없음
        }
        else
        {
            Debug.Log("에러 : 잘못된 인덱스");
        }
    }

    /// <summary>
    /// mask 설정에 따라 어떤 스프라이트를 사용할 것인지를 결정해서 돌려주는 함수
    /// </summary>
    /// <param name="mask">주변 타일 상황 확인용 마스크</param>
    /// <returns>그려질 스프라이트의 인덱스</returns>
    int GetIndex(AdjTilePosition mask)
    {
        int index = -1;
        switch (mask)
        {
            case AdjTilePosition.None:
                index = 0;
                break;
            case AdjTilePosition.South | AdjTilePosition.West:
            case AdjTilePosition.West | AdjTilePosition.North:
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.East | AdjTilePosition.South:
                index = 1;  // ㄱ자 모양의 스프라이트
                break;
            case AdjTilePosition.North:
            case AdjTilePosition.East:
            case AdjTilePosition.South:
            case AdjTilePosition.West:
            case AdjTilePosition.North | AdjTilePosition.South:
            case AdjTilePosition.East | AdjTilePosition.West:
                index = 2;  // ㅣ자 모양의 스프라이트
                break;
            case AdjTilePosition.All & ~AdjTilePosition.North:  // 0000 1111 & 1111 1110 = 0000 1110 
            case AdjTilePosition.All & ~AdjTilePosition.East:
            case AdjTilePosition.All & ~AdjTilePosition.South:
            case AdjTilePosition.All & ~AdjTilePosition.West:
                index = 3;  // ㅗ자 모양의 스프라이트
                break;
            case AdjTilePosition.All:
                index = 4;
                break;
        }
        return index;
    }

    /// <summary>
    /// mask 설정에 따라 스프라이트를 몇도 회전 시킬 것인지를 결정해서 회전을 돌려주는 함수
    /// </summary>
    /// <param name="mask">주변 타일 상황 확인용 마스크</param>
    /// <returns>스프라이트를 회전 시킬 회전</returns>
    Quaternion GetRotation(AdjTilePosition mask)
    {
        Quaternion rotate = Quaternion.identity;
        switch(mask)
        {
            case AdjTilePosition.North | AdjTilePosition.West:  // ㄱ자 돌리기
            case AdjTilePosition.East:                          // ㅣ자 돌리기
            case AdjTilePosition.West:
            case AdjTilePosition.East | AdjTilePosition.West:
            case AdjTilePosition.All & ~AdjTilePosition.West:   // ㅗ자 돌리기
                rotate = Quaternion.Euler(0, 0, -90);
                break;
            case AdjTilePosition.North | AdjTilePosition.East:  // ㄱ자 돌리기
            case AdjTilePosition.All & ~AdjTilePosition.North:  // ㅗ자 돌리기
                rotate = Quaternion.Euler(0, 0, -180);                
                break;
            case AdjTilePosition.East | AdjTilePosition.South:  // ㄱ자 돌리기
            case AdjTilePosition.All & ~AdjTilePosition.East:   // ㅗ자 돌리기
                rotate = Quaternion.Euler(0, 0, -270);
                break;
        }

        return rotate;
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

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Tiles/RoadTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject( // 파일 저장용 창 열기
            "Save Road Tile",   // 제목
            "New Road Tile",    // 파일의 기본 이름
            "Asset",            // 파일의 확장자
            "Save Road Tile",   // 출력용 메세지
            "Assets");          // 기본으로 지정된 폴더
        if( path != "" )
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);    // 위에서 지정한 경로에 RoadTile 에셋을 하나 만듬
        }
    }
#endif
}
