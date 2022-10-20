using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HP_Bar : MonoBehaviour
{
    Transform fill;

    private void Awake()
    {
        fill = transform.GetChild(1);       // fill 찾기

        IHealth target = GetComponentInParent<IHealth>();
        target.onHealthChange += Refresh;   // 델리게이트에 함수 연결
    }

    private void Refresh(float ratio)
    {
        fill.localScale = new Vector3(ratio, 1, 1); // 입력받은 비율대로 스케일 조절
    }

    /// <summary>
    /// 모든 게임 오브젝트의 Update 함수가 호출 된 이후에 호출되는 Update 함수.
    /// </summary>
    private void LateUpdate()
    {
        //transform.forward = Camera.main.transform.forward;  // 카메라의 forward와 나의 forward를 일치시킴
        transform.rotation = Camera.main.transform.rotation;    // 카메라와 회전을 일치시킴
    }
}
