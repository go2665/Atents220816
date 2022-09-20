using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 연결된 문을 열고 닫는 스위치. IUseableObject 상속받음
/// </summary>
public class DoorSwitch : MonoBehaviour, IUseableObject
{
    /// <summary>
    /// 이 스위치로 열고 닫을 문
    /// </summary>
    public Door targetDoor;

    /// <summary>
    /// 스위치 사용 여부. true면 켰다. false면 껐다.
    /// </summary>
    bool switchOn = false;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 이 오브젝트가 사용되면 실행될 함수
    /// </summary>
    public void Use()
    {
        switchOn = !switchOn;   // 스위치 on/off 서로 전환
        anim.SetBool("SwitchOn", switchOn); // switchOn에 맞게 애니메이션 재생
        if ( switchOn )
        {
            // 스위치를 켰으면 targetDoor를 연다.
            targetDoor.Open();            
        }
        else
        {
            // 스위치를 끄면 targetDoor를 닫는다.
            targetDoor.Close();
        }
    }
}
