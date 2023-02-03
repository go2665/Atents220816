using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeploymentInfoMaker : MonoBehaviour
{
    /// <summary>
    /// 함선의 한칸을 표시할 개발용 오브젝트
    /// </summary>
    public GameObject infoPrefab;

    /// <summary>
    /// 함선 종류별로 개발용 오브젝트에 적용할 머티리얼
    /// </summary>
    public Material[] infoMaterials;

    /// <summary>
    /// 함선 종류별로 생성한 개발용 오브젝트의 목록을 가지는 딕셔너리
    /// </summary>
    Dictionary<ShipType, List<GameObject>> infoObjects;

    private void Awake()
    {
        // 딕셔너리 생성(키:함선타입, 값:함선종류별 개발용 오브젝트 들)
        infoObjects = new Dictionary<ShipType, List<GameObject>>(ShipManager.Inst.ShipTypeCount);
        infoObjects[ShipType.Carrier] = new List<GameObject>(); 
        infoObjects[ShipType.Battleship] = new List<GameObject>(); 
        infoObjects[ShipType.Destroyer] = new List<GameObject>(); 
        infoObjects[ShipType.Submarine] = new List<GameObject>(); 
        infoObjects[ShipType.PatrolBoat] = new List<GameObject>(); 
    }

    /// <summary>
    /// 특정 종류의 함선의 개발용 표시 오브젝트들 생성
    /// </summary>
    /// <param name="shipType">생성할 배의 종류</param>
    /// <param name="positions">생성할 위치들</param>
    public void MarkShipDeploymentInfo(ShipType shipType, Vector3[] positions)
    {
        foreach(var pos in positions)   // 모든 위치를 순회하면서
        {
            GameObject obj = Instantiate(infoPrefab, transform);    // 개발용 오브젝트 생성
            Renderer renderer= obj.GetComponent<Renderer>();        // 랜더러 찾아서
            renderer.material = infoMaterials[(int)(shipType - 1)]; // 머티리얼 변경

            obj.transform.position = pos;   // 새 위치 지정
            infoObjects[shipType].Add(obj); // 딕셔너리에 추가
        }
    }

    /// <summary>
    /// 생성한 개발용 표시 오브젝트들을 삭제하는 함수
    /// </summary>
    /// <param name="shipType">삭제할 함선의 종류</param>
    public void UnMarkShipDeploymentInfo(ShipType shipType)
    {
        foreach(var infoObj in infoObjects[shipType])   // 함선종류 별 개발용 오브젝트의 리스트를 순회하면서
        {
            Destroy(infoObj);           // 하나씩 삭제
        }
        infoObjects[shipType].Clear();  // 딕셔너리에 들어있는 리스트를 클리어
    }
}
