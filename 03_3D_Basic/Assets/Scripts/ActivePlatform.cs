using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePlatform : Platform
{
    bool playerIn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = true;
            Player player = other.GetComponent<Player>();
            player.onObjectUse += Used;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = false;
            Player player = other.GetComponent<Player>();
            player.onObjectUse -= Used;
        }
    }

    void Used()
    {
        if( playerIn )
        {
            isMoving = true;
        }
    }
}
