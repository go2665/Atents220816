using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    private void Start()
    {
        if( target == null )
        {
            target = GameManager.Inst.Player.transform;
        }

        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        transform.position = target.position + offset;
    }
}
