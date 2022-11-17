using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ItemData_EquipItem item = GameManager.Inst.ItemData[ItemIDCode.SilverSword] as ItemData_EquipItem;
        GameManager.Inst.Player.EquipItem(EquipPartType.Weapon, item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
