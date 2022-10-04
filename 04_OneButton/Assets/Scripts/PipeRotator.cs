using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeRotator : MonoBehaviour
{
    /// <summary>
    /// 파이프가 움직이는 속도
    /// </summary>
    public float pipeMoveSpeed = 5.0f;
    
    /// <summary>
    /// 움직일 파이프들
    /// </summary>
    Pipe[] pipes;

    /// <summary>
    /// 파이프가 오른쪽 끝으로 이동할 때 이동하는 위치
    /// </summary>
    float startPointX;

    /// <summary>
    /// 파이프가 오른쪽 끝으로 가게되는 기준점(이것보다 왼쪽이면 이동)
    /// </summary>
    float endPointX;

    private void Awake()
    {
        pipes = GetComponentsInChildren<Pipe>();    // 자식으로 있는 Pipe 모두 찾기
        startPointX = transform.Find("StartPoint").position.x;  // startPointX 구하기
        endPointX = transform.Find("EndPoint").position.x;      // endPointX 구하기
    }

    private void FixedUpdate()
    {        
        foreach(var pipe in pipes)  // pipes에 있는 모든 pipe를 하나씩 처리하기
        {            
            pipe.MoveLeft(pipeMoveSpeed * Time.fixedDeltaTime); // 파이프를 초당 pipeMoveSpeed만큼의 속도로 계속 왼쪽으로 이동 시키기
                        
            if ( endPointX > pipe.transform.position.x)         // 파이프의 위치가 endPointX보다 왼쪽인지 체크
            {
                // 파이프의 위치가 왼쪽에 있으면 startPointX로 이동
                // 파이프의 높이를 랜덤으로 변화시키기                
                pipe.transform.position = new Vector3(startPointX, pipe.RandomHeight, 0);
            }
        }
    }

    public void AddPipeSoredDelegate(Action<int> del)
    {
        foreach(Pipe pipe in pipes)
        {
            pipe.onScored += del;
        }
    }
}
