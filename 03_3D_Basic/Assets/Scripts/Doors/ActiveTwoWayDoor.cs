using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTwoWayDoor : TwoWayDoor, IUseableObject
{
    bool isDoorOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openInFront = IsInFront(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {    
        // 영역을 나갔을 때 자동으로 닫히는 것 방지
    }

    public void Use()
    {
        if (isDoorOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
        isDoorOpen = !isDoorOpen;
    }
}
