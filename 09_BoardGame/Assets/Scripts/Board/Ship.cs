using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    // 변수 ---------------------------------------------------------------------------------------
    
    /// <summary>
    /// 배의 이름
    /// </summary>
    string shipName;

    /// <summary>
    /// 배의 종류. 배의 크기 및 최대 HP 결정
    /// </summary>
    ShipType type = ShipType.None;

    /// <summary>
    /// 배가 바라보는 방향. 기본적으로 북동남서 순서가 정방향 회전 순서. 
    /// </summary>
    ShipDirection direction = ShipDirection.North;

    /// <summary>
    /// 배의 크기. 초기화할 때 배의 종류에 맞게 설정된다.
    /// </summary>
    int size = 0;

    /// <summary>
    /// 배의 현재 HP. 초기화할 때 배의 크기에 맞게 설정되고 0이 되면 침몰
    /// </summary>
    int hp = 0;

    /// <summary>
    /// 배의 생존 여부. 기본값 true.
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// 배의 배치 여부. 기본값 false.
    /// </summary>
    bool isDeployed = false;
    
    /// <summary>
    /// 배가 배치된 위치. 배의 각 칸들의 위치
    /// </summary>
    Vector2Int[] positions = null;

    /// <summary>
    /// 배의 모델 색상 변경 용
    /// </summary>
    Renderer shipRenderer = null;

    /// <summary>
    /// 배의 모델 부분의 트랜스폼
    /// </summary>
    Transform model;

    
    
    // 프로퍼티 ------------------------------------------------------------------------------------
    
    /// <summary>
    /// 배 이름 확인용 프로퍼티. 읽기 전용.
    /// </summary>
    public string ShipName => shipName;

    /// <summary>
    /// 배 종류 확인용 프로퍼티. 읽기 전용.
    /// </summary>
    public ShipType Type => type;

    /// <summary>
    /// 배의 방향 확인용 프로퍼티. 읽기 전용.
    /// </summary>
    public ShipDirection Direction => direction;

    /// <summary>
    /// 배의 크기 확인용 프로퍼티. 읽기 전용. 배의 종류에 따라 결정됨
    /// </summary>
    public int Size => size;

    /// <summary>
    /// 배의 현재 HP 확인용 프로퍼티. 읽기 전용.
    /// </summary>
    public int HP => hp;

    /// <summary>
    /// 배의 생존 여부 확인용 프로퍼티. 읽기 전용.
    /// </summary>
    public bool IsAlive => isAlive;

    /// <summary>
    /// 배의 배치 여부 확인용 프로퍼티. 읽기 전용.
    /// </summary>
    public bool IsDeployed => isDeployed;

    /// <summary>
    /// 배의 각 칸별 위치 확인용 프로퍼티. 읽기 전용.
    /// </summary>
    public Vector2Int[] Positions => positions;

    /// <summary>
    /// 배의 랜더러 접근용 프로퍼티. 읽기 전용.
    /// </summary>
    public Renderer ShipRenderer => shipRenderer;


    // 함수들 --------------------------------------------------------------------------------------

    /// <summary>
    /// 함선 생성 직후에 함선 타입에 맞춰 각종 초기화 작업을 하기 위한 함수
    /// </summary>
    /// <param name="shipType">이 함선의 타입</param>
    public void Initialize(ShipType shipType)
    {
        // 함선의 타입별 처리
        type = shipType;
        switch (type)
        {
            case ShipType.Carrier:      // 항공모함은 5칸
                size = 5;                
                break;
            case ShipType.Battleship:   // 전함은 4칸
                size = 4;
                break;
            case ShipType.Destroyer:    // 구축함은 3칸
                size = 3;
                break;
            case ShipType.Submarine:    // 잠수함은 3칸
                size = 3;
                break;
            case ShipType.PatrolBoat:   // 경비정은 2칸
                size = 2;
                break;
            default:
                break;
        }
        hp = size;      // HP는 크기를 그대로 사용
        shipName = ShipManager.Inst.ShipNames[(int)type - 1];   // 함선 이름 설정

        // 모든 함선 공통
        direction = ShipDirection.North;                        // 함선 방향 초기화
        isAlive = true;                                         // 생존 여부 초기화
        isDeployed = false;                                     // 배치 여부 초기화
        positions = null;                                       // 함선의 칸별 위치
        model = transform.GetChild(0);                          // 함선의 모델링 트랜스폼
        shipRenderer = model.GetComponentInChildren<Renderer>();    // 함선 모델링의 랜더러
    }

    /// <summary>
    /// 함선이 배치될 때 실행되는 함수
    /// </summary>
    /// <param name="deployPositions">배치되는 위치들</param>
    public void Deploy(Vector2Int[] deployPositions)
    {
    }

    /// <summary>
    /// 함선이 배치 해제 되었을 때 실행되는 함수
    /// </summary>
    public void UnDeploy()
    {
    }

    /// <summary>
    /// 함선을 90도씩 회전 시키는 함수
    /// </summary>
    /// <param name="isCCW">true면 반시계방향으로 회전. false면 시계방향으로 회전</param>
    public void Rotate(bool isCCW)
    {
    }

    /// <summary>
    /// 함선이 공격 받았을 때 실행되는 함수
    /// </summary>
    public void OnAttacked()
    {
    }

    /// <summary>
    /// 함선이 침몰할 때 실행되는 함수
    /// </summary>
    private void OnSinking()
    {
    }


}
