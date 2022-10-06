using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Test_Base : MonoBehaviour
{
    BirdInputActions inputActions;

    protected virtual void Awake()
    {
        inputActions = new BirdInputActions();
    }

    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test0.performed += TestInput0;
        inputActions.Test.Test1.performed += TestInput1;
        inputActions.Test.Test2.performed += TestInput2;
        inputActions.Test.Test3.performed += TestInput3;
        inputActions.Test.Test4.performed += TestInput4;
        inputActions.Test.Test5.performed += TestInput5;
        inputActions.Test.Test6.performed += TestInput6;
        inputActions.Test.Test7.performed += TestInput7;
        inputActions.Test.Test8.performed += TestInput8;
        inputActions.Test.Test9.performed += TestInput9;
    }

    private void OnDisable()
    {
        inputActions.Test.Test0.performed -= TestInput0;
        inputActions.Test.Test1.performed -= TestInput1;
        inputActions.Test.Test2.performed -= TestInput2;
        inputActions.Test.Test3.performed -= TestInput3;
        inputActions.Test.Test4.performed -= TestInput4;
        inputActions.Test.Test5.performed -= TestInput5;
        inputActions.Test.Test6.performed -= TestInput6;
        inputActions.Test.Test7.performed -= TestInput7;
        inputActions.Test.Test8.performed -= TestInput8;
        inputActions.Test.Test9.performed -= TestInput9;
        inputActions.Test.Disable();
    }

    protected virtual void TestInput1(InputAction.CallbackContext obj)
    {
    }

    protected virtual void TestInput2(InputAction.CallbackContext obj)
    {
    }

    protected virtual void TestInput3(InputAction.CallbackContext obj)
    {
    }

    protected virtual void TestInput4(InputAction.CallbackContext obj)
    {
    }

    protected virtual void TestInput5(InputAction.CallbackContext obj)
    {
    }

    protected virtual void TestInput6(InputAction.CallbackContext obj)
    {
    }

    protected virtual void TestInput7(InputAction.CallbackContext obj)
    {
    }

    protected virtual void TestInput8(InputAction.CallbackContext obj)
    {
    }

    protected virtual void TestInput9(InputAction.CallbackContext obj)
    {
    }

    protected virtual void TestInput0(InputAction.CallbackContext obj)
    {
    }

}
