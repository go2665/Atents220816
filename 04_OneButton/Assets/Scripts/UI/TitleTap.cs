using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleTap : MonoBehaviour
{
    private void Update()
    {
        if ( Keyboard.current.anyKey.wasPressedThisFrame        // 현재 키보드의 어떤 키라도 이 프레임에 입력이 들어오면 true
            || Mouse.current.leftButton.wasPressedThisFrame )   // 현재 마우스의 왼쪽 버튼이 이번 프레임에 눌러지면 true
        {
            GameManager.Inst.GameStart();   // 게임 메니저에게 시작 신호를 보냄
            Destroy(this.gameObject);       // 자기 자신을 삭제
        }
    }
}
