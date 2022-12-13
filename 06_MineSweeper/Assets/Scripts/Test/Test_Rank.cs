using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SortTest
{
    public int a;
    public float b;

    public SortTest(int _a, float _b)
    {
        a = _a;
        b = _b;
    }

    public void Print()
    {
        Debug.Log($"a : {a}, b : {b}");
    }
}

public class Test_Rank : TestBase
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        List<int> list = new List<int>();
        list.Add(20);
        list.Add(10);
        list.Add(40);
        list.Add(30);
        list.Add(50);

        list.Sort();

        foreach(var num in list)
        {
            Debug.Log($"{num}");
        }
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        List<SortTest> list = new List<SortTest>();
        list.Add(new SortTest(10, 30.5f));
        list.Add(new SortTest(40, 20.5f));
        list.Add(new SortTest(30, 50.5f));
        list.Add(new SortTest(50, 10.5f));
        list.Add(new SortTest(20, 40.5f));

        list.Sort( (target1, target2) =>
        {
            if( target1.a > target2.a )
            {
                return 1;
            }
            else if(target1.a < target2.a)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });

        foreach (var num in list)
        {
            num.Print();
        }
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        List<SortTest> list = new List<SortTest>();
        list.Add(new SortTest(10, 30.5f));
        list.Add(new SortTest(40, 20.5f));
        list.Add(new SortTest(30, 50.5f));
        list.Add(new SortTest(50, 10.5f));
        list.Add(new SortTest(20, 40.5f));

        list.Sort(SortB);

        foreach (var num in list)
        {
            num.Print();
        }
    }

    int SortB(SortTest target1, SortTest target2)
    {
        return 0;
    }
}
