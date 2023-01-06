using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float speed = 3.0f;
    Transform target = null;
    Vector3 offset;

    private void Start()
    {
        target = GameManager.Inst.Player.transform;
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.fixedDeltaTime);
    }
}
