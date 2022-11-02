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

    public bool IsEmpty => (slotItemData == null);
    public uint Index => slotIndex;

    public ItemData ItemData => slotItemData;
    public uint ItemCount => itemCount;


    public ItemSlot(uint index)
    {
        slotIndex = index;
    }

    public void AssignSlotItem(ItemData data, uint count = 1)
    {
        itemCount = count;
        slotItemData = data;
    }
}
