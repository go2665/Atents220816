using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float initialSpeed = 10.0f;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward * initialSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.CompareTag("Player") )
        {
            IDead playerDead = collision.gameObject.GetComponent<IDead>();
            if(playerDead != null)
            {
                playerDead.Die();
            }
        }
        Destroy(this.gameObject, 2.0f);
    }
}
