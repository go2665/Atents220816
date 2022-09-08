using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int score = 50;
    public float rotateSpeed = 360.0f;          // 회전 속도
    public float moveSpeed = 3.0f;              // 이동 속도

    public float minMoveSpeed = 2.0f;
    public float maxMoveSpeed = 4.0f;
    public float minRotateSpeed = 30.0f;
    public float maxRotateSpeed = 360.0f;

    float lifeTime;
    public float minLifeTime = 4.0f;
    public float maxLifeTime = 6.0f;


    public GameObject small;
    [Range(1,16)]
    public int splitCount = 3;

    public Vector3 direction = Vector3.left;    // 운석이 이동할 방향
    public int hitPoint = 3;

    GameObject explosion;
    private System.Action<int> onDead;

    private void Awake()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
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

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);        
        float ratio = (moveSpeed - minMoveSpeed) / (maxMoveSpeed - minMoveSpeed);        
        //rotateSpeed = ratio * (maxRotateSpeed - minRotateSpeed) + minRotateSpeed;
        rotateSpeed = Mathf.Lerp(minRotateSpeed, maxRotateSpeed, ratio);
        //Debug.Log($"calc : {rotateSpeed}");
        //Debug.Log($"Lerp : {Mathf.Lerp(minRotateSpeed, maxRotateSpeed, ratio)}");

        lifeTime = Random.Range(minLifeTime, maxLifeTime);
    }

    private void Start()
    {
        explosion = transform.GetChild(0).gameObject;

        Player player = FindObjectOfType<Player>();
        onDead += player.AddScore;

        StartCoroutine(SelfCrush());
    }

    IEnumerator SelfCrush()
    {
        yield return new WaitForSeconds(lifeTime);

        Crush();
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
                onDead?.Invoke(score);
                Crush();
            }
        }
    }

    void Crush()
    {
        explosion.SetActive(true);
        explosion.transform.parent = null;

        //// 5%, 10%, 85%
        //float randTest = Random.Range(0.0f, 1.0f);
        //if( randTest < 0.05f )
        //{
        //}
        //else if( randTest < 0.15f )
        //{
        //}
        //else
        //{
        //}

        // 5%의 확률 확인하기
        if ( Random.Range(0.0f,1.0f)  < 0.05f )
        {
            // 5% 확률에 당첨되었다.
            splitCount = 20;
        }
        else
        {
            // 95% 확률에 당첨되었다.
            splitCount = Random.Range(3, 6);   // 1/3 확률로 3~5가 나온다.
        }        


        float angleGap = 360.0f / (float)splitCount;    // 작은 운석들의 진행 방향의 사이각
        float rand = Random.Range(0.0f, 360.0f);        // 첫 운석 방향 변화용
        for(int i=0;i<splitCount;i++)
        {
            Instantiate(small, transform.position, Quaternion.Euler(0, 0, (angleGap * i) + rand));
        }

        Destroy(this.gameObject);
    }
}
