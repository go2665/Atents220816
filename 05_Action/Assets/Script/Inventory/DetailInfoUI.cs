using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DetailInfoUI : MonoBehaviour
{
    /// <summary>
    /// 알파값이 변경되는 속도
    /// </summary>
    public float alphaChangeSpeed = 10.0f;
    
    // 컴포넌트들
    Image itemIcon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemValue;
    TextMeshProUGUI itemDesc;
    CanvasGroup canvasGroup;

    /// <summary>
    /// 작동 일시 정지 확인용 변수
    /// </summary>
    bool isPause = false;

    /// <summary>
    /// 목표로 하는 알파값
    /// </summary>
    float targetAlpha = 0.0f;


    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// 일시정지를 확인하고 설정하는 프로퍼티
    /// </summary>
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if(isPause)     // 일시 정지가 되면 무조건 상세정보 창을 닫는다.
            {
                Close();
            }
        }
    }

    /// <summary>
    /// 열려있는지 확인하는 프로퍼티
    /// </summary>
    public bool IsOpen => (canvasGroup.alpha > 0.0f);

    
    // 함수 ---------------------------------------------------------------------------------------
    private void Awake()
    {
        // 컴포넌트 찾기
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemValue = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemDesc = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if(targetAlpha > 0)
        {
            // 목표 알파가 0보다 크다 => 켜지고 있는 중이다.
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
        }
        else
        {
            // 목표 알파가 0보다 작거나 같다. => 꺼지고 있는 중이다.
            canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
        }
        canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha, 0, 1);   // 항상 범위가 0~1이 되도록 설정
    }

    /// <summary>
    /// 상세정보 창 열기
    /// </summary>
    /// <param name="itemData">상세정보 창에 설정할 아이템 데이터</param>
    public void Open(ItemData itemData)
    {
        if ( !isPause && itemData != null)   // 일시 정지 상태가 아니고 itemData가 있을 때만 처리
        {
            itemIcon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;
            itemValue.text = itemData.value.ToString();
            itemDesc.text = itemData.itemDescription;

            targetAlpha = 1;  // 알파값을 모두 1로 만들도록 설정

            MovePosition(Mouse.current.position.ReadValue());   // 열릴 때 항상 마우스 위치를 기준으로 열기
        }
    }

    /// <summary>
    /// 상세정보 창 닫기
    /// </summary>
    public void Close()
    {
        targetAlpha = 0;    // 알파값을 모두 0으로 만들도록 설정
    }

    /// <summary>
    /// 상세정보창의 위치를 옮기는 함수
    /// </summary>
    /// <param name="pos">새 위치</param>
    public void MovePosition(Vector2 pos)
    {
        RectTransform rect = (RectTransform)transform;

        if (pos.x + rect.sizeDelta.x > Screen.width) // 상세정보 창이 화면을 벗어났는지 확인
        {
            pos.x -= rect.sizeDelta.x;   // 상세정보창이 화면을 넘어서면 상세정보창의 가로 길이만큼 왼쪽으로 이동
        }

        transform.position = pos; // 실제로 상세정보창에 이동 적용
    }
}
