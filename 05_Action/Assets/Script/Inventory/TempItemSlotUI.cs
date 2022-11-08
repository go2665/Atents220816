using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class TempItemSlotUI : ItemSlotUI
{
    public void Open()
    {
        if(!ItemSlot.IsEmpty)
        {
            gameObject.SetActive(true);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
