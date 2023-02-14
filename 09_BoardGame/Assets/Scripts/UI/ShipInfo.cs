using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipInfo : MonoBehaviour
{
    public PlayerBase player;

    TextMeshProUGUI[] texts;

    private void Awake()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Ship[] ships = player.Ships;
        for(int i=0;i<ships.Length;i++)
        {
            texts[i].text = $"{ships[i].HP}/{ships[i].Size}";
        }
    }
}
