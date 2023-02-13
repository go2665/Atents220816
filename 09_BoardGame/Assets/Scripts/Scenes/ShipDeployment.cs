using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployment : MonoBehaviour
{
    private void Start()
    {
        GameManager.Inst.GameState = GameState.ShipDeployment;
    }
}
