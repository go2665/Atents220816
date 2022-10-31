using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 생성만하는 클래스. 팩토리 디자인 패턴
/// </summary>
public class ItemFactory
{
    static int itemCount = 0;   // 생성된 아이템 총 갯수. 아이템 생성 아이디의 역할도 함.

    // 함수의 구성 요소 : 이름, 파라메터, 리턴값, 바디(코드)
    // overloading : 이름은 같은데 파라메터가 다른 함수 만드는 것
    // overriding : 이름, 파라메터, 리턴값이 같은 함수를 만드는 것

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

    /// <summary>
    /// 아이템 코드를 이용해 특정 위치에 아이템을 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템 코드</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="randomNoise">위치에 랜덤성을 더할지 여부. true면 약간의 랜덤성을 더한다. 기본값은 false</param>
    /// <returns>생성된 아이템</returns>
    public static GameObject MakeItem(ItemIDCode code, Vector3 position, bool randomNoise = false)
    {
        GameObject obj = MakeItem(code);    // 만들고
        if (randomNoise)                    // 위치에 랜덤성을 더하면
        {
            Vector2 noise = Random.insideUnitCircle * 0.5f; // 반지름 0.5인 원의 안쪽에서 랜덤한 위치 구함
            position.x += noise.x;          // 구한 랜덤함을 파라메터로 받은 기존 위치에 추가
            position.z += noise.y;
        }
        obj.transform.position = position;  // 위치지정
        return obj;
    }

    /// <summary>
    /// 아이템 코드를 이용해 아이템을 한번에 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 아이템 코드</param>
    /// <param name="count">생성할 갯수</param>
    /// <returns>생성된 아이템들이 담긴 배열</returns>
    public static GameObject[] MakeItem(ItemIDCode code, int count)
    {
        GameObject[] objs = new GameObject[count];  // 배열 만들고
        for(int i=0;i<count;i++)
        {
            objs[i] = MakeItem(code);               // count만큼 반복해서 아이템 생성
        }
        return objs;                                // 생성한 것 리턴
    }

    /// <summary>
    /// 아이템 코드를 이용해 특정 위치에 아이템을 한번에 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 아이템 코드</param>
    /// <param name="count">생성할 갯수</param>
    /// <param name="position">생성할 기준 위치</param>
    /// <param name="randomNoise">위치에 랜덤성을 더할지 여부. true면 약간의 랜덤성을 더한다. 기본값은 false</param>
    /// <returns>생성된 아이템들이 담긴 배열</returns>
    public static GameObject[] MakeItem(ItemIDCode code, int count, Vector3 position, bool randomNoise = false)
    {
        GameObject[] objs = new GameObject[count];  // 배열 만들고
        for (int i = 0; i < count; i++)
        {
            objs[i] = MakeItem(code, position, randomNoise);    // count만큼 반복해서 아이템 생성. 위치와 노이즈도 적용
        }
        return objs;                                // 생성한 것 리턴
    }


    /// <summary>
    /// 아이템 id로 아이템 생성
    /// </summary>
    /// <param name="id">생성할 아이템 ID</param>
    /// <returns>생성한 아이템</returns>
    public static GameObject MakeItem(int id)
    {
        if (id < 0)
            return null;

        return MakeItem((ItemIDCode)id);
    }

    /// <summary>
    /// 아이템 id를 이용해 특정 위치에 아이템을 생성하는 함수
    /// </summary>
    /// <param name="id">생성할 아이템 아이디</param>
    /// <param name="position">생성할 위치</param>
    /// <returns>생성된 아이템</returns>
    public static GameObject MakeItem(int id, Vector3 position, bool randomNoise = false)
    {
        GameObject obj = MakeItem(id);      // 만들고
        if (randomNoise)                    // 위치에 랜덤성을 더하면
        {
            Vector2 noise = Random.insideUnitCircle * 0.5f; // 반지름 0.5인 원의 안쪽에서 랜덤한 위치 구함
            position.x += noise.x;          // 구한 랜덤함을 파라메터로 받은 기존 위치에 추가
            position.z += noise.y;
        }
        obj.transform.position = position;  // 위치지정
        return obj;
    }

    /// <summary>
    /// 아이템 ID를 이용해 아이템을 한번에 여러개 생성하는 함수
    /// </summary>
    /// <param name="id">생성할 아이템의 아이템 아이디</param>
    /// <param name="count">생성할 갯수</param>
    /// <returns>생성된 아이템들이 담긴 배열</returns>
    public static GameObject[] MakeItem(int id, int count)
    {
        GameObject[] objs = new GameObject[count];  // 배열 만들고
        for (int i = 0; i < count; i++)
        {
            objs[i] = MakeItem(id);                 // count만큼 반복해서 아이템 생성
        }
        return objs;                                // 생성한 것 리턴
    }

    /// <summary>
    /// 아이템 ID를 이용해 특정 위치에 아이템을 한번에 여러개 생성하는 함수
    /// </summary>
    /// <param name="id">생성할 아이템의 아이템 id</param>
    /// <param name="count">생성할 갯수</param>
    /// <param name="position">생성할 기준 위치</param>
    /// <param name="randomNoise">위치에 랜덤성을 더할지 여부. true면 약간의 랜덤성을 더한다. 기본값은 false</param>
    /// <returns>생성된 아이템들이 담긴 배열</returns>
    public static GameObject[] MakeItem(int id, int count, Vector3 position, bool randomNoise = false)
    {
        GameObject[] objs = new GameObject[count];  // 배열 만들고
        for (int i = 0; i < count; i++)
        {
            objs[i] = MakeItem(id, position, randomNoise);    // count만큼 반복해서 아이템 생성. 위치와 노이즈도 적용
        }
        return objs;                                // 생성한 것 리턴
    }
}
