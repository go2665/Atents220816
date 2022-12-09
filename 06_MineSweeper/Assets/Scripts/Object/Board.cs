using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    // 변수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 생성할 셀의 프리팹
    /// </summary>
    public GameObject cellPrefab;

    /// <summary>
    /// 인풋 액션
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 보드가 가지는 가로 셀의 길이. (가로 줄의 셀 갯수)
    /// </summary>
    private int width = 16;

    /// <summary>
    /// 보드가 가지는 세로 셀의 길이. (세로 줄의 셀 갯수)
    /// </summary>
    private int height = 16;

    /// <summary>
    /// 보드에 설치될 지뢰의 갯수
    /// </summary>
    private int mineCount = 10;

    /// <summary>
    /// 셀 한 변의 길이(셀은 정사각형)
    /// </summary>
    const float Distance = 1.0f;    // 1일 때 카메라 크기 9.

    /// <summary>
    /// 이 보드가 가지는 모든 셀
    /// </summary>
    Cell[] cells;

    /// <summary>
    /// 열린 셀에서 표시될 이미지
    /// </summary>
    public Sprite[] openCellImages;

    /// <summary>
    /// 안열린 셀에서 표시될 이미지
    /// </summary>
    public Sprite[] closeCellImages;

    /// <summary>
    /// 현재 마우스가 올라가 있는 셀
    /// </summary>
    Cell currentCell = null;

    /// <summary>
    /// 이 보드에서 열려있는 셀의 숫자
    /// </summary>
    private int openCellCount = 0;

    /// <summary>
    /// 이 보드에서 찾은 지뢰의 숫자
    /// </summary>
    private int foundMineCount = 0;

    // 델리게이트 ----------------------------------------------------------------------------------
    public Action onBoardPress;
    public Action onBoardRelease;

    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// OpenCellType으로 이미지를 받아오는 인덱서
    /// </summary>
    /// <param name="type">필요한 이미지의 enum타입</param>
    /// <returns>enum타입에 맞는 이미지</returns>
    public Sprite this[OpenCellType type] => openCellImages[(int)type];

    /// <summary>
    /// CloseCellType 이미지를 받아오는 인덱서
    /// </summary>
    /// <param name="type">필요한 이미지의 enum타입</param>
    /// <returns>enum타입에 맞는 이미지</returns>
    public Sprite this[CloseCellType type] => closeCellImages[(int)type];

    /// <summary>
    /// 현재 마우스가 올라가 있는 셀을 확인하는 프로퍼티
    /// </summary>
    Cell CurrentCell
    {
        get => currentCell;
        set
        {            
            currentCell?.OnExitCell();  // 셀에서 마우스 나가는 처리
            currentCell = value;            
            currentCell?.OnEnterCell(); // 셀에 마우스가 들어가는 처리
        }
    }

    /// <summary>
    /// 열린 셀의 갯수를 확인만 가능한 프로퍼티
    /// </summary>
    public int OpenCellCount => openCellCount;

    /// <summary>
    /// 찾은 지뢰의 갯수를 확인만 가능한 프로퍼티
    /// </summary>
    public int FoundMineCount => foundMineCount;

    // 함수 ---------------------------------------------------------------------------------------

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.RightClick.performed += OnRightClick;
        inputActions.Player.LeftClick.performed += OnLeftPress;
        inputActions.Player.LeftClick.canceled += OnLeftRelease;
        inputActions.Player.MouseMove.performed += OnMouseMove;
    }

    private void OnDisable()
    {
        inputActions.Player.MouseMove.performed -= OnMouseMove;
        inputActions.Player.LeftClick.canceled -= OnLeftRelease;
        inputActions.Player.LeftClick.performed -= OnLeftPress;
        inputActions.Player.RightClick.performed -= OnRightClick;
        inputActions.Player.Disable();
    }    

    /// <summary>
    /// 이 보드가 가질 모든 셀을 생성하고 배치하는 함수
    /// </summary>
    public void Initialize(int newWidth, int newHeight, int newMineCount)
    {
        // 기존에 존재하던 셀 다 지우기
        ClearCells();   

        width = newWidth;
        height = newHeight;
        mineCount = newMineCount;

        Vector3 basePos = transform.position;       // 기준 위치 설정(보드의 위치)

        // 보드의 피봇을 중심으로 셀이 생성되게 하기 위해 셀이 생성될 시작점 계산용도로 구하기
        Vector3 offset = new(-(width-1) * Distance * 0.5f, (height-1) * Distance * 0.5f);   

        // 셀들의 배열 생성
        cells = new Cell[width * height];

        // 셀들을 하나씩 생성하기 위한 이중 for
        GameManager gameManager = GameManager.Inst;
        for (int y = 0; y<height; y++)
        {
            for(int x = 0;x<width; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, transform);    // 이 보드를 부모로 삼고 생성
                Cell cell = cellObj.GetComponent<Cell>();                   // 생성한 오브젝트에서 Cell 컴포넌트 찾기
                cell.ID = y * width + x;                                    // ID 설정(ID를 통해 위치도 확인 가능)
                cell.Board = this;                                          // 보드 설정
                cell.onFlagUse += gameManager.DecreaseFlagCount;
                cell.onFlagReturn += gameManager.IncreaseFlagCount;
                cell.onOpen += () => openCellCount++;
                cell.onMineFound += () => foundMineCount++;
                cell.onMineFoundCancel += () => foundMineCount--;
                cell.onAction += gameManager.FinishPlayerAction;
                cellObj.name = $"Cell_{cell.ID}_({x},{y})";                 // 오브젝트 이름 지정
                cell.transform.position = basePos + offset + new Vector3(x * Distance, -y * Distance);  // 적절한 위치에 배치
                cells[cell.ID] = cell;                                      // cells 배열에 저장
            }
        }
        gameManager.onGameReset += ResetBoard;
        gameManager.onGameOver += OnGameOver;

        ResetBoard();        
    }

    /// <summary>
    /// 보드를 초기 상태로 되돌리고 랜덤으로 지뢰설치
    /// </summary>
    public void ResetBoard()
    {
        // 모든 셀 초기화
        foreach (var cell in cells)
        {
            cell.ResetCell();
        }

        // 만들어진 셀에 지뢰를 mineCount만큼 설치하기
        int[] ids = new int[cells.Length];
        for (int i = 0; i < cells.Length; i++)
        {
            ids[i] = i;
        }
        Shuffle(ids);
        for (int i = 0; i < mineCount; i++)
        {
            cells[ids[i]].SetMine();
        }

        openCellCount = 0;  // 모든 셀이 다 닫혀있음.
        foundMineCount = 0; // 찾은 지뢰 갯수 초기화
    }

    /// <summary>
    /// 파라메터로 받은 배열 내부의 데이터 순서를 섞는 함수
    /// </summary>
    /// <param name="source">내부 데이터를 섞을 배열</param>
    public void Shuffle(int[] source)
    {
        int count = source.Length - 1;
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, count + 1 - i);
            int lastIndex = count - i;
            (source[randomIndex], source[lastIndex]) = (source[lastIndex], source[randomIndex]);    // swap 처리
        }
    }

    /// <summary>
    /// 파라메터로 받은 ID를 가진 셀 주변의 셀들을 리턴하는 함수
    /// </summary>
    /// <param name="id">찾을 중심 셀</param>
    /// <returns>id주변에 있는 셀들</returns>
    public List<Cell> GetNeighbors(int id)
    {
        // ID가 id인 셀 주변의 모든 셀을 result에 담기
        List<Cell> result = new List<Cell>(8);
        Vector2Int grid = IdToGrid(id);

        for(int i=-1;i<2;i++)
        {
            for(int j=-1;j<2;j++)
            {
                int index = GridToID(j + grid.x, i + grid.y);
                if( index != Cell.ID_NOT_VALID && !(i == 0 && j == 0) )
                {
                    result.Add(cells[index]);
                }
            }
        }        

        return result;
    }

    /// <summary>
    /// 셀의 ID를 Grid좌표로 변경하는 함수. Vaild한 그리드 좌표가 나온다는 보장은 없음.
    /// </summary>
    /// <param name="id">Grid로 변경할 ID</param>
    /// <returns>변경 완료된 Grid좌표</returns>
    Vector2Int IdToGrid(int id)
    {
        return new Vector2Int(id % width, id / width);
    }

    /// <summary>
    /// Grid 좌표를 ID로 변경하는 함수
    /// </summary>
    /// <param name="x">변경할 그리드 좌표의 X값</param>
    /// <param name="y">변경할 그리드 좌표의 Y값</param>
    /// <returns>변경에 성공하면 정상적인 ID. 실패할 경우 ID_NOT_VALID(-1)</returns>
    int GridToID(int x, int y)
    {
        if(IsValidGrid(x,y))
            return x + y * width;   // 그리드 좌표가 적합하면 변환

        return Cell.ID_NOT_VALID;   // 적합하지 않으면 ID_NOT_VALID
    }

    /// <summary>
    /// 입력 받은 스크린 좌표가 몇번째 그리드에 있는지 알려주는 함수
    /// </summary>
    /// <param name="screenPos">확인할 스크린좌표</param>
    /// <returns>스크린좌표와 매칭되는 보드 위의 그리드 좌표</returns>
    Vector2Int ScreenToGrid(Vector2 screenPos)
    {
        // 스크린 좌표를 월드 좌표로 변경하기
        Vector2 worldPos = (Vector2)Camera.main.ScreenToWorldPoint(screenPos);

        // 보드의 왼쪽 위(시작 좌표) 구하기
        Vector2 startPos = new Vector2(-width * Distance * 0.5f, height * Distance * 0.5f) + (Vector2)transform.position;        

        // 보드의 왼쪽 위에서 마우스가 얼마만큼 떨어져 있는지 확인
        Vector2 diff = worldPos - startPos;

        // Distance로 나누어서 Grid좌표로 변환
        return new((int)(diff.x / Distance), (int)(-diff.y / Distance));
    }

    /// <summary>
    /// 입력받은 스크린 좌표를 ID로 변경하는 함수
    /// </summary>
    /// <param name="screenPos">확인할 스크린좌표</param>
    /// <returns>스크린좌표와 매칭되는 보드위의 셀 ID</returns>
    int ScreenToID(Vector2 screenPos)
    {
        Vector2Int grid = ScreenToGrid(screenPos);
        return GridToID(grid.x, grid.y);
    }

    /// <summary>
    /// 보드안에 있는 그리즈 좌표인지 확인하는 함수
    /// </summary>
    /// <param name="x">확인할 그리드 좌표의 X</param>
    /// <param name="y">확인할 그리드 좌표의 Y</param>
    /// <returns>보드 안에 있는 그리드 좌표이면 true. 아니면 false</returns>
    bool IsValidGrid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    /// <summary>
    /// 보드안에 있는 그리즈 좌표인지 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 그리드 좌표</param>
    /// <returns>보드 안에 있는 그리드 좌표이면 true. 아니면 false</returns>
    bool IsValidGrid(Vector2Int grid)
    {
        return IsValidGrid(grid.x, grid.y);
    }

    /// <summary>
    /// 보드의 모든 셀을 제거하는 함수.
    /// </summary>
    public void ClearCells()
    {
        if (cells != null)  // 기존에 만들어진 셀이 있으면
        {
            // 이미 만들어진 셀 오브젝트를 모두 삭제하기
            foreach (var cell in cells)
            {
                Destroy(cell.gameObject);
            }
            cells = null;   // 안의 내용을 다 제거했다고 표시
        }
    }

    /// <summary>
    /// 마우스 왼쪽 버튼을 눌렀을 때 실행될 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnLeftPress(InputAction.CallbackContext _)
    {
        Debug.Log("왼쪽 눌렀다.");
        Vector2 screenPos = Mouse.current.position.ReadValue();     // 마우스 커서의 스크린 좌표를 읽기
        Vector2Int grid = ScreenToGrid(screenPos);                  // 스크린 좌표를 Grid좌표로 변환
        if (IsValidGrid(grid))                                      // 결과 그리드 좌표가 적합한지 확인 => 적합하지 않으면 보드 밖이라는 의미
        {
            GameManager.Inst.GameStart();                           // 매번 Play 상태로 변경 시도(Ready 상태일 때만 진행됨)
            Cell target = cells[GridToID(grid.x, grid.y)];          // 해당 셀 가져오기
            target.CellPress();

            onBoardPress?.Invoke();
        }
    }

    /// <summary>
    /// 마우스 왼쪽 버튼을 땠을 때 실행될 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnLeftRelease(InputAction.CallbackContext _)
    {
        Debug.Log("왼쪽 땠다.");
        Vector2 screenPos = Mouse.current.position.ReadValue();     // 마우스 커서의 스크린 좌표를 읽기
        Vector2Int grid = ScreenToGrid(screenPos);                  // 스크린 좌표를 Grid좌표로 변환
        if (IsValidGrid(grid))                                      // 결과 그리드 좌표가 적합한지 확인 => 적합하지 않으면 보드 밖이라는 의미
        {
            Cell target = cells[GridToID(grid.x, grid.y)];          // 해당 셀 가져오기
            target.CellRelease();
        }
        onBoardRelease?.Invoke();
    }

    /// <summary>
    /// 마우스 오른쪽 버튼을 클릭했을 때 실행될 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnRightClick(InputAction.CallbackContext _)
    {
        Debug.Log("오른쪽 클릭");
        Vector2 screenPos = Mouse.current.position.ReadValue();     // 마우스 커서의 스크린 좌표를 읽기
        Vector2Int grid = ScreenToGrid(screenPos);                  // 스크린 좌표를 Grid좌표로 변환
        if( IsValidGrid(grid) )                                     // 결과 그리드 좌표가 적합한지 확인 => 적합하지 않으면 보드 밖이라는 의미
        {
            Cell target = cells[GridToID(grid.x, grid.y)];          // 해당 셀 가져오기
            Debug.Log($"{target.gameObject.name}을 우클릭했습니다.");
            target.CellRightPress();
        }
        else
        {
            Debug.Log("셀 없음");
        }
    }

    /// <summary>
    /// 마우스가 움직일 때 실행되는 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector2Int grid = ScreenToGrid(screenPos);                  // 스크린 좌표를 Grid좌표로 변환
        if (IsValidGrid(grid))                                      // 결과 그리드 좌표가 적합한지 확인 => 적합하지 않으면 보드 밖이라는 의미
        {
            CurrentCell = cells[GridToID(grid.x, grid.y)];          // 해당 셀을 CurrentCell에 기록
        }
        else
        {            
            CurrentCell = null; // CurrentCell 비우기
        }
    }

    /// <summary>
    /// 게임 오버가 되었을 때 Board가 처리해야 할 일을 수행하는 함수
    /// </summary>
    private void OnGameOver()
    {
        foreach (var cell in cells)
        {
            if(cell.IsFlaged && !cell.HasMine)
            {
                // 깃발을 잘못 설치했다. == 깃발이 표시되어있는데 지뢰가 없다.
                cell.SetFlagIncorrect();
            }
            if(!cell.IsFlaged && cell.HasMine)
            {
                // 지뢰를 못찾았다. == 지뢰는 있는데 깃발이 표시되어 있지 않다.
                cell.SetMineNotFound();
            }
        }
    }
}
