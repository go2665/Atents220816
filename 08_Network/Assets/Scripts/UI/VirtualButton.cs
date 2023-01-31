using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualButton : MonoBehaviour, IPointerClickHandler
{
    // VirtualButton의 공용 데이터 -----------------------------------------------------------------
    
    /// <summary>
    /// 공격 종류
    /// </summary>
    public enum AttackType
    {
        Bullet,
        Ball
    }

    /// <summary>
    /// 이 버튼들이 공용으로 사용할 공격 아이콘들
    /// </summary>
    public Sprite[] attackIcons = new Sprite[Enum.GetValues(typeof(AttackType)).Length];




    // 버튼 인스턴스 하나가 단독으로 사용하는 데이터 --------------------------------------------------
    /// <summary>
    /// 사용할 공격 종류. 인스펙터 창에서 쉽게 아이콘을 바꾸기 위한 용도
    /// </summary>
    public AttackType attackType = AttackType.Bullet;

    /// <summary>
    /// 공격 아이콘이 보여질 이미지. OnValidate에서 attackIcon가 없어서 발생하는 에러 제거를 위해 public으로 선언
    /// </summary>
    public Image attackIcon;

    /// <summary>
    /// 공격 쿨다운 표시용 이미지
    /// </summary>
    Image attackCoolDown;

    /// <summary>
    /// 버튼이 클릭되었을 때 실행될 델리게이트. 플레이어에게 알리기 위한 용도
    /// </summary>
    public Action onClick;

    private void Awake()
    {
        attackIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        attackCoolDown = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        attackCoolDown.fillAmount = 0;
    }

    /// <summary>
    /// attackType 변경 처리를 위한 OnValidata
    /// </summary>
    private void OnValidate()
    {
        if (attackIcons.Length > 0)
        {
            RefreshButtonIcon();
        }
    }

    /// <summary>
    /// 현재 attackType에 따라 아이콘 이미지 변경
    /// </summary>
    void RefreshButtonIcon()
    {
        attackIcon.sprite = attackIcons[(int)attackType];
    }

    public void OnPointerClick(PointerEventData _)
    {
        onClick?.Invoke();  // 클릭했을 때 델리게이트만 보내기
    }
}
