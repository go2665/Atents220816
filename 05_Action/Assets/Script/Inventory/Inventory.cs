using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// 인벤토리의 정보만 가지는 클래스
public class Inventory
{
    // 상수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 기본 인벤토리 칸 수 
    /// </summary>
    public const int Default_Inventory_Size = 6;
    public const uint TempSlotIndex = 99999;        // 어떤 숫자든 상관없다. slots의 인덱스가 될 수 있는 값만 아니면 된다.

    // 변수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 이 인벤토리가 가지고 있는 아이템 슬롯의 배열
    /// </summary>
    ItemSlot[] slots = null;

    /// <summary>
    /// 드래그 중인 아이템을 임시 저장하는 슬롯
    /// </summary>
    ItemSlot tempSlot = null;

    /// <summary>
    /// 게임 메니저가 가지는 아이템 데이터 매니저 캐싱용
    /// </summary>
    ItemDataManager dataManager;

    /// <summary>
    /// 이 인벤토리를 가지고 있는 플레이어
    /// </summary>
    Player owner;

    // 프로퍼티 ------------------------------------------------------------------------------------

    public int SlotCount => slots.Length;

    public ItemSlot TempSlot => tempSlot;

    /// <summary>
    /// 특정 번째의 ItemSlot을 돌려주는 인덱서
    /// </summary>
    /// <param name="index">돌려줄 슬롯의 위치</param>
    /// <returns>index번째에 있는 ItemSlot</returns>
    public ItemSlot this[uint index] => slots[index];

    /// <summary>
    /// 이 인벤토리를 가지고 있는 플레이어를 확인하는 프로퍼티
    /// </summary>
    public Player Owner => owner;

    // 함수들 --------------------------------------------------------------------------------------       
    public Inventory(Player owner, int size = Default_Inventory_Size)
    {        
        Debug.Log($"{size}칸짜리 인벤토리 생성");
        slots = new ItemSlot[size];
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot((uint)i);
        }
        tempSlot = new ItemSlot(TempSlotIndex);

        dataManager = GameManager.Inst.ItemData;

        this.owner = owner;
    }

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
            result = targetSlot.IncreaseSlotItem(out uint _);    // 갯수 증가 시도. 결과에 따라 result 변경
        }
        else
        {
            // 인벤토리에 같은 종류의 아이템이 없다.
            ItemSlot emptySlot = FindEmptySlot();
            if (emptySlot != null)
            {
                // 비어있는 슬롯을 찾았다.
                emptySlot.AssignSlotItem(data, false);
                result = true;
            }
            else
            {
                // 인벤토리가 가득 찼다.
                Debug.Log($"실패 : 인벤토리가 가득 찼습니다.");
            }
        }      

        return result;
    }

    /// <summary>
    /// 아이템을 인벤토리의 특정 슬롯에 1개 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템 코드</param>
    /// <param name="index">아이템이 추가될 슬롯의 인덱스</param>
    /// <returns>true면 성공, false면 실패</returns>
    public bool AddItem(ItemIDCode code, uint index)
    {
        return AddItem(dataManager[code], index);
    }

    /// <summary>
    /// 아이템을 인벤토리의 특정 슬롯에 1개 추가하는 함수
    /// </summary>
    /// <param name="data">추가할 아이템 데이터</param>
    /// <param name="index">아이템이 추가될 슬롯의 인덱스</param>
    /// <returns>true면 성공, false면 실패</returns>
    public bool AddItem(ItemData data, uint index)
    {
        bool result = false;

        if(IsValidSlotIndex(index))         // 인덱스가 적절한가?
        {
            ItemSlot slot = slots[index];   // 해당 인덱스의 슬롯 가져오기

            if(slot.IsEmpty)                // 해당 슬롯이 비어있는가?
            {
                // 비어있으면 그냥 아이템을 넣는다.
                slot.AssignSlotItem(data, false);
                result = true;
            }
            else
            {
                // 슬롯이 비어있지 않다.
                if(slot.ItemData == data)   //같은 종류의 아이템이 있는가?
                {
                    // 같은 종류의 아이템이 들어있으면 갯수만 추가                    
                    result = slot.IncreaseSlotItem(out uint _);    // 갯수 증가 시도. 결과에 따라 result 변경
                }
                else
                {
                    // 다른 종류의 아이템이 들어있으면 그냥 실패
                    Debug.Log($"실패 : 인벤토리 {index}번 슬롯에 다른 아이템이 들어있습니다.");
                }
            }
        }
        else
        {
            Debug.Log($"실패 : {index}는 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 아이템을 특정 인벤토리 슬롯에서 특정 갯수만큼 제거하는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 제거할 슬롯의 인덱스</param>
    /// <param name="decreaseCount">제거할 갯수(기본적으로 1)</param>
    /// <returns>성공이면 true, 실패면 false</returns>
    public bool RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;
        if( IsValidSlotIndex(slotIndex) )   // 적절한 인덱스일 때
        {
            ItemSlot slot = slots[slotIndex];       // 해당 슬롯에
            slot.DecreaseSlotItem(decreaseCount);   // decreaseCount만큼 아이템 갯수 감소
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

    /// <summary>
    /// 인벤토리의 모든 아이템을 비우는 함수
    /// </summary>
    public void ClearInventory()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }

    /// <summary>
    /// 아이템을 이동시키는 함수
    /// </summary>
    /// <param name="from">시작 인덱스</param>
    /// <param name="to">도칙 인덱스</param>
    public void MoveItem(uint from, uint to)
    {
        // from이 적절한 인덱스이고 아이템이 들어있다. 그리고 to는 적절한 인덱스이다.
        if(IsValidAndNotEmptySlotIndex(from) && IsValidSlotIndex(to))
        {
            // 슬롯 가져오기
            ItemSlot fromSlot = (from == Inventory.TempSlotIndex) ? TempSlot : slots[from];
            ItemSlot toSlot = (to == Inventory.TempSlotIndex) ? TempSlot : slots[to];

            if (fromSlot.ItemData == toSlot.ItemData)
            {
                // from과 to가 같은 아이템을 가지고 있으면 to에서 아이템 합치기
                toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // 아이템 증가 시도한 후 넘친 갯수 받아오기
                fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // from에서 to에 증가된 분량 만큼만 감소시키기
                Debug.Log($"인벤토리의 {from}슬롯에서 {to}슬롯으로 아이템 합치기 성공");
            }
            else
            {
                // from과 to가 서로 다른 아이템을 가지고 있으면 서로 스왑처리
                ItemData tempData = fromSlot.ItemData;
                bool tempEqip = fromSlot.IsEquipped;
                uint tempCount = fromSlot.ItemCount;
                fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.IsEquipped, toSlot.ItemCount);
                toSlot.AssignSlotItem(tempData, tempEqip, tempCount);
                Debug.Log($"인벤토리의 {from}슬롯과 {to}슬롯의 아이템 교체 성공");
            }
        }
    }

    /// <summary>
    /// 특정 슬롯에 있는 아이템을 임시 슬롯으로 옮기는 함수
    /// </summary>
    /// <param name="slotID">아이템을 감소시킬 슬롯</param>
    /// <param name="count">감소시키는 갯수</param>
    public void MoveItemToTempSlot(uint slotID, uint count)
    {
        if (IsValidAndNotEmptySlotIndex(slotID))    // 적절한 슬롯일때(인덱스가 적절하고 아이템이 들어있다.)
        {
            ItemSlot fromSlot = slots[slotID];                  // 슬롯 가져오고
            fromSlot.DecreaseSlotItem(count);                   // 원래 슬롯에 들어있던 아이템 갯수는 count만큼 감소
            tempSlot.AssignSlotItem(fromSlot.ItemData, fromSlot.IsEquipped, count);  // 임시 슬롯에 원래 슬롯에 들어있던 아이템 종류를 count만큼 설정
        }
    }

    // 아이템 사용

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
    /// 인벤토리에 파라메터와 같은 종류의 아이템이 있는지 찾아보는 함수(아이템이 들어갈 공간이 있는지도 확인)
    /// </summary>
    /// <param name="itemData">찾을 아이템</param>
    /// <returns>찾았으면 null이 아닌값(찾는 아이템이 들어있는 슬롯), 찾지 못했으면 null</returns>
    private ItemSlot FindSameItem(ItemData itemData)
    {
        ItemSlot findSlot = null;

        foreach(var slot in slots)
        {
            // 같은 종류의 아이템이고 빈칸이 있어야 한다.
            if( slot.ItemData == itemData && slot.ItemCount < slot.ItemData.maxStackCount)
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
    private bool IsValidSlotIndex(uint index) => (index < SlotCount) || (index == TempSlotIndex);

    /// <summary>
    /// 파라메터로 받은 인덱스가 적절한 인덱스이면서 비어있지 않은 것을 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 인덱스이면서 아이템이 들어있는 함수, false면 적절한 인덱스가 아니거나 비어있다.</returns>
    private bool IsValidAndNotEmptySlotIndex(uint index)
    {
        if (IsValidSlotIndex(index))
        {
            ItemSlot testSlot = (index == TempSlotIndex) ? TempSlot : slots[index];

            return !testSlot.IsEmpty;
        }

        return false;
    }
    
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
