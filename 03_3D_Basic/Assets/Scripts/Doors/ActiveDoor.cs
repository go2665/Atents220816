using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수동문 클래스. Door(문의 기본 기능 가짐), IUseableObject(사용할 수 있는 오브젝트라는 특성을 가짐) 상속 받음.
/// </summary>
public class ActiveDoor : Door, IUseableObject
{
    /// <summary>
    /// 플레이어가 문을 열수 있는 위치에 있는지 판단하는 변수. true면 플레이어가 문을 열 수 있는 위치에 있다.
    /// </summary>
    bool playerIn = false;

    /// <summary>
    /// 현재 문이 열렸는지를 기록하는 변수. true면 문이 열려있다.
    /// </summary>
    bool isDoorOpen = false;
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = true;    // 플레이어가 문앞에 오면 playerIn을 true로 만든다.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = false;   // 플레이어가 문앞에서 멀어지면 playerIn을 false로 만든다.
        }
    }

    /// <summary>
    /// 오브젝트가 사용될 때 실행될 함수. 플레이어가 문 앞에 있는 상태로 실행되면 문을 열고 닫는다.
    /// </summary>
    public void Use()
    {
        if (playerIn)   // 플레이어가 문앞에 있는지 확인
        {
            // 플레이어가 문 앞에 있는 상태에서
            if(isDoorOpen)
            {
                Close();    // 문이 열려있으면 문을 닫아라.
            }
            else
            {
                Open();     // 문이 닫혀있으면 문을 열어라.
            }
            isDoorOpen = !isDoorOpen;   // 문 열린 상태 표시용 스위치 변경하기
        }
    }
}
