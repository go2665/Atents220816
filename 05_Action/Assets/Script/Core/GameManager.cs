using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 변수 ---------------------------------------------------------------------------------------
    
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 아이템 데이터를 관리하는 메니저
    /// </summary>
    ItemDataManager itemData;

    /// <summary>
    /// 인벤토리 UI
    /// </summary>
    InventoryUI inventoryUI;


    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// player 읽기 전용 프로퍼티.
    /// </summary>
    public Player Player => player;

    /// <summary>
    /// 아이템 데이터 메니저(읽기전용) 프로퍼티
    /// </summary>
    public ItemDataManager ItemData => itemData;

    public InventoryUI InvenUI => inventoryUI;

    // 함수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 게임 메니저가 새로 만들어지거나 씬이 로드 되었을 때 실행될 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        itemData = GetComponent<ItemDataManager>();
        player = FindObjectOfType<Player>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }
}
