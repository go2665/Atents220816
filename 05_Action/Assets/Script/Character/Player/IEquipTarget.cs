interface IEquipTarget
{
    // 이 캐릭터가 어떤 부위에 어떤 아이템을 장착하고 있는지를 확인하고 설정할 수 있는 프로퍼티
    ItemData_EquipItem[] PartsItems { get; }

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="part">장비할 위치</param>
    /// <param name="itemData">장비할 아이템</param>
    void EquipItem(EquipPartType part, ItemData_EquipItem itemData);

    /// <summary>
    /// 아이템 장비를 해제하는 함수
    /// </summary>
    /// <param name="part">장비를 해제할 위치</param>
    void UnEquipItem(EquipPartType part);
}