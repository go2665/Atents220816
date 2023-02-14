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
            PrintHP(texts[i], ships[i]);

            int index = i;
            ships[i].onHit += (ship) => PrintHP(texts[index], ship);
            ships[i].onSinking += (ship) => PrintDestroy(texts[index]);
        }
    }

    private void PrintHP(TextMeshProUGUI text, Ship ship)
    {
        text.text = $"{ship.HP}/{ship.Size}";
    }

    private void PrintDestroy(TextMeshProUGUI text)
    {
        text.fontSize = 40;
        text.text = "<#ff0000>Destroy!!</color>";
    }
}
