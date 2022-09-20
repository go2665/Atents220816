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
            //Debug.Log("Use");
            isDoorOpen = !isDoorOpen;
            anim.SetBool("IsOpen", isDoorOpen);
        }
    }
}
