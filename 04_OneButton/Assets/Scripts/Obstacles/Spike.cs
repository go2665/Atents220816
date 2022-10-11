using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : SpawnBase
{
    public Action onDie;

    private void OnDisable()
    {
        onDie?.Invoke();
    }

}
