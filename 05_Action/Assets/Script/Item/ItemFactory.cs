using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 생성만하는 클래스. 팩토리 디자인 패턴
/// </summary>
public class ItemFactory
{
    static int itemCount = 0;   // 생성된 아이템 총 갯수. 아이템 생성 아이디의 역할도 함.

    /// <summary>
    /// ItemIDCode로 아이템 생성
    /// </summary>
    /// <param name="code">생성할 아이템 코드</param>
    /// <returns>생성 결과</returns>
    public static GameObject MakeItem(ItemIDCode code)
    {
        GameObject obj = new GameObject();

        Item item = obj.AddComponent<Item>();           // Item 컴포넌트 추가하기
        item.data = GameManager.Inst.ItemData[code];    // ItemData 할당

        string[] itemName = item.data.name.Split("_");  // 00_Ruby => 00 Ruby로 분할
        obj.name = $"{itemName[1]}_{itemCount++}";      // 오브젝트 이름 설정하기
        obj.layer = LayerMask.NameToLayer("Item");      // 레이어 설정

        SphereCollider sc = obj.AddComponent<SphereCollider>(); // 컬라이더 추가
        sc.isTrigger = true;
        sc.radius = 0.5f;
        sc.center = Vector3.up;

        return obj;
    }




    public static GameObject MakeItem(uint id)
    {
        return MakeItem((ItemIDCode)id);
    }
}
