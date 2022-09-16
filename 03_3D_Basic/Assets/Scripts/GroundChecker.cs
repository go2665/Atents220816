using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public Action onGrounded;

    private void OnTriggerEnter(Collider other)
    {
        onGrounded?.Invoke();
    }
}
