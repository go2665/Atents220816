using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBase : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    Spawner parentSpawner;
    Rigidbody2D rigid;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        parentSpawner = GetComponentInParent<Spawner>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveSpeed * Time.fixedDeltaTime * Vector2.left);

        if(rigid.position.x < -25.0f)
        {
            parentSpawner.RemoveChild(this);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            RunningMan player = collision.gameObject.GetComponent<RunningMan>();
            player.Die();
        }
    }
}
