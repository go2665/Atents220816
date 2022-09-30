using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollingSpeed = 5.0f;
    public float width = 7.2f;  // 하늘은 7.2, 바닥은 8.4

    Transform[] bgSlots;
    float edgePoint;

    private void Awake()
    {
        bgSlots = new Transform[transform.childCount];
        for(int i=0;i<transform.childCount;i++)
        {
            bgSlots[i] = transform.GetChild(i);
        }
    }

    private void Start()
    {
        edgePoint = transform.position.x - width * 0.9f;
    }

    private void Update()
    {        
        foreach(var slot in bgSlots)
        {
            slot.Translate(scrollingSpeed * Time.deltaTime * -transform.right);
            if( slot.position.x < edgePoint)
            {
                slot.Translate(width * bgSlots.Length * transform.right);
            }
        }
    }
}
