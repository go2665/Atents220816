using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : SpawnBase
{
    float gap = 1.25f;

    protected override void Awake()
    {
        base.Awake();

        transform.position += Random.Range(0, 3) * gap * Vector3.up;
    }
}
