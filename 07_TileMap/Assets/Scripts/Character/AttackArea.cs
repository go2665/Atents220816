using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    /// <summary>
    /// Slime이 이 영역 안에 들어오면 실행되는 델리게이트
    /// </summary>
    public Action<Slime> onTarget;

    /// <summary>
    /// Slime이 이 영역 밖으로 나가면 실행되는 델리게이트
    /// </summary>
    public Action<Slime> onTargetRelease;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))   // 태그 먼저 확인(가벼울 것이라 예상되어서)
        {
            Slime slime = collision.GetComponent<Slime>();  // 슬라임 받아오기
            if (slime != null)                              // 슬라임이 있으면
            {
                onTarget?.Invoke(slime);                    // 슬라임이 들어왔다고 신호 보내기
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))                  // 태그 확인
        {
            Slime slime = collision.GetComponent<Slime>();  // 슬라임 받아오고
            if (slime != null)                              // 슬라임이 있으면
            {
                onTargetRelease?.Invoke(slime);             // 슬라임이 나갔다고 신호보내기
            }
        }
    }
}
