using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    public Action<bool> onToggleChange;

    bool isOn = false;

    Image buttonImage;
    Button toggle;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        toggle = GetComponent<Button>();
        toggle.onClick.AddListener(ToggleClick);
    }

    private void ToggleClick()
    {
        SetToggleState(!isOn);
    }

    public void SetToggleState(bool on)
    {
        if( on )
        {
            buttonImage.sprite = onSprite;
        }
        else
        {
            buttonImage.sprite = offSprite;
        }
        isOn = on;
        onToggleChange?.Invoke(isOn);
    }
}
