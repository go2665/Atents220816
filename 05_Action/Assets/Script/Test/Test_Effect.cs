using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Effect : TestBase
{
    TrailRenderer trail;
    LineRenderer line;

    private void Start()
    {
        trail = FindObjectOfType<TrailRenderer>();
        line = FindObjectOfType<LineRenderer>();
        line.startColor = Color.green;
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        trail.time = 0.1f;
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        trail.time = 3.0f;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        trail.startWidth = 0.0f;
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        trail.startWidth = 2.0f;
    }
}
