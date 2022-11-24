using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackArea : MonoBehaviour
{
    public Action<IBattle> onPlayerIn;
    public Action<IBattle> onPlayerOut;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 공격 범위에 들어옴.");
            IBattle battle = other.GetComponent<IBattle>();
            onPlayerIn?.Invoke(battle);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 공격 범위에서 나감.");
            IBattle battle = other.GetComponent<IBattle>();
            onPlayerOut?.Invoke(battle);
        }
    }
}
