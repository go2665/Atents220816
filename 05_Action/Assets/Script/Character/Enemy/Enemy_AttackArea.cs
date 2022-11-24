using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackArea : MonoBehaviour
{
    public Action onPlayerIn;
    public Action onPlayerOut;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 공격 범위에 들어옴.");
            onPlayerIn?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 공격 범위에서 나감.");
            onPlayerOut?.Invoke();
        }
    }
}
