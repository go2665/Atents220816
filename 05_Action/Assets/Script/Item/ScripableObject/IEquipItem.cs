using UnityEngine;

interface IEquipItem
{
    /// <summary>
    /// 이 장비 아이템이 장착될 부위
    /// </summary>
    EquipPartType EquipPart { get; }

    /// <summary>
    /// 아이템 장비하기
    /// </summary>
    /// <param name="target">아이템을 장비할 대상</param>
    /// <param name="slot">아이템이 들어있는 인벤토리 슬롯</param>
    void EquipItem(GameObject target, ItemSlot slot);

    /// <summary>
    /// 아이템을 해제하기
    /// </summary>
    /// <param name="target">아이템을 해제할 대상</param>
    /// <param name="slot">아이템이 들어있는 인벤토리 슬롯</param>
    void UnEquipItem(GameObject target, ItemSlot slot);

    /// <summary>
    /// 아이템을 자연스럽게 장착하고 해제하는 함수
    /// </summary>
    /// <param name="target">장비하고 해제할 대상</param>
    /// <param name="slot">아이템이 들어있는 인벤토리 슬롯</param>
    void AutoEquipItem(GameObject target, ItemSlot slot);
}