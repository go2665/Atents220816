using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Cell : TestBase
{
    public GameObject cell;
    public float distance = 0.65f;
    public int size = 16;

    private void Start()
    {
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                GameObject obj = Instantiate(cell, this.transform);
                obj.transform.position = new Vector3(j * distance, i * distance);
            }
        }
    }
}
