using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float dirChangeTime = 1.0f;      // 파워업 아이템의 이동방향이 바뀌는데 걸리는 시간
    public float moveSpeed = 0.5f;          // 이동 속도
    public float selfDestroyTime = 10.0f;   // 스스로 없어지는데 걸리는 시간

    Player player;              // 파워업 아이템의 이동방향 설정에 필요한 플레이어
    Vector2 dir;                // 현재 이동 방향
    WaitForSeconds waitTime;    // 코루틴에서 사용하기 위한 기다리는 시간 간격

    private void Start()
    {
        waitTime = new WaitForSeconds(dirChangeTime);   // dirChangeTime만큼 기다리도록 waitTime 미리 생성
        // Player 타입을 찾기(무조건 한개만 찾기 때문에 Player가 여러개가 있을 경우 어떤 것이 들어올지는 알 수 없다.)        
        player = FindObjectOfType<Player>();            
        SetRandomDir();                 // 랜덤하게 현재 이동 방향 설정
        StartCoroutine(DirChange());    // 코루틴 실행해서 일정 시간 간격으로 이동 방향 변경되게 설정
        Destroy(this.gameObject, selfDestroyTime);      // selfDestroyTime초 후에 스스로 사라지기
    }

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * dir);  // 현재 이동방향으로 토당 moveSpeed만큼 이동하기
    }

    /// <summary>
    /// 이동 방향을 주기적으로 변경하는 코루틴용 함수.
    /// </summary>
    /// <returns></returns>
    IEnumerator DirChange()
    {
        while(true) // 무한 반복
        {
            yield return waitTime;  // dirChangeTime만큼 대기한 후
            SetRandomDir(false);    // 랜덤하게 현재 이동 방향 설정
        }
    }

    /// <summary>
    /// 랜덤하게 현재 이동방향 설정
    /// </summary>
    /// <param name="allRandom">true면 완전 랜덤. false면 플레이어 반대방향으로 이동할 확률이 높다.</param>
    void SetRandomDir(bool allRandom = true)    // 디폴트 파라메터. 값을 지정하지 않으면 디폴트 값이 대신 들어간다.
    {
        if (allRandom)
        {
            dir = Random.insideUnitCircle;  // 반지름 1인 원한의 랜덤한 위치 리턴 => 이 원의 원점에서 랜덤한 위치로 가는 방향 백터 생성
            dir = dir.normalized;   // 단위백터로 수정
        }
        else
        {
            Vector2 playerToPowerUp = transform.position - player.transform.position;   // 플레이어 위치에서 파워업 아이템 위치로 가는 방향백터 계산.
            playerToPowerUp = playerToPowerUp.normalized;   // 단위 백터로 변경
            if (Random.value < 0.4f)    // 40% 확률로 플레이어 방향으로 이동
            {
                playerToPowerUp *= -1.0f;                
            }

            // playerToPowerUp 벡터를 z축으로 -90~+90만큼 회전시켜서 dir에 넣기
            dir = Quaternion.Euler(0, 0, Random.Range(-90.0f, 90.0f)) * playerToPowerUp;
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            // 보더랑 충돌하면 dir 반사
            dir = Vector2.Reflect(dir, collision.contacts[0].normal);
        }
    }
}
