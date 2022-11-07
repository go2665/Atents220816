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

    private void Awake()
    {
        Transform slotParent = transform.GetChild(0);
        slotUIs = GetComponentsInChildren<ItemSlotUI>();
    }

    /// <summary>
    /// 입력받은 인벤토리에 맞게 각종 초기화 작업을 하는 함수
    /// </summary>
    /// <param name="playerInven">이 UI로 표시할 인벤토리</param>
    public void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;

        if( Inventory.Default_Inventory_Size != inven.SlotCount )
        {
            // 기본 사이즈와 다르면 기존 슬롯을 전부 삭제하고 새로 만들기            
            //Debug.Log("인벤토리 사이즈가 다르다.");
            foreach( var slot in slotUIs)
            {
                Destroy(slot.gameObject);   // 기본적으로 가지고 있던 슬롯 모두 삭제
            }

            Transform slotParent = transform.GetChild(0);   // 새로 생성한 슬롯이 붙을 부모 찾기
            slotUIs = new ItemSlotUI[inven.SlotCount];      // 슬롯 배열을 새 크기에 맞게 새로 생성
            for(uint i=0;i<inven.SlotCount;i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);   // 슬롯을 하나씩 생성
                obj.name = $"{slotPrefab.name}_{i}";                    // 슬롯 이름이 안겹치게 변경
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();            // 슬롯을 배열에 저장
                slotUIs[i].InitializeSlot((uint)i, inven[i]);           // 각 슬롯 초기화
            }

            // 인벤토리 크기에 따라 ItemSlotUI의 크기 변경
            RectTransform rectParent = (RectTransform)slotParent;
            float totalArea = rectParent.rect.width * rectParent.rect.height;   // slotParent의 전체 면적 계산
            float slotArea = totalArea / inven.SlotCount;                       // slot 하나의 면적 구하기

            GridLayoutGroup grid = slotParent.GetComponent<GridLayoutGroup>();
            float slotSideLength = Mathf.Floor(Mathf.Sqrt(slotArea)) - grid.spacing.x;  // spacing 크기 고려해서 slot 한변의 길이 구하기
            grid.cellSize = new Vector2(slotSideLength, slotSideLength);                // 계산 결과 적용
        }
        else
        {
            // 크기가 같으면 슬롯UI들의 초기화 진행
            //Debug.Log("인벤토리 사이즈가 같다.");
            for (uint i=0;i<inven.SlotCount;i++)
            {
                slotUIs[i].InitializeSlot((uint)i, inven[i]);           // 각 슬롯 초기화
            }
        }
    }
}
