using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    RectTransform containerRect;
    RectTransform handleRect;

    float stickRange;

    Action<Vector2> onMoveInput;

    private void Awake()
    {
        containerRect = GetComponent<RectTransform>();
        Transform child = transform.GetChild(0);
        handleRect = child.GetComponent<RectTransform>();

        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }


    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("드래그 중");

        //position은 containerRect의 pivot 기준으로 얼마만큼 이동했는지를 받아옴
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);   

        position = Vector2.ClampMagnitude(position, stickRange);    // 핸들이 stickRange 안쪽으로만 움직이도록 계산
        handleRect.anchoredPosition = position;                     // 핸들의 위치 설정

        position /= stickRange;     // 최대값이 1이 되도록 설정
        NetPlayer player = GameManager.Inst.Player;
        if (player != null)
        {
            //player.SetInputDir(ref position);
            player.SetInputDir(position);
        }


        Debug.Log(position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("마우스 업");
        handleRect.anchoredPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("마우스 다운");
    }
}
