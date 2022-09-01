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

    private void Awake()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        sprite.flipY = false;

        //int rand = Random.Range(0, 100) % 2;
        ////if( rand == 0 )
        ////{
        ////    sprite.flipX = true;
        ////}
        ////else
        ////{
        ////    sprite.flipX = false;
        ////}
        //sprite.flipX = (rand == 0);

        // 코드 해석하고 주석달기(아래3줄)
        int rand = Random.Range(0, 4);  // rand에다가 0(0b_00), 1(0b_01), 2(0b_10), 3(0b_11) 중 하나의 숫자를 랜덤으로 준다.

        // rand & 0b_01 : rand의 제일 오른쪽 비트가 0인지 1인지 확인하는 작업
        // ((rand & 0b_01) != 0) : rand의 제일 오른쪽 비트가 1이면 true, 0이면 false
        sprite.flipX = ((rand & 0b_01) != 0);

        // rand & 0b_10 : rand의 제일 오른쪽에서 두번째 비트가 0인지 1인지 확인하는 작업
        // ((rand & 0b_10) != 0) : rand의 제일 오른쪽에서 두번째 비트가 1이면 true, 0이면 false
        sprite.flipY = ((rand & 0b_10) != 0);


        // 0b_1001110101010
        // 0b_0000000010000


        moveSpeed = Random.Range(2.0f, 4.0f);
        rotateSpeed = Random.Range(30.0f, 360.0f);
    }

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
