using System;
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
    /// 이 슬롯에 들어있는 아이템 데이터
    /// </summary>
    public ItemData ItemData
    {
        get => slotItemData;        // 읽기는 누구나 가능
        private set                 // 쓰기는 자신만 가능
        {
            if( slotItemData != value )     // 슬롯에 아이템이 변경이 있었을 때만
            {
                slotItemData = value;       // 값을 변경하고
                onSlotItemChange?.Invoke(); // 델리게이트에 연결된 함수들 실행(주로 UI 갱신용)
            }
        }
    }
    

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 갯수
    /// </summary>
    public uint ItemCount
    {
        get => itemCount;       // 읽기는 누구나 가능
        private set             // 쓰기는 자신만 가능
        {
            if(itemCount != value)              // 아이템 갯수에 변경이 일어났을 때만
            {
                itemCount = value;              // 값을 변경하고
                onSlotItemChange?.Invoke();     // 델리게이트에 연결된 함수들 실행(주로 UI 갱신용)
            }
        }
    }


    // 프로퍼티(읽기전용) --------------------------------------------------------------------------

    /// <summary>
    /// 이 슬롯이 비었는지 여부(true면 비었고,false 무엇인가 들어있다.)
    /// </summary>
    public bool IsEmpty => (slotItemData == null);

    /// <summary>
    /// 이 슬롯의 인덱스
    /// </summary>
    public uint Index => slotIndex;


    // 델리게이트 ----------------------------------------------------------------------------------
    public Action onSlotItemChange;


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
        ItemCount = count;
        ItemData = data;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {ItemCount}개 설정");
    }

    /// <summary>
    /// 이 슬롯에서 아이템을 제거하는 함수
    /// </summary>
    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
    }

    /// <summary>
    /// 이 슬롯에 아이템 갯수를 증가시키는 함수
    /// </summary>
    /// <param name="count">증가시킬 아이템 갯수</param>
    public void IncreaseSlotItem(uint count = 1)
    {
        if (!IsEmpty)   // 슬롯이 비어있지 않을 때만 가능
        {
            ItemCount += count;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {count}개만큼 증가. 현재 {ItemCount}개");
        }
    }

    /// <summary>
    /// 이 슬롯에 아이템 갯수를 감소시키는 함수
    /// </summary>
    /// <param name="count">감소시킬 아이템 갯수</param>
    public void DecreaseSlotItem(uint count = 1)
    {
        if (!IsEmpty)   // 슬롯이 비어있지 않을 때만 가능
        {
            int newCount = (int)ItemCount - (int)count; // underflow를 대비해서 부호있는 인티저로 처리

            if (newCount < 1)
            {
                // 새로운 갯수가 0이하면 슬롯을 비우기
                ClearSlotItem();
            }
            else
            {
                // 갯수가 남아있으면 해당 갯수로 설정
                ItemCount = (uint)newCount;
                Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {count}개만큼 감소. 현재 {ItemCount}개");
            }
        }
    }
}
