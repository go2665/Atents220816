using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5.0f;
    public float lifeTime = 3.0f;

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
        // this는 이 클래스의 인스턴스(자기 자신)
    }

    private void Update()
    {
        //transform.Translate(speed * Time.deltaTime * new Vector3(1,0) );
        transform.Translate(speed * Time.deltaTime * Vector3.right , Space.Self );  // Space.Self : 자기 기준, Space.World : 씬 기준
    }

    // 충돌한 대상이 컬라이더일 때 실행
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"Collision : {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }

        // 매우 좋지 못한 코드
        //if (collision.gameObject.tag == "Enemy") 
        //{
        //}
        // 1. CompareTag는 숫자와 숫자를 비교하지만 == 은 문자열 비교라서 더 느리다.
        // 2. 필요 없는 가비지가 생긴다.

    }


    //// 충돌한 대상이 트리거일 때 실행
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log($"Trigger : {collision.gameObject.name}");
    //}
}
