using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler
{
    RectTransform containerRect;
    RectTransform handleRect;

    float stickRange;

    private void Awake()
    {
        containerRect = GetComponent<RectTransform>();
        Transform child = transform.GetChild(0);
        handleRect = child.GetComponent<RectTransform>();

        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }


    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 중");
    }
}
