using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 문 클래스
/// </summary>
public class Door : MonoBehaviour
{
    /// <summary>
    /// 애니메이터 컴포넌트(문 열고 닫는 애니메이션 처리용)
    /// </summary>
    protected Animator anim;

    /// <summary>
    /// Awake. 필요한 컴포넌트 찾기
    /// </summary>
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 플레이어가 들어오면 문을 열기.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player")) // 대상의 태그 확인
        {
            // 태그가 플레이어이면 문을 연다.
            Open(); // 문열기
        }
    }

    /// <summary>
    /// 플레이어가 나가면 문을 닫기
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 태그가 플레이어면 문을 닫는다.
            Close();
        }
    }

    /// <summary>
    /// 문을 여는 함수
    /// </summary>
    public virtual void Open()
    {
        anim.SetBool("IsOpen", true);
    }

    /// <summary>
    /// 문을 닫는 함수
    /// </summary>
    public virtual void Close()
    {
        anim.SetBool("IsOpen", false);
    }
}
