using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailInfoUI : MonoBehaviour
{
    // 컴포넌트들
    Image itemIcon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemValue;
    TextMeshProUGUI itemDesc;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        // 컴포넌트 찾기
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemValue = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemDesc = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 상세정보 창 열기
    /// </summary>
    /// <param name="itemData">상세정보 창에 설정할 아이템 데이터</param>
    public void Open(ItemData itemData)
    {
        if (itemData != null)   // itemData가 있을 때만 처리
        {
            itemIcon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;
            itemValue.text = itemData.value.ToString();
            itemDesc.text = itemData.itemDescription;

            canvasGroup.alpha = 1;  // 알파값을 모두 1로 만들어서 보이게 만들기
        }
    }

    /// <summary>
    /// 상세정보 창 닫기
    /// </summary>
    public void Close()
    {
        canvasGroup.alpha = 0;  // 알파값을 모두 0으로 만들어서 안보이게 만들기
    }
}
