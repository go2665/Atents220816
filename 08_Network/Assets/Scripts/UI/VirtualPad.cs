using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class VirtualPad : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    RectTransform containerRect;
    RectTransform handleRect;

    float stickRange;

    public Action<Vector2> onMoveInput;

    private void Awake()
    {
        containerRect = GetComponent<RectTransform>();
        Transform child = transform.GetChild(0);
        handleRect = child.GetComponent<RectTransform>();

        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }


    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("드래그 중");

        //position은 containerRect의 pivot 기준으로 얼마만큼 이동했는지를 받아옴
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);   

        position = Vector2.ClampMagnitude(position, stickRange);    // 핸들이 stickRange 안쪽으로만 움직이도록 계산
        
        InputUpdate(position);

        // Debug.Log(position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("마우스 업");

        InputUpdate(Vector2.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("마우스 다운");
    }

    /// <summary>
    /// 입력 업데이트(가상 패드에서 어떻게 입력이 들어왔는지 전달)
    /// </summary>
    /// <param name="pos">핸들이 부모에서 얼마나 떨어져 있는지</param>
    void InputUpdate(Vector2 pos)
    {
        handleRect.anchoredPosition = pos;      // 핸들의 위치 설정
        onMoveInput?.Invoke(pos / stickRange);  // 이동 방향을 알림
    }
}
