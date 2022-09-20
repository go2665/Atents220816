using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePlatform : Platform, IUseableObject
{
    bool playerIn = false;

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
        if( playerIn )  // 플레이어가 트리거안에 들어온 상태에서 사용해야 움직이기
        {
            isMoving = true;
        }
    }
}
