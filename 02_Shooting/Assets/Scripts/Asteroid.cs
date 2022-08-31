using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float rotateSpeed = 360.0f;          // 회전 속도
    public float moveSpeed = 3.0f;              // 이동 속도
    public Vector3 direction = Vector3.left;    // 운석이 이동할 방향
    public int hitPoint = 3;

    GameObject explosion;

    private void Start()
    {
        explosion = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        //transform.rotation *= Quaternion.Euler(new(0, 0, 90));  // 계속 90도씩 회전
        //transform.rotation *= Quaternion.Euler(new(0, 0, rotateSpeed * Time.deltaTime));    // 1초에 rotateSpeed도씩 회전
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);   // Vector3.forward 축을 기준으로 1초에 rotateSpeed도씩 회전

        transform.Translate(moveSpeed * Time.deltaTime * direction, Space.World);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + direction * 1.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            hitPoint--;

            if (hitPoint <= 0)
            {
                explosion.SetActive(true);
                explosion.transform.parent = null;
                Destroy(this.gameObject);
            }
        }
    }
}
