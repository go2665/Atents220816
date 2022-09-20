using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 양방향 수동문, TwoWayDoor, IUseableObject 상속받음
/// </summary>
public class ActiveTwoWayDoor : TwoWayDoor, IUseableObject
{
    // 현재 문이 열렸는지 닫혔는지 표시용. true면 문이 열려있음.
    bool isDoorOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openInFront = IsInFront(other.transform.position);  // 플레이어가 문 앞에 있는지 뒤에 있는지 체크
        }
    }

    private void OnTriggerExit(Collider other)
    {    
        // 영역을 나갔을 때 문이 자동으로 닫히는 것 방지
    }

    /// <summary>
    /// 오브젝트가 사용될 때 실행될 함수. 문 열고 닫힘.
    /// </summary>
    public void Use()
    {
        if (isDoorOpen) 
        {
            Close();    // 문이 열려있으면 문을 닫고
        }
        else
        {
            Open();     // 문이 닫혀있으면 문을 연다.
        }
        isDoorOpen = !isDoorOpen;   // 문 열린 상태 변화 기록
    }
}
