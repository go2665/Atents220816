using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleTap : MonoBehaviour
{
    private void Update()
    {
        if( Keyboard.current.anyKey.wasPressedThisFrame 
            || Mouse.current.leftButton.wasPressedThisFrame )
        {
            GameManager.Inst.GameStart();
            Destroy(this.gameObject);
        }
    }
}
