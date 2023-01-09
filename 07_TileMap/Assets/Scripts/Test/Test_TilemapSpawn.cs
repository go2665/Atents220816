using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TilemapSpawn : TestBase
{
    SpawnerManager manager;

    protected override void Awake()
    {
        base.Awake();
        manager = FindObjectOfType<SpawnerManager>();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
    }
}
