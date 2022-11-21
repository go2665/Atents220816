using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class TempItemSlotUI : ItemSlotUI
{
    /// <summary>
    /// 이 임시슬롯이 포함된 인벤토리
    /// </summary>
    private InventoryUI invenUI;

    /// <summary>
    /// 이 임시 슬롯이 포함된 인벤토리를 가지고 있는 플레이어
    /// </summary>
    private Player owner;

    /// <summary>
    /// 임시 슬롯이 열리고 닫힘을 알리는 델리게이트. true면 열렸다. false면 닫혔다.
    /// </summary>
    public Action<bool> onTempSlotOpenClose;

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();        // 매 프레임마다 마우스 위치로 이동
    }

    /// <summary>
    /// 슬롯 초기화 함수
    /// </summary>
    /// <param name="id">슬롯의 ID. 99999</param>
    /// <param name="slot">이 UI가 보여줄 임시 ItemSlot</param>
    /// <param name="owner">이 UI를 사용하는 플레이어</param>
    public override void InitializeSlot(uint id, ItemSlot slot)
    {
        onTempSlotOpenClose = null;     // 델리게이트 초기화 추가

        invenUI = GameManager.Inst.InvenUI; // 인벤토리 UI 찾기
        owner = invenUI.Owner;              // owner 설정

        base.InitializeSlot(id, slot);        
    }

    /// <summary>
    /// TempItemSlotUI를 여는 함수
    /// </summary>
    public void Open()
    {
        if(!ItemSlot.IsEmpty)               // 아이템이 들어있을 때만 열기
        {
            transform.position = Mouse.current.position.ReadValue();    // 열릴 때 마우스 위치로 이동
            onTempSlotOpenClose?.Invoke(true);  // 열었다고 알림
            gameObject.SetActive(true);         // 활성화
        }
    }

    /// <summary>
    /// TempItemSlotUI를 닫는 함수
    /// </summary>
    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);     // 닫혔다고 알림
        gameObject.SetActive(false);            // 비활성화
    }

    public void OnDrop(InputAction.CallbackContext _)
    {
        //Debug.Log("OnDrop");
        Vector2 screenPos = Mouse.current.position.ReadValue();         // 스크린 좌표 가져오기
        if (!invenUI.IsInInventoryArea(screenPos) && !ItemSlot.IsEmpty) // 스크린좌표가 인벤토리 영역 밖이고 임시 슬롯에 아이템이 있을 때
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);          // 스크린 좌표로 레이 생성
            //Debug.Log($"Ray : {ray}");
            if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground"))) // 레이와 땅의 충돌 여부 확인
            {
                // 레이와 땅이 충돌했으면
                Vector3 dropDir = hit.point - owner.transform.position; // 피킹된 지점과 플레이어의 위치를 계산해서 방향 벡터 구하기
                Vector3 dropPos = hit.point;    // 피킹한 지점 따로 저장

                if (dropDir.sqrMagnitude > owner.itemPickupRange * owner.itemPickupRange)
                {
                    // 피킹한 지점이 너무 멀리 떨어져 있으면
                    // 플레이어 위치에서 일정 범위(owner.itemPickupRange)를 벗어나지 않도록 처리
                    dropPos = owner.transform.position + dropDir.normalized * owner.itemPickupRange;
                }

                // 아이템을 땅에 버릴 때 아이템을 장착하고 있으면 해제하고 버리기
                if (ItemSlot.IsEquipped)
                {
                    ItemData_EquipItem equipItem = ItemSlot.ItemData as ItemData_EquipItem;
                    if (equipItem)
                    {
                        equipItem.UnEquipItem(owner.gameObject, ItemSlot);        
                    }
                }

                // 아이템 생성
                ItemFactory.MakeItem((int)ItemSlot.ItemData.id, (int)ItemSlot.ItemCount, dropPos, true);
                ItemSlot.ClearSlotItem();   // 임시 슬롯 비우고
                Close();                    // 임시 슬롯 닫기
            }
        }
    }
}
