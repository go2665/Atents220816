using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBase : MonoBehaviour
{
    PlayerInputActions inputActions;

    protected virtual void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    protected virtual void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test1.performed += Test1;
        inputActions.Test.Test2.performed += Test2;
        inputActions.Test.Test3.performed += Test3;
        inputActions.Test.Test4.performed += Test4;
        inputActions.Test.Test5.performed += Test5;
    }
    protected virtual void OnDisable()
    {
        inputActions.Test.Test5.performed -= Test5;
        inputActions.Test.Test4.performed -= Test4;
        inputActions.Test.Test3.performed -= Test3;
        inputActions.Test.Test2.performed -= Test2;
        inputActions.Test.Test1.performed -= Test1;
        inputActions.Test.Disable();
    }

    protected virtual void Test1(InputAction.CallbackContext _)
    {
    }

    protected virtual void Test2(InputAction.CallbackContext _)
    {
    }

    protected virtual void Test3(InputAction.CallbackContext _)
    {
    }

    protected virtual void Test4(InputAction.CallbackContext _)
    {
    }

    protected virtual void Test5(InputAction.CallbackContext _)
    {
    }
}
