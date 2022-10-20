using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float HP { get; set; }  // HP를 확인하고 설정할 수 있다.
    float MaxHP { get; }    // 최대HP를 확인할 수 있다.

    /// <summary>
    /// HP가 변경될 때 실행될 델리게이트용 프로퍼티.
    /// 파라메터는 (현재/최대) 비율.
    /// </summary>
    Action<float> onHealthChange { get; set; }

    void Die();             // 죽었을 때 실행될 함수

    Action onDie { get; set; }  // 죽었을 때 실행될 데리게이트용 프로퍼티
}
