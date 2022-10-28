using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 아이템 데이터를 관리하는 메니저
    /// </summary>
    ItemDataManager itemData;

    /// <summary>
    /// player 읽기 전용 프로퍼티.
    /// </summary>
    public Player Player => player;

    /// <summary>
    /// 아이템 데이터 메니저(읽기전용) 프로퍼티
    /// </summary>
    public ItemDataManager ItemData => itemData;

    /// <summary>
    /// 게임 메니저가 새로 만들어지거나 씬이 로드 되었을 때 실행될 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        itemData = GetComponent<ItemDataManager>();

        player = FindObjectOfType<Player>();
    }
}
