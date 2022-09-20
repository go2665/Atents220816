using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayDoor : Door
{
    protected bool openInFront = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("문이 열려야 한다.");

            openInFront = IsInFront(other.transform.position);
            
            Open();
        }
    }

    public override void Open()
    {
        if(openInFront)
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

    public override void Close()
    {
        anim.SetTrigger("Close");
    }

    /// <summary>
    /// 플레이어가 문 앞에 있는지 뒤에 있는지 체크
    /// </summary>
    /// <param name="playerPosition">플레이어 위치</param>
    /// <returns>true면 문앞에 있고 false면 문 뒤에 있다.</returns>
    protected bool IsInFront(Vector3 playerPosition)
    {
        Vector3 playerToDoor = transform.position - playerPosition;
        return (Vector3.Angle(transform.forward, playerToDoor) > 90.0f);        
    }
}
