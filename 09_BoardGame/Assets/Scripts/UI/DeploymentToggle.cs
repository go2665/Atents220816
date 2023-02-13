using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeploymentToggle : MonoBehaviour
{
    /// <summary>
    /// 버튼의 이미지
    /// </summary>
    Image image;

    /// <summary>
    /// 이 스크립트가 있는 버튼
    /// </summary>
    Button button;

    /// <summary>
    /// 눌러졌을 때 적용될 색상
    /// </summary>
    readonly Color selectedColor = new(1, 1, 1, 0.2f);

    /// <summary>
    /// 버튼이 눌려진 상태인지 여부
    /// </summary>
    bool isToggled = false;

    /// <summary>
    /// 버튼 토글 상태를 확인하거나 토글 되었을 때 수행할 일들을 처리하는 프로퍼티
    /// </summary>
    private bool IsToggled
    {
        get => isToggled;
        set
        {
            if( isToggled != value )                // 토글 상태가 변경되는지 확인
            {
                isToggled = value;                  // 변경된 상황이면 변경 적용
                if( isToggled )
                {
                    image.color = selectedColor;    // 눌려졌으면 색상 변경하고
                    onTogglePress?.Invoke(this);    // 눌려졌다고 델리게이트 날리기
                }
                else
                {
                    image.color = Color.white;      // 눌려진 것이 해제되었으면 원래 색상으로 되돌리기
                }
            }
        }
    }

    /// <summary>
    /// 눌려지지 않은 상태에서 눌러졌을 때 실행될 델리게이트
    /// </summary>
    public Action<DeploymentToggle> onTogglePress;    

    private void Awake()
    {
        image= GetComponent<Image>();
        button= GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// 버튼이 클릭되었을 때 실행되는 함수
    /// </summary>
    private void OnClick()
    {
        IsToggled = !IsToggled; // 토글 상태만 반전 시킴
    }

    /// <summary>
    /// 눌려진 상태를 헤제하는 함수
    /// </summary>
    public void UnToggle()
    {
        IsToggled = false;      // 무조건 해제
    }
}
