using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cell : MonoBehaviour
{
    /// <summary>
    /// 닫힌 셀에 어떤 표시가 있는지 나타내는 enum
    /// </summary>
    enum CellMarkState
    {
        None = 0,   // 아무것도 표시 안됨
        Flag,       // 깃발 설치됨
        Question    // 물음표 표시됨
    }

    // 변수  --------------------------------------------------------------------------------------

    /// <summary>
    /// ID가 잘못되었다고 알려주는 const
    /// </summary>
    public const int ID_NOT_VALID = -1;

    /// <summary>
    /// 셀의 ID이면서 위치를 표시하는 역할
    /// </summary>
    int id = ID_NOT_VALID;

    /// <summary>
    /// 셀이 열려있는지 닫혀있는지 여부. true면 열려있고 false면 닫혀있다.
    /// </summary>
    bool isOpen = false;        
    
    /// <summary>
    /// 이 셀에 지뢰가 있는지 없는지 여부. true면 지뢰가 있다. false면 없다.
    /// </summary>
    bool hasMine = false;

    /// <summary>
    /// 이 셀이 닫혀있을 때 표시되고 있는 것
    /// </summary>
    CellMarkState markState = CellMarkState.None;

    /// <summary>
    /// 주변 셀의 지뢰 갯수. 셀이 열렸을 때 표시할 이미지 결정
    /// </summary>
    int aroundMineCount = 0;

    /// <summary>
    /// 이 셀이 들어있는 보드
    /// </summary>
    Board parentBoard;

    /// <summary>
    /// 닫혔을 때 보일 스프라이트 렌더러
    /// </summary>
    SpriteRenderer cover;

    /// <summary>
    /// 열렸을 때 보일 스프라이트 랜더러
    /// </summary>
    SpriteRenderer inside;

    /// <summary>
    /// 이 셀에 의해 눌려진 셀의 목록(자기 자신 or 자기 주변에 닫혀있던 셀)
    /// </summary>
    List<Cell> pressedCells;

    /// <summary>
    /// 이 셀의 주변 셀들
    /// </summary>
    List<Cell> neighbors;


    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// ID 확인 및 설정용 프로퍼티(설정은 한번만 가능)
    /// </summary>
    public int ID
    {
        get => id;
        set
        {
            if( id == ID_NOT_VALID )    // ID는 처음 한번만 설정 가능
            {
                id = value;
            }
        }
    }

    /// <summary>
    /// 이 셀이 소속되어있는 보드 확인 및 설정용 프로퍼티(설정은 한번만 가능)
    /// </summary>
    public Board Board
    {
        get => parentBoard;
        set
        {
            if( parentBoard == null )
            {
                parentBoard = value;
            }
        }
    }

    /// <summary>
    /// 셀이 열렸는지 닫혔는지 확인하는 프로퍼티
    /// </summary>
    public bool IsOpen => isOpen;

    /// <summary>
    /// 셀에 지뢰가 있는지 확인하는 프로퍼티
    /// </summary>
    public bool HasMine => hasMine;

    /// <summary>
    /// 셀에 깃발이 표시되어 있는지 확인하는 프로퍼티
    /// </summary>
    public bool IsFlaged => markState == CellMarkState.Flag;

    /// <summary>
    /// 셀에 물음표가 표시되어 있는지 확인하는 프로퍼티
    /// </summary>
    public bool IsQuestion => markState == CellMarkState.Question;

    // 델리게이트 ----------------------------------------------------------------------------------
    public Action onFlagUse;
    public Action onFlagReturn;


    // 함수 ---------------------------------------------------------------------------------------

    private void Awake()
    {
        pressedCells = new List<Cell>(8);               // 새로 메모리 할당

        Transform child = transform.GetChild(0);
        cover = child.GetComponent<SpriteRenderer>();
        child = transform.GetChild(1);
        inside = child.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        neighbors = Board.GetNeighbors(this.ID);
    }

    /// <summary>
    /// 셀을 여는 함수
    /// </summary>
    void Open()
    {
        if (!isOpen && !IsFlaged)               // 닫혀있고 깃발 표시가 안되었을 때만 연다.
        {
            isOpen = true;                      // 열렸다고 표시하고
            if(hasMine)                         // 지뢰가 있으면
            {
                inside.sprite = Board[OpenCellType.Mine_Explosion]; // 터지는 이미지로 변경
                GameManager.Inst.GameOver();
            }
            cover.gameObject.SetActive(false);  // 셀이 열릴 때 커버를 비활성화

            if (aroundMineCount == 0 && !hasMine)   // 주변 지뢰갯수가 0이면 
            {
                foreach (var cell in neighbors) // 주변 셀들을
                {
                    cell.Open();                // 모두 연다.(재귀호출)
                }
            }
        }
    }

    /// <summary>
    /// 마우스 왼쪽 버튼이 이 셀을 눌렀을 때 실행될 함수.
    /// </summary>
    public void CellPress()
    {
        if (GameManager.Inst.IsPlaying)
        {
            Debug.Log($"CellPress 실행 - {this.gameObject.name}");
            pressedCells.Clear();   // 새롭게 눌려졌으니 기존에 눌려져 있던 셀에 대한 기록은 제거
            if (IsOpen)
            {
                // 이 셀이 열려져 있으면, 자신 주변의 닫힌 셀을 모두 누른 표시를 한다.
                foreach (var cell in neighbors) // 주변 셀을 모두 순회하기
                {
                    if (!cell.IsOpen)            // 주변 셀 중에 닫혀있는셀만 
                    {
                        pressedCells.Add(cell); // 누르고 있는 셀이라고 표시하고
                        cell.CellPress();       // 누르고 있는 표시 진행
                        Debug.Log($"{cell.gameObject.name} 눌렀다.");
                    }
                }
            }
            else
            {
                // 이 셀이 닫힌 셀일 때 자신을 누른 표시를 한다.
                PressCover();
                Debug.Log($"{this.gameObject.name} 눌렀다.");
            }
        }
    }

    /// <summary>
    /// 마우스 왼쪽 버튼이 이 셀 위에서 떨어졌을 때 실행될 함수.
    /// </summary>
    public void CellRelease()
    {
        if (GameManager.Inst.IsPlaying)
        {
            if (pressedCells.Count != 1)                // 1개가 아닐 때(2개 이상일 때는 다 처리. 0개일 때는 중복 실행이지만 무시되니까 그냥 처리)
            {
                int flagCount = 0;
                foreach (var cell in neighbors)          // 주변에 있는 깃발 갯수 세기
                {
                    if (cell.IsFlaged)
                        flagCount++;
                }

                if (flagCount == aroundMineCount)       // 주변의 깃발 갯수와 주변 지뢰의 갯수가 같을 때만 눌려진 것들 다 열기
                {
                    foreach (var cell in pressedCells)  // 눌려져 있던 셀들을 전부 순회하면서 열기
                    {
                        cell.Open();
                    }
                }
                else
                {
                    RestoreCovers();                    // 갯수가 다르면 눌려져있던 셀들 복구
                }
            }
            else
            {
                // 1개 일때는 자기 자신만 열고 끝내기
                pressedCells[0].Open();
            }
            pressedCells.Clear();               // 연 셀들을 눌린 셀 목록에서 제거
        }
    }

    /// <summary>
    /// 이 셀이 눌러졌을 때 처리해야 할 일을 모아 놓은 함수
    /// </summary>
    void PressCover()
    {
        switch (markState)      // 표시 상태에 따라 이미지 변경
        {
            case CellMarkState.None:
                cover.sprite = Board[CloseCellType.Close_Press];
                break;
            case CellMarkState.Question:
                cover.sprite = Board[CloseCellType.Question_Press];
                break;
            case CellMarkState.Flag:
            default:
                break;
        }
        pressedCells.Add(this); // 눌러진 셀이라고 표시
    }

    /// <summary>
    /// 이 셀과 관련해서 눌려져 있던 셀들이 복구 될 때 해야할 일을 모아 놓은 함수
    /// </summary>
    void RestoreCovers()
    {
        foreach (var cell in pressedCells)  // 전부 순회하면서 복구
        {
            cell.RestoreCover();
        }
        pressedCells.Clear();               // 리스트 비우기
    }

    /// <summary>
    /// 이 셀 하나가 눌려져 있다가 복구 될 때 해야할 일을 모아 놓은 함수
    /// </summary>
    void RestoreCover()
    {
        switch (markState)  // 이미지 상태에 따라서 복구
        {
            case CellMarkState.None:
                cover.sprite = Board[CloseCellType.Close];
                break;
            case CellMarkState.Question:
                cover.sprite = Board[CloseCellType.Question];
                break;
            case CellMarkState.Flag:
            default:
                break;
        }

        Debug.Log($"{this.gameObject.name} 복구.");
    }

    /// <summary>
    /// 주변 8칸에 지뢰가 추가될 때 실행되는 함수
    /// </summary>
    public void IncreaseAroundMineCount()
    {
        if (!hasMine)
        {
            aroundMineCount++;
            inside.sprite = Board[(OpenCellType)aroundMineCount];   // 주변 지뢰 숫자에 맞게 이미지 설정
        }
    }

    /// <summary>
    /// 이 셀에 지뢰를 추가하는 함수
    /// </summary>
    public void SetMine()
    {
        hasMine = true;     // 지뢰 설치 되었다고 표시
        inside.sprite = Board[OpenCellType.Mine_NotFound];  // 지뢰로 이미지 변경

        // 이 셀 주변 셀들의 IncreaseAroundMineCount함수 실행(aroundMineCount를 1씩 증가)
        List<Cell> cellList = Board.GetNeighbors(ID);       // 실행 타이밍이 Start보다 빨라 따로 구해줌
        foreach( var cell in cellList)
        {
            cell.IncreaseAroundMineCount();
        }
    }

    public void ResetCell()
    {
        isOpen = false;
        cover.gameObject.SetActive(true);
        hasMine = false;
        markState = CellMarkState.None;
        aroundMineCount = 0;
        cover.sprite = Board[CloseCellType.Close];
        inside.sprite = Board[OpenCellType.Empty];
        pressedCells.Clear();
    }

    /// <summary>
    /// 셀을 오른쪽 클릭했을 때 실행될 함수
    /// </summary>
    public void CellRightPress()
    {
        if (GameManager.Inst.IsPlaying && !IsOpen)
        {
            switch (markState)
            {
                case CellMarkState.None:
                    // markState가 none이면 flag가 된다.      -> 깃발 갯수가 줄어든다. 셀 이미지 변경된다.
                    markState = CellMarkState.Flag;
                    cover.sprite = Board[CloseCellType.Flag];
                    onFlagUse?.Invoke();
                    break;
                case CellMarkState.Flag:
                    // markState가 flag이면 question이 된다.  -> 깃발 갯수가 늘어난다. 셀 이미지 변경된다.
                    markState = CellMarkState.Question;
                    cover.sprite = Board[CloseCellType.Question];
                    onFlagReturn?.Invoke();
                    break;
                case CellMarkState.Question:
                    // markState가 question이면 none이 된다.  -> 셀 이미지 변경된다.
                    markState = CellMarkState.None;
                    cover.sprite = Board[CloseCellType.Close];
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 셀 위에 마우스가 올라왔을 때 실행되는 함수
    /// </summary>
    public void OnEnterCell()
    {
        //Debug.Log("들어왔음");
        if (Mouse.current.leftButton.ReadValue() > 0)   // 마우스 왼쪽 버튼이 눌러져 있으면
        {
            Debug.Log($"마우스 왼쪽버튼을 누른체로 들어왔음\n({this.gameObject.name})");
            CellPress();
        }
    }

    /// <summary>
    /// 셀 위에 마우스가 있다가 밖으로 나갔을 때 실행되는 함수
    /// </summary>
    public void OnExitCell()
    {
        //Debug.Log("나갔음");
        if (Mouse.current.leftButton.ReadValue() > 0)   // 마우스 왼쪽 버튼이 눌러져 있으면
        {
            Debug.Log($"마우스 왼쪽버튼을 누른체로 나갔음\n({this.gameObject.name})");
            RestoreCovers();
        }
    }
}
