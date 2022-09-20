using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 양쪽으로 열리고 닫히는 문. Door 상속 받음
/// </summary>
public class TwoWayDoor : Door
{
    // 문을 앞에서 열었는지 뒤에서 열었는지 표시용 변수. true면 앞에서 열었다.
    protected bool openInFront = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 문앞에 있는 상황에서

            openInFront = IsInFront(other.transform.position);  // 플레이어의 위치가 문 앞인지 뒤인지 확인하고
            
            Open(); // 문을 연다.
        }
    }

    /// <summary>
    /// 문을 여는 함수
    /// </summary>
    public override void Open()
    {
        if(openInFront) // 문의 앞인지 뒤인지 확인
        {
            // 문 앞에서 문을 연다.
            anim.SetTrigger("OpenInFront");
        }
        else
        {
            // 문 뒤에서 문을 연다.
            anim.SetTrigger("OpenInBack");
        }
    }

    /// <summary>
    /// 문을 닫는 함수
    /// </summary>
    public override void Close()
    {
        anim.SetTrigger("Close");   // 닫는것은 열린 방향과 상관 없다.
    }

    /// <summary>
    /// 플레이어가 문 앞에 있는지 뒤에 있는지 체크
    /// </summary>
    /// <param name="playerPosition">플레이어 위치</param>
    /// <returns>true면 문앞에 있고 false면 문 뒤에 있다.</returns>
    protected bool IsInFront(Vector3 playerPosition)
    {
        Vector3 playerToDoor = transform.position - playerPosition;         // 플레이어 위치에서 문의 위치로 가는 방향 벡터 계산
        return (Vector3.Angle(transform.forward, playerToDoor) > 90.0f);    // 방향 벡터와 문의 front 방향 벡터의 사이각을 통해 앞뒤 판단.
    }
}
