using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Player : TestBase
{
    public Slider slider;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

}
