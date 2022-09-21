using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 트리거에 들어온 대상이 죽을 수 있으면 무조건 죽인다.
        IDead target = other.GetComponent<IDead>(); 
        if(target != null)
        {
            target.Die();
        }
    }
}
