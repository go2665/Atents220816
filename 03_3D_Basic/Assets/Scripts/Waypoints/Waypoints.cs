using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    Transform[] waypoint;
    int index = 0;      // 현재 향하고 있는 웨이포인트의 인덱스

    public Transform CuurentWaypoint { get => waypoint[index]; }

    private void Awake()
    {
        waypoint = new Transform[transform.childCount];
        for(int i=0;i<transform.childCount;i++)
        {
            waypoint[i] = transform.GetChild(i);
        }
    }

    public Transform MoveToNextWaypoint()
    {
        index++;
        index %= waypoint.Length;   // index = index % waypoint.Length;
        return waypoint[index];
    }
}
