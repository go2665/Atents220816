using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 1개를 표현할 클래스
/// </summary>
public class Item : MonoBehaviour
{
    public ItemData data;   // 아이템의 정보

    private void Start()
    {
        Instantiate(data.modelPrefab, transform.position, transform.rotation, transform);   // 아이템의 외형 추가
    }
}
