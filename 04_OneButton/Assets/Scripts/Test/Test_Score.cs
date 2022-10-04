using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Test_Score : MonoBehaviour
{
    public ImageNumber imageNumber;
    public TMP_InputField inputField;

    BirdInputActions inputActions;

    private void Awake()
    {
        inputActions = new BirdInputActions();
    }

    private void Start()
    {
        inputField.onValueChanged.AddListener(OnInputChanged);
    }

    private void OnInputChanged(string text)
    {
        if (text != "")
        {
            imageNumber.Number = int.Parse(text);
        }
        else
        {
            imageNumber.Number = 0;
        }
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

    private void TestInput9(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 9;
    }

    private void TestInput8(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 8;
    }

    private void TestInput7(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 7;
    }

    private void TestInput6(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 6;
    }

    private void TestInput5(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 5;
    }

    private void TestInput4(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 4;
    }

    private void TestInput3(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 123456;
    }

    private void TestInput2(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 123;
    }

    private void TestInput1(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 12;
    }

    private void TestInput0(InputAction.CallbackContext obj)
    {
        //imageNumber.Number = 0;
    }

}
