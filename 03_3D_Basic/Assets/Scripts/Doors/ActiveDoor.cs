using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDoor : Door, IUseableObject
{
    bool playerIn = false;
    bool isDoorOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = false;
        }
    }

    public void Use()
    {
        if (playerIn)
        {
            // 플레이어가 영역안에 들어왔는데 
            //Debug.Log("Use");
            if(isDoorOpen)
            {
                Close();    // 문이 열려있으면 문을 닫아라.
            }
            else
            {
                Open();     // 문이 닫혀있으면 문을 열어라.
            }
            isDoorOpen = !isDoorOpen;            
        }
    }
}
