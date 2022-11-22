using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Particle : TestBase
{
    ParticleSystem ps;

    private void Start()
    {
        ps = FindObjectOfType<ParticleSystem>();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        ps.Stop();
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        ps.Play();
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        Debug.Log($"Particle count : {ps.particleCount}");
        Debug.Log($"Duration : {ps.main.duration}");
        Debug.Log($"is Play : {ps.isPlaying}");
    }
}
