using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishDeploymentButton : MonoBehaviour
{
    UserPlayer player;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
        foreach (var ship in player.Ships)
        {
            ship.onDeploy += OnShipDeployed;
        }

        button.interactable = false;
    }

    private void OnShipDeployed(bool isDeployed)
    {
        if (isDeployed && player.IsAllDeployed)
        {
            OnComplete();
        }
        else
        {
            OnNotComplete();
        }
    }

    private void OnClick()
    {
        Debug.Log("클릭");
    }

    private void OnComplete()
    {
        button.interactable = true;
    }

    private void OnNotComplete()
    {
        button.interactable = false;
    }
}
