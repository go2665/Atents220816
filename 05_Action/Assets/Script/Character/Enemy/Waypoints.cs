using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    Transform[] children;
    int index = 0;

    /// <summary>
    /// 현재 웨이포인트를 돌려주는 프로퍼티
    /// </summary>
    public Transform Current => children[index];

    private void Awake()
    {
        // 모든 자식을 웨이포인트로 사용
        children = new Transform[transform.childCount];
        for(int i=0;i<transform.childCount;i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// 다음 웨이포인트 리턴
    /// </summary>
    /// <returns>다음에 이동할 웨이포인트</returns>
    public Transform MoveNext()
    {
        index++;                    // 1증가 시키고
        index %= children.Length;   // 계속 반복을 위해 %연산 사용

        return children[index];
    }
}
