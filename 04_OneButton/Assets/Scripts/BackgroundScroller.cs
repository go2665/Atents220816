using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    /// <summary>
    /// 배경 스크롤 속도
    /// </summary>
    public float scrollingSpeed = 5.0f;

    /// <summary>
    /// 배경 이미지의 가로 크기
    /// </summary>
    public float width = 7.2f;  // 하늘은 7.2, 바닥은 8.4

    /// <summary>
    /// 배경 이미지 오브젝트들의 트랜스폼
    /// </summary>
    Transform[] bgSlots;

    /// <summary>
    /// 이미지가 반대쪽으로 넘어갈 위치(충분히 왼쪽으로 이동한 지점)
    /// </summary>
    float endPoint;

    private void Awake()
    {
        // Awake : 이 오브젝트안에 있는 것을 처리할 때 사용.
        bgSlots = new Transform[transform.childCount];  // 자식 갯수만큼 bgSlots 배열 크기 잡기
        for(int i=0;i<transform.childCount;i++)
        {
            bgSlots[i] = transform.GetChild(i); //bgSlots에 들어갈(이미지가 있는 자식 오브젝트) 트랜스폼들 넣기
        }
    }

    private void Start()
    {
        // Start : 이 오브젝트 바깥에 있는 것들에 접근을 할 때 사용. 또는 정확히 게임이 시작될 때 처리해야할 일들을 할 때 사용
        endPoint = transform.position.x - width * 0.9f;
    }

    private void Update()
    {        
        foreach(var slot in bgSlots)    // bgSlots안에 있는 오브젝트들 순차적으로 처리
        {
            slot.Translate(scrollingSpeed * Time.deltaTime * -transform.right); // 초당 scrollingSpeed만큼의 속도로 왼쪽으로 이동
            if ( slot.position.x < endPoint)    // 이동 후에 endPoint보다 왼쪽에 있으면
            {
                slot.Translate(width * bgSlots.Length * transform.right);   // 오른쪽 끝으로 보내기
            }
        }
    }
}
