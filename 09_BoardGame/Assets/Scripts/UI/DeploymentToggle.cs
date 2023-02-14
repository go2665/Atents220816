using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeploymentToggle : MonoBehaviour
{
    /// <summary>
    /// 이 버튼이 처리할 함선의 종류
    /// </summary>
    public ShipType shipType = ShipType.None;

    /// <summary>
    /// 배치할 함선을 가진 플레이어
    /// </summary>
    UserPlayer player;

    // 버튼 관련 변수 ------------------------------------------------------------------------------

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

    // 함수들 --------------------------------------------------------------------------------------

    private void Awake()
    {
        image= GetComponent<Image>();
        button= GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
    }

    /// <summary>
    /// 버튼이 클릭되었을 때 실행되는 함수
    /// </summary>
    private void OnClick()
    {
        if( IsToggled )
        {
            // 눌려져 있다가 해제될 예정이다.
            // => 배치되어있던 함선을 배치 취소해야 한다.
            player.UndoShipDeploy(shipType);        // 배치 취소
        }
        else
        {
            // 해제되어있다가 눌려질 예정이다.
            player.SelectShipToDeploy(shipType);    // 배치하기 위해 선택
        }

        IsToggled = !IsToggled; // 토글 상태만 반전 시킴
    }

    /// <summary>
    /// 눌려진 상태를 헤제하는 함수
    /// </summary>
    public void UnToggle()
    {
        if (!player.Ships[(int)shipType - 1].IsDeployed)    // 배치되지 않은 함선은
        {
            IsToggled = false;                              // 무조건 해제
        }
    }

    /// <summary>
    /// 무조건 눌려진 상태로 만드는 함수
    /// </summary>
    public void SetPress()
    {
        IsToggled = true;
    }
}
