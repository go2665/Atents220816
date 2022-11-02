using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// 인벤토리의 정보만 가지는 클래스
public class Inventory
{
    /// <summary>
    /// 기본 인벤토리 칸 수 
    /// </summary>
    public const int Default_Inventory_Size = 6;

    /// <summary>
    /// 이 인벤토리가 가지고 있는 아이템 슬롯의 배열
    /// </summary>
    ItemSlot[] slots = null;

    /// <summary>
    /// 게임 메니저가 가지는 아이템 데이터 매니저 캐싱용
    /// </summary>
    ItemDataManager dataManager;

    public int SlotCount => slots.Length;

    public Inventory(int size = Default_Inventory_Size)
    {
        Debug.Log($"{size}칸짜리 인벤토리 생성");
        slots = new ItemSlot[size];
        for(int i=0;i<size;i++)
        {
            slots[i] = new ItemSlot((uint)i);
        }

        dataManager = GameManager.Inst.ItemData;
    }

    // 아이템 추가

    /// <summary>
    /// 아이템을 인벤토리에 1개 추가하는 함수
    /// </summary>
    /// <param name="code">추가될 아이템의 코드</param>
    /// <returns>성공여부(true면 성공, false면 실패)</returns>
    public bool AddItem(ItemIDCode code)
    {
        return AddItem(dataManager[code]);
    }

    /// <summary>
    /// 아이템을 인벤토리에 1개 추가하는 함수
    /// </summary>
    /// <param name="data">추가될 아이템 데이터</param>
    /// <returns>성공여부(true면 성공, false면 실패)</returns>
    public bool AddItem(ItemData data)
    {
        // 같은 종류의 아이템을 합치려면 어떻게 해야 하는가?

        bool result = false;

        // 같은 종류의 아이템이 있는가?
        // 있으면 -> 갯수 증가
        // 없으면 -> 새 슬롯에 아이템 넣기

        ItemSlot targetSlot = FindSameItem(data);
        if(targetSlot != null)
        {
            // 같은 종류의 아이템이 있다.
            targetSlot.IncreaseSlotItem();  // 갯수 증가
            result = true;
        }
        else
        {
            // 인벤토리에 같은 종류의 아이템이 없다.
            ItemSlot emptySlot = FindEmptySlot();
            if (emptySlot != null)
            {
                // 비어있는 슬롯을 찾았다.
                emptySlot.AssignSlotItem(data);
                result = true;
            }
            else
            {
                // 인벤토리가 가득 찼다.
                Debug.Log($"인벤토리가 가득 찼습니다.");
            }
        }      

        return result;
    }

    public bool RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;
        if( IsValidSlotIndex(slotIndex) )
        {
            ItemSlot slot = slots[slotIndex];
            slot.DecreaseSlotItem(decreaseCount);
            result = true;
        }
        else
        {
            Debug.Log($"실패 : {slotIndex}는 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 특정 슬롯에서 아이템을 제거하는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 제거할 함수</param>
    /// <returns>true면 성공, false면 실패</returns>
    public bool ClearItem(uint slotIndex)
    {
        bool result = false;

        if(IsValidSlotIndex(slotIndex))
        {
            ItemSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
            return true;
        }
        else
        {
            Debug.Log($"실패 : {slotIndex}는 잘못된 인덱스입니다.");
        }
        

        return result;
    }

    // 아이템 사용
    // 아이템 이동

    /// <summary>
    /// 비어있는 슬롯을 찾는 함수
    /// </summary>
    /// <returns>비어있는 함수를 찾으면 null이 아니고 비어있는 함수가 없으면 null</returns>
    private ItemSlot FindEmptySlot()
    {
        ItemSlot result = null;
        foreach(var slot in slots)
        {
            if(slot.IsEmpty)
            {
                result = slot;
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// 인벤토리에 파라메터와 같은 종류의 아이템이 있는지 찾아보는 함수
    /// </summary>
    /// <param name="itemData">찾을 아이템</param>
    /// <returns>찾았으면 null이 아닌값(찾는 아이템이 들어있는 슬롯), 찾지 못했으면 null</returns>
    private ItemSlot FindSameItem(ItemData itemData)
    {
        ItemSlot findSlot = null;

        foreach(var slot in slots)
        {
            if( slot.ItemData == itemData )
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// 파라메터로 받은 인덱스가 적절한 인덱스인지 판단하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 사용가능한 인덱스, false면 사용불가능한 인덱스</returns>
    private bool IsValidSlotIndex(uint index) => (index < SlotCount);

    public void PrintInventory()
    {
        // 출력 예시 : [ 루비(1), 사파이어(1), 에메랄드(2), (빈칸), (빈칸), (빈칸) ]
        string printText = "[ ";

        // ItemsSlot을 마지막 슬롯을 빼고 전부 확인해서 이름과 갯수를 printText에 추가
        for(int i=0;i<SlotCount-1;i++)
        {
            if(!slots[i].IsEmpty)
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount})";
            }
            else
            {
                printText += "(빈칸)";
            }
            printText += ", ";
        }

        // 마지막 슬롯만 따로 처리
        ItemSlot lastSlot = slots[SlotCount - 1];
        if (!lastSlot.IsEmpty)
        {
            printText += $"{lastSlot.ItemData.itemName}({lastSlot.ItemCount}) ]";
        }
        else
        {
            printText += "(빈칸) ]";
        }

        Debug.Log(printText);
    }
}
