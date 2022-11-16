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
    void EquipItem(GameObject target);

    /// <summary>
    /// 아이템을 해제하기
    /// </summary>
    /// <param name="target">아이템을 해제할 대상</param>
    void UnEquipItem(GameObject target);

    /// <summary>
    /// 아이템을 자연스럽게 장착하고 해제하는 함수
    /// </summary>
    /// <param name="target">장비하고 해제할 대상</param>
    /// <returns>true면 아이템 장비, false 이이템 해제</returns>
    bool AutoEquipItem(GameObject target);
}