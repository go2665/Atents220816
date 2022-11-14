using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMana
{
    float MP { get; set; }  // MP를 확인하고 설정할 수 있다.
    float MaxMP { get; }    // 최대MP를 확인할 수 있다.

    /// <summary>
    /// MP가 변경될 때 실행될 델리게이트용 프로퍼티.
    /// 파라메터는 (현재/최대) 비율.
    /// </summary>
    Action<float> onManaChange { get; set; }

    /// <summary>
    /// Update 함수에서 실행될 함수. 마나를 지속적으로 증가시켜주는 함수
    /// </summary>
    void ManaRegenerate();
}
