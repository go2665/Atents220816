using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    private uint id;    // 몇번째 슬롯인가?

    protected ItemSlot itemSlot;    // 이 UI와 연결된 ItemSlot


    public uint ID => id;
    public ItemSlot ItemSlot => itemSlot;

    public void InitializeSlot(uint id, ItemSlot slot)
    {
        this.id = id;
        this.itemSlot = slot;
    }
}
