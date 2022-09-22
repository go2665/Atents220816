using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    Transform[] waypoint;   // 자식으로 가지고 있는 웨이포인트들
    int index = 0;          // 현재 향하고 있는 웨이포인트의 번호(인덱스)

    public Transform CurrentWaypoint { get => waypoint[index]; }    // 현재 향하고 있는 웨이포인트

    private void Awake()
    {
        waypoint = new Transform[transform.childCount]; // 자식 갯수만큼 배열 확보
        for(int i=0;i<transform.childCount;i++)
        {
            waypoint[i] = transform.GetChild(i);        // 모든 자식을 하나씩 저장
        }
    }

    /// <summary>
    /// 웨이포인트에 도착했을 때 다음 웨이포인트를 알려주는 함수
    /// </summary>
    /// <returns>다음 웨이포인트의 트랜스폼</returns>
    public Transform MoveToNextWaypoint()
    {
        index++;
        index %= waypoint.Length;   // index = index % waypoint.Length;
        return waypoint[index];     // 인덱스를 증가시킨 후 다음 웨이포인트의 트랜스폼 리턴
    }
}
