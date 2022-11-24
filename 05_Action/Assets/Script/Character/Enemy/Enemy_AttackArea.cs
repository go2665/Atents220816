using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy_AttackArea : MonoBehaviour
{
    public Action<IBattle> onPlayerIn;
    public Action<IBattle> onPlayerOut;
    public SphereCollider col;


    private void Awake()
    {
        if(col == null )
        {
            col = GetComponent<SphereCollider>();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, col.radius, 5);
    }
#endif

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
