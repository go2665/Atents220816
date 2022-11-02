using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 한칸의 정보를 나타내는 클래스
public class ItemSlot
{
    /// <summary>
    /// 이 슬롯의 인덱스(인벤토리의 몇번째 슬롯인가?)
    /// </summary>
    uint slotIndex;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템
    /// </summary>
    ItemData slotItemData = null;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 갯수
    /// </summary>
    uint itemCount = 0;

    // 프로퍼티 ------------------------------------------------------------------------------------
    /// <summary>
    /// 이 슬롯이 비었는지 여부(true면 비었고,false 무엇인가 들어있다.)
    /// </summary>
    public bool IsEmpty => (slotItemData == null);

    /// <summary>
    /// 이 슬롯의 인덱스
    /// </summary>
    public uint Index => slotIndex;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 데이터
    /// </summary>
    public ItemData ItemData => slotItemData;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 갯수
    /// </summary>
    public uint ItemCount => itemCount;


    public ItemSlot(uint index)
    {
        slotIndex = index;
    }

    /// <summary>
    /// 이 슬롯에 지정된 아이템을 지정된 갯수로 넣는 함수
    /// </summary>
    /// <param name="data">추가할 아이템</param>
    /// <param name="count">설정된 갯수</param>
    public void AssignSlotItem(ItemData data, uint count = 1)
    {
        itemCount = count;
        slotItemData = data;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{slotItemData.itemName}\" 아이템 {itemCount}개 설정");
    }

    /// <summary>
    /// 이 슬롯에서 아이템을 제거하는 함수
    /// </summary>
    public void ClearSlotItem()
    {
        slotItemData = null;
        itemCount = 0;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
    }

    /// <summary>
    /// 이 슬롯에 아이템 갯수를 증가시키는 함수
    /// </summary>
    /// <param name="count">증가시킬 아이템 갯수</param>
    public void IncreaseSlotItem(uint count = 1)
    {
        itemCount += count;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{slotItemData.itemName}\" 아이템 {count}개만큼 증가. 현재 {itemCount}개");
    }

    /// <summary>
    /// 이 슬롯에 아이템 갯수를 감소시키는 함수
    /// </summary>
    /// <param name="count">감소시킬 아이템 갯수</param>
    public void DecreaseSlotItem(uint count = 1)
    {
        int newCount = (int)itemCount - (int)count; // underflow를 대비해서 부호있는 인티저로 처리

        if(newCount < 1)
        {
            // 새로운 갯수가 0이하면 슬롯을 비우기
            ClearSlotItem();
        }
        else
        {
            // 갯수가 남아있으면 해당 갯수로 설정
            itemCount = (uint)newCount;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{slotItemData.itemName}\" 아이템 {count}개만큼 감소. 현재 {itemCount}개");
        }
    }

    // 내일 볼 것 : 프로퍼티 사용
}
