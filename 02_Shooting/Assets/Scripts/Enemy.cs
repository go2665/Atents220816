using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;
    GameObject explosion;

    private void Start()
    {
        explosion = transform.GetChild(0).gameObject;
        //explosion.SetActive(false); // 활성화 상태를 끄기(비활성화)
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left, Space.World );
        //transform.Translate(speed * Time.deltaTime * new Vector3(-1,0), Space.Self);  // new로 새로 만들기 때문에 Vector3.left를 쓰는 것보다는 느리다.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.CompareTag("Bullet") )
        {
            //GameObject obj = Instantiate(explosion, transform.position, Quaternion.identity);
            //Destroy(obj, 0.42f);
            explosion.SetActive(true);  // 총알에 맞았을 때 익스플로젼을 활성화 시키고
            explosion.transform.parent = null;  // 익스플로전의 부모(Enemy) 연결을 제거한다.

            Destroy(this.gameObject);   // Enemy를 파괴한다.
        }
    }
}
