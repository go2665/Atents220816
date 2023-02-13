using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeploymentToggle : MonoBehaviour
{
    Image image;
    Button button;

    readonly Color selectedColor = new(1, 1, 1, 0.2f);

    bool isDeployed = false;
    private bool IsDeployed
    {
        get => isDeployed;
        set
        {
            isDeployed = value;
            if( isDeployed )
            {
                image.color = selectedColor;
            }
            else
            {
                image.color = Color.white;
            }
        }
    }

    private void Awake()
    {
        image= GetComponent<Image>();
        button= GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        IsDeployed = !IsDeployed;
    }
}
