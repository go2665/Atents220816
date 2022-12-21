using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimeFactory : TestBase
{
    List<Slime> spawnedList;

    private void Start()
    {
        spawnedList = new List<Slime>(SlimeFactory.Inst.poolSize);
    }


    protected override void Test1(InputAction.CallbackContext _)
    {
        Slime slime = SlimeFactory.Inst.GetSlime();
        float x = Random.Range(-8, 8);
        float y = Random.Range(-4, 4);
        slime.transform.position = new Vector2(x,y);
        spawnedList.Add(slime);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        foreach(Slime slime in spawnedList)
        {
            slime.Die();
        }
        spawnedList.Clear();
    }
}
