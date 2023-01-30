using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetBall : NetworkBehaviour
{
    public float speed = 10.0f;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        transform.Rotate(-30.0f, 0, 0);
        rigid.velocity = transform.forward * speed;
    }
}
