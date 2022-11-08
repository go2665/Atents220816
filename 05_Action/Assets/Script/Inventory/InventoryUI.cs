using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // ItemSlotUI가 있는 프리팹. 인벤토리 크기 변화에 대비해서 가지고 있기.
    public GameObject slotPrefab;

    Inventory inven;

    ItemSlotUI[] slotUIs;
    TempItemSlotUI tempSlotUI;

    private void Awake()
    {
        //Transform slotParent = transform.GetChild(0);
        slotUIs = GetComponentsInChildren<ItemSlotUI>();
        tempSlotUI = GetComponentInChildren<TempItemSlotUI>();
    }

    /// <summary>
    /// 입력받은 인벤토리에 맞게 각종 초기화 작업을 하는 함수
    /// </summary>
    /// <param name="playerInven">이 UI로 표시할 인벤토리</param>
    public void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;

        Transform slotParent = transform.GetChild(0);               // 가져오기 용도
        GridLayoutGroup grid = slotParent.GetComponent<GridLayoutGroup>();

        if ( Inventory.Default_Inventory_Size != inven.SlotCount )  // 인벤토리 크기가 기본과 다를 때의 처리
        {
            // 기본 사이즈와 다르면 기존 슬롯을 전부 삭제하고 새로 만들기            
            //Debug.Log("인벤토리 사이즈가 다르다.");
            foreach( var slot in slotUIs)
            {
                Destroy(slot.gameObject);   // 기본적으로 가지고 있던 슬롯 모두 삭제
            }
            
            // 인벤토리 크기에 따라 ItemSlotUI의 크기 변경
            RectTransform rectParent = (RectTransform)slotParent;
            float totalArea = rectParent.rect.width * rectParent.rect.height;   // slotParent의 전체 면적 계산
            float slotArea = totalArea / inven.SlotCount;                       // slot 하나의 면적 구하기

            float slotSideLength = Mathf.Floor(Mathf.Sqrt(slotArea)) - grid.spacing.x;  // spacing 크기 고려해서 slot 한변의 길이 구하기
            grid.cellSize = new Vector2(slotSideLength, slotSideLength);                // 계산 결과 적용

            // 슬롯 새롭개 생성
            slotUIs = new ItemSlotUI[inven.SlotCount];      // 슬롯 배열을 새 크기에 맞게 새로 생성
            for(uint i=0;i<inven.SlotCount;i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);   // 슬롯을 하나씩 생성
                obj.name = $"{slotPrefab.name}_{i}";                    // 슬롯 이름이 안겹치게 변경
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();            // 슬롯을 배열에 저장                
            }            
        }

        // 공통 처리부분
        for (uint i = 0; i < inven.SlotCount; i++)
        {
            slotUIs[i].InitializeSlot((uint)i, inven[i]);           // 각 슬롯 초기화
            slotUIs[i].Resize(grid.cellSize.x * 0.75f);             // 슬롯 크기에 맞게 내부 크기 리사이즈
            slotUIs[i].onDragStart += OnItemDragStart;
            slotUIs[i].onDragEnd += OnItemDragEnd;
        }

        tempSlotUI.InitializeSlot(Inventory.TempSlotIndex, inven.TempSlot);
        tempSlotUI.Close();

    }
        
    private void OnItemDragStart(uint slotID)
    {
        inven.MoveItem(slotID, Inventory.TempSlotIndex);
        tempSlotUI.Open();
    }

    private void OnItemDragEnd(uint slotID)
    {
        tempSlotUI.Close();
        inven.MoveItem(Inventory.TempSlotIndex, slotID);
    }

}
