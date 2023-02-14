using System;
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

    // 델리게이트 ----------------------------------------------------------------------------------

    /// <summary>
    /// 함선이 배치되거나 배치 해제가 되었을 때 실행되는 델리게이트
    /// 파라메터 : 배치할 때 true, 배치 해제할 때 false
    /// </summary>
    public Action<bool> onDeploy;

    /// <summary>
    /// 함선이 공격을 당했을 때 실행될 델리게이트
    /// 파라메터 : 자기 자신
    /// </summary>
    public Action<Ship> onHit;

    /// <summary>
    /// 함선이 침몰했을 때 실행될 델리게이트
    /// 파라메터 : 자기 자신
    /// </summary>
    public Action<Ship> onSinking;
    
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
    public ShipDirection Direction
    { 
        get => direction; 
        set
        {
            direction = value;
            model.rotation = Quaternion.Euler(0, (int)direction * 90.0f, 0);
        }
    }

    /// <summary>
    /// 배의 크기 확인용 프로퍼티. 읽기 전용. 배의 종류에 따라 결정됨
    /// </summary>
    public int Size => size;

    /// <summary>
    /// 배의 현재 HP 확인용 프로퍼티. 읽기 전용.
    /// </summary>
    public int HP
    {
        get => hp;
        private set
        {
            hp = value;
            if( hp <= 0 && isAlive )        // HP가 0 이하인데 살아있으면
            {
                OnSinking();                // 함선 침몰
            }
        }
    } 

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

    ///// <summary>
    ///// 배의 랜더러 접근용 프로퍼티. 읽기 전용.
    ///// </summary>
    //public Renderer ShipRenderer => shipRenderer;


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
        
        shipName = ShipManager.Inst.ShipNames[(int)type - 1];   // 함선 이름 설정
                
        model = transform.GetChild(0);                          // 함선의 모델링 트랜스폼
        shipRenderer = model.GetComponentInChildren<Renderer>();    // 함선 모델링의 랜더러
        
        // 모든 함선 공통
        ResetData();
    }

    void ResetData()
    {
        hp = size;                          // HP는 크기를 그대로 사용
        Direction = ShipDirection.North;    // 함선 방향 초기화(반드시 프로퍼티를 초기화 해야 함선 방향도 초기화가 된다.)
        isAlive = true;                     // 생존 여부 초기화
        isDeployed = false;                 // 배치 여부 초기화
        positions = null;                   // 함선의 칸별 위치
    }

    /// <summary>
    /// 함선의 머티리얼을 선택하는 함수
    /// </summary>
    /// <param name="isNormal">true면 normal 머티리얼, false면 deployMode 머티리얼</param>
    public void SetMaterialType(bool isNormal = true)
    {
        if( isNormal)
        {
            shipRenderer.material = ShipManager.Inst.NormalShipMaterial;
        }
        else
        {
            shipRenderer.material = ShipManager.Inst.DeployModeShipMaterial;
        }
    }

    /// <summary>
    /// 함선이 배치될 때 실행되는 함수
    /// </summary>
    /// <param name="deployPositions">배치되는 위치들</param>
    public void Deploy(Vector2Int[] deployPositions)
    {
        positions = deployPositions;    // 배치된 위치 기록
        isDeployed = true;              // 배치 되었다고 표시
        onDeploy?.Invoke(true);         // 배치 되었다고 알림
    }

    /// <summary>
    /// 함선이 배치 해제 되었을 때 실행되는 함수
    /// </summary>
    public void UnDeploy()
    {
        ResetData();
        onDeploy?.Invoke(false);
    }

    /// <summary>
    /// 함선을 90도씩 회전 시키는 함수
    /// </summary>
    /// <param name="isCCW">true면 반시계방향으로 회전. false면 시계방향으로 회전</param>
    public void Rotate(bool isCCW)
    {
        // Direction을 어떻게 수정해야 하나?
        int count = ShipManager.Inst.ShipDirectionCount;
        if( isCCW )
        {
            Direction = (ShipDirection)(((int)(Direction) + count - 1) % count);
        }
        else
        {
            Direction = (ShipDirection)(((int)(Direction) + 1) % count); ;
        }
    }

    /// <summary>
    /// 함선을 랜덤한 방향으로 회전시키는 함수
    /// </summary>
    public void RandomRotate()
    {
        int rotateCount = UnityEngine.Random.Range(0, ShipManager.Inst.ShipDirectionCount); // 바라볼 방향 결정
        bool isCCW = UnityEngine.Random.Range(0,2) == 0;    // 시계방향/반시계바향 고르기
        for(int i=0;i<rotateCount;i++) 
        {
            Rotate(isCCW);  // 바라볼 방향을 향해 회전
        }
    }

    /// <summary>
    /// 함선이 공격 받았을 때 실행되는 함수
    /// </summary>
    public void OnAttacked()
    {
        Debug.Log($"{type}이 공격 받음");
        HP--;

        if(IsAlive)
        {
            onHit?.Invoke(this);    // 함선이 데미지를 입었다고 알림.
        }
    }

    /// <summary>
    /// 함선이 침몰할 때 실행되는 함수
    /// </summary>
    private void OnSinking()
    {
        Debug.Log($"{type}이 침몰했습니다.");
        isAlive = false;            // 함선이 침몰했다고 기록
        onSinking?.Invoke(this);    // 함선이 침몰했다고 알림
    }


}
