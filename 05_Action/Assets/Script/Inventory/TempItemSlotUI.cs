using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class TempItemSlotUI : ItemSlotUI
{
    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();        // 매 프레임마다 마우스 위치로 이동
    }

    /// <summary>
    /// TempItemSlotUI를 여는 함수
    /// </summary>
    public void Open()
    {
        if(!ItemSlot.IsEmpty)               // 아이템이 들어있을 때만 열기
        {
            transform.position = Mouse.current.position.ReadValue();    // 열릴 때 마우스 위치로 이동
            gameObject.SetActive(true);     // 활성화
        }
    }

    /// <summary>
    /// TempItemSlotUI를 닫는 함수
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);        // 비활성화
    }
}
