using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Pipe : MonoBehaviour
{
    public Pipe pipe;

    private void Update()
    {
        if( Keyboard.current.digit1Key.wasPressedThisFrame )
        {
            pipe.transform.position = Vector3.zero;
            //pipe.ResetRandomHeight();
        }
    }
}
