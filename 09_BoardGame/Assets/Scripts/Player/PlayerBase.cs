using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    // 변수들 --------------------------------------------------------------------------------------
    
    /// <summary>
    /// 이 플레이어가 가지고 있는 보드.(자식으로 있음)
    /// </summary>
    protected Board board;

    /// <summary>
    /// 이 플레이어가 가지고 있는 함선들
    /// </summary>
    protected Ship[] ships;

    /// <summary>
    /// 아직 침몰하지 않은 함선의 수
    /// </summary>
    protected int remainShipCount;

    /// <summary>
    /// 이번턴 행동 여부. true면 행동 완료, false면 아직 행동하지 않음
    /// </summary>
    bool isActionDone = false;

    /// <summary>
    /// 대전 상대
    /// </summary>
    protected PlayerBase opponent;

    // 공격 관련 변수 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 후보지역을 표시하는데 사용하는 프리팹
    /// </summary>
    public GameObject highCandidatePrefab;

    /// <summary>
    /// 아직 공격하지 않은 모든 지점.
    /// </summary>
    List<int> attackCandiateIndices;

    /// <summary>
    /// 다음에 공격했을 때 성공확률이 높은 지점
    /// </summary>
    List<int> attackHighCandidateIndices;

    /// <summary>
    /// 마지막에 공격을 성공한 위치
    /// </summary>
    Vector2Int lastAttackSuccessPos;

    /// <summary>
    /// invalid한 좌표 표시용.(이번턴에 공격을 성공하지 못한 것을 표시)
    /// </summary>
    readonly Vector2Int NOT_SUCCESS_YET = -Vector2Int.one;

    /// <summary>
    /// 후보지역 표시를 위한 게임 오브젝트 저장하는 딕셔너리
    /// </summary>
    Dictionary<int, GameObject> highCandidateMark = new Dictionary<int, GameObject>();


    // 프로퍼티들 ----------------------------------------------------------------------------------

    public Board Board => board;
    public Ship[] Ships => ships;
    public bool IsDpeat => remainShipCount < 1;


    // 델리게이트 들

    /// <summary>
    /// 플레이어의 공격이 실패했음을 알리는 델리게이트
    /// </summary>
    public Action<PlayerBase> onAttackFail;

    /// <summary>
    /// 플레이어의 행동이 끝났음을 알리는 델리게이트
    /// </summary>
    public Action onActionEnd;

    /// <summary>
    /// 플레이어가 패배했음을 알리는 델리게이트
    /// </summary>
    public Action<PlayerBase> onDefeat;


    // 유니티 이벤트 함수들 -------------------------------------------------------------------------
    protected virtual void Awake()
    {
        board = GetComponentInChildren<Board>();

        int shipTypeCount = ShipManager.Inst.ShipTypeCount;
        ships= new Ship[shipTypeCount];
        for(int i=0;i<shipTypeCount;i++)
        {
            ships[i] = ShipManager.Inst.MakeShip((ShipType)(i + 1), transform);
            ships[i].onSinking += OnShipDestroy;
        }        

        attackCandiateIndices= new List<int>();
    }

    protected virtual void Start()
    {
        int shipTypeCount = ShipManager.Inst.ShipTypeCount;
        remainShipCount = shipTypeCount;    
        
        lastAttackSuccessPos = NOT_SUCCESS_YET;
    }

    // 턴 관리용 함수 ------------------------------------------------------------------------------
    public virtual void OnPlayerTurnStart(int turnNumber)
    {
        isActionDone = true;
    }

    public virtual void OnPlayerTurnEnd()
    {
    }

    // 공격용 함수 ---------------------------------------------------------------------------------

    // - 공격
    public void Attack(Vector2Int attackGridPos)
    {
        if(!isActionDone)
        {

            isActionDone = true;
        }
    }

    public void Attack(Vector3 worldPos)
    {
        Attack(opponent.Board.WorldToGrid(worldPos));
    }

    private void OnShipDestroy(Ship ship)
    {
        throw new NotImplementedException();
    }

    // - 자동 공격

}
