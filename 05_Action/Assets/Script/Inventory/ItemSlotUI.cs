using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    private uint id;    // 몇번째 슬롯인가?

    protected ItemSlot itemSlot;    // 이 UI와 연결된 ItemSlot

    private Image itemImage;


    public uint ID => id;
    public ItemSlot ItemSlot => itemSlot;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// 슬롯 초기화 함수
    /// </summary>
    /// <param name="id">슬롯의 ID</param>
    /// <param name="slot">이 UI가 보여줄 ItemSlot</param>
    public void InitializeSlot(uint id, ItemSlot slot)
    {
        this.id = id;
        this.itemSlot = slot;
        this.itemSlot.onSlotItemChange = Refresh;

        Refresh();
    }

    /// <summary>
    /// 자식 게임 오브젝트들의 크기 변경
    /// </summary>
    /// <param name="iconSize">아이콘 한변의 크기</param>
    public void Resize(float iconSize)
    {
        RectTransform rectTransform = (RectTransform)itemImage.gameObject.transform;
        rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
    }

    /// <summary>
    /// 슬롯의 보이는 모습 갱신 용도의 함수. itemSlot의 내부 데이터가 변경될 때마다 실행.
    /// </summary>
    private void Refresh()
    {
        if( itemSlot.IsEmpty )
        {
            // 아이템 슬롯이 비었으면
            itemImage.sprite = null;        // 스프라이트 빼고
            itemImage.color = Color.clear;  // 투명화
        }
        else
        {
            // 아이템 슬롯에 아이템이 있으면
            itemImage.sprite = itemSlot.ItemData.itemIcon;  // 해당 아이콘 이미지 표시
            itemImage.color = Color.white;                  // 불투명화
        }
    }    
}
