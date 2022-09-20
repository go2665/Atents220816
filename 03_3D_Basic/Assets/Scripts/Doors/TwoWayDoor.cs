using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayDoor : Door
{
    bool openInFront = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("문이 열려야 한다.");
            Vector3 playerToDoor = transform.position - other.transform.position;
            if( Vector3.Angle(transform.forward, playerToDoor ) > 90.0f )
            {
                // 앞
                openInFront = true;                
            }
            else
            {
                // 뒤
                openInFront = false;                
            }
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
}
