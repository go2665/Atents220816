using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

// IPointerClickHandler : 마우스 입력을 받을 수 있다는 인터페이스
public class StartScreen : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Close();
        GameManager.Inst.GameStart();
    }

    void Update()
    {
        // 현재 사용중인 키보드에서 어떤 키든 이 프레임에 눌러졌을 때 true다.
        if( Keyboard.current.anyKey.wasPressedThisFrame )
        {
            Close();
            GameManager.Inst.GameStart();
        }
    }

    void Close()
    {
        this.gameObject.SetActive(false);
    }
}
