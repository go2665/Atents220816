using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item Data", menuName = "Scriptable Object/Item Data - Weapon", order = 5)]
public class ItemData_Weapon : ItemData, IEquipItem
{
    [Header("무기 데이터")]
    public float attackPower = 30;

    public EquipPartType EquipPart => EquipPartType.Weapon;

    public void EquipItem(GameObject target)
    {
    }

    public void ToggleEquipItem(GameObject target)
    {
    }

    public void UnEquipItem(GameObject target)
    {
    }
}
