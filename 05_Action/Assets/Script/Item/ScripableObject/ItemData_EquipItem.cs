using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장비 아이템의 기본 아이템 데이터 클래스. 모든 장비 아이템의 아이템데이터는 이 클래스를 상속받아야 한다.
/// </summary>
public class ItemData_EquipItem : ItemData, IEquipItem
{
    /// <summary>
    /// 아이템을 장비했을 때 보일 프리팹
    /// </summary>
    public GameObject equipPrefab;  

    /// <summary>
    /// 이 아이템이 어떤 부분에 장비 될 것인지를 알려주는 프로퍼티. 상속 받는 곳에서 리턴하는 것을 변경해야만 한다.
    /// </summary>
    public virtual EquipPartType EquipPart => EquipPartType.Weapon;

    /// <summary>
    /// 아이템 장비하기
    /// </summary>
    /// <param name="target">아이템을 장비할 대상</param>
    /// <param name="slot">아이템이 들어있는 인벤토리 슬롯</param>
    public virtual void EquipItem(GameObject target, ItemSlot slot)
    {
        // 대상이 아이템을 장비할 수 있는지 확인
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if(equipTarget != null)
        {
            slot.IsEquipped = true;
            equipTarget.EquipItem(EquipPart, slot); // target에게 아이템 장착 시키기
        }
    }

    /// <summary>
    /// 아이템을 해제하기
    /// </summary>
    /// <param name="target">아이템을 해제할 대상</param>
    /// <param name="slot">아이템이 들어있는 인벤토리 슬롯</param>
    public virtual void UnEquipItem(GameObject target, ItemSlot slot)
    {
        // 대상이 아이템을 장비할 수 있는지 확인
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if (equipTarget != null)
        {
            slot.IsEquipped = false;
            equipTarget.UnEquipItem(EquipPart); // target에게 아이템 해제 시키기
        }
    }

    /// <summary>
    /// 아이템을 자연스럽게 장착하고 해제하는 함수
    /// </summary>
    /// <param name="target">장비하고 해제할 대상</param>
    /// <param name="slot">아이템이 들어있는 인벤토리 슬롯</param>
    /// <returns>장비를 했으면 true, 해제를 했으면 false</returns>
    public virtual void AutoEquipItem(GameObject target, ItemSlot slot)
    {
        // 대상이 아이템을 장비할 수 있는지 확인
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if (equipTarget != null)
        {
            // 이 파츠에 아이템이 장비되어있는지 아닌지 확인
            ItemSlot partsSlot = equipTarget.PartsSlots[(int)EquipPart];

            if (partsSlot != null)
            {
                // 지금 장비된 아이템이 있다.
                UnEquipItem(target, partsSlot);    // 장비된 아이템을 장착 해제

                ItemData_EquipItem equipItem = partsSlot.ItemData as ItemData_EquipItem; ;
                if (equipItem != this)
                {
                    EquipItem(target, slot);    // 같은 파츠인데 다른 아이템이 장비 시도되었으면 다른 아이템을 장비
                }
            }
            else
            {
                // 지금 장비된 아이템이 없다.
                EquipItem(target, slot);        // 아이템 장비
            }
        }
    }
}
