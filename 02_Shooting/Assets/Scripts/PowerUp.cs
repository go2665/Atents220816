using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float dirChangeTime = 1.0f;
    public float moveSpeed = 0.5f;
    public float selfDestroyTime = 10.0f;

    Player player;        
    Vector2 dir;
    WaitForSeconds waitTime;

    private void Start()
    {
        waitTime = new WaitForSeconds(dirChangeTime);
        player = FindObjectOfType<Player>();
        SetRandomDir();
        StartCoroutine(DirChange());
        Destroy(this.gameObject, selfDestroyTime);
    }

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * dir);
    }

    IEnumerator DirChange()
    {
        while(true)
        {
            yield return waitTime;
            SetRandomDir(false);
        }
    }

    void SetRandomDir(bool allRandom = true)    // 디폴트 파라메터. 값을 지정하지 않으면 디폴트 값이 대신 들어간다.
    {
        if (allRandom)
        {
            dir = Random.insideUnitCircle;
            dir = dir.normalized;
        }
        else
        {
            Vector2 playerToPowerUp = transform.position - player.transform.position;
            playerToPowerUp = playerToPowerUp.normalized;
            if (Random.value < 0.6f)    // 60% 확률로 플레이어 반대방향으로 이동
            {
                // playerToPowerUp 벡터를 z축으로 -90~+90만큼 회전시켜서 dir에 넣기
                dir = Quaternion.Euler(0, 0, Random.Range(-90.0f, 90.0f)) * playerToPowerUp;
            }
            else  // 40% 확률로 플레이어 방향으로 이동
            {
                dir = Quaternion.Euler(0, 0, Random.Range(-90.0f, 90.0f)) * -playerToPowerUp;
            }
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            dir = Vector2.Reflect(dir, collision.contacts[0].normal);
        }
    }
}
