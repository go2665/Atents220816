using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트를 가진 오브젝트는 y축을 기준으로 계속 회전(시계방향)하면서 위아래로 올라갔다 내려갔다 한다(삼각함수 활용).
public class ItemRotator : MonoBehaviour
{
    public float rotateSpeed = 360.0f;  // 오브젝트의 회전 속도
    public float minHeight = 0.5f;      // 오브젝트의 가장 낮은 높이
    public float maxHeight =1.5f;       // 오브젝트의 가장 높은 높이

    float timeElapsed;                  // 전체 진행 시간(cos에 사용할 용도)
    float halfDiff;                     // 계산 캐싱용
    Vector3 newPosition;                // 아이템의 새로운 위치

    private void Start()
    {
        newPosition = transform.position;   // 현재 위치로 newPosition을 설정
        newPosition.y = minHeight;          // newPosition의 y 값을 가장 낮은 높이값으로 설정        
        transform.position = newPosition;   // 오브젝트의 위치를 newPosition으로 설정
        
        timeElapsed = 0.0f;                 // 시간 누적값 초기화
        halfDiff = 0.5f * (maxHeight - minHeight);  // 캐싱용 계산 결과 저장
    }

    private void Update()
    {
        //Mathf.Deg2Rad * 180.0f; // 1파이
        //Mathf.Rad2Deg * pi; // 180도

        timeElapsed += Time.deltaTime * 2;  // 시간은 계속 누적시킴( 한번 왕복하는데 3.1415....... 초가 걸린다)
        newPosition.x = transform.parent.position.x;                            // 부모의 x,z 위치는 계속 적용
        newPosition.z = transform.parent.position.z;
        newPosition.y = minHeight + (1 - Mathf.Cos(timeElapsed)) * halfDiff;    // 높이값은 cos 그래프를 이용해 계산
        transform.position = newPosition;   // 계산이 끝난 newPosition으로 위치 옮기기

        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);   // 제자리에서 빙글빙글 돌리기
    }
}
