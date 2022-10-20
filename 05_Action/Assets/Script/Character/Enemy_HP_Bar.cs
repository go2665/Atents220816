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
}
