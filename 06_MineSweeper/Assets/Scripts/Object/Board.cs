using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 생성할 셀의 프리팹
    /// </summary>
    public GameObject cellPrefab;

    /// <summary>
    /// 보드가 가지는 가로 셀의 길이. (가로 줄의 셀 갯수)
    /// </summary>
    private int width = 16;

    /// <summary>
    /// 보드가 가지는 세로 셀의 길이. (세로 줄의 셀 갯수)
    /// </summary>
    private int height = 16;

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
    /// OpenCellType으로 이미지를 받아오는 인덱서
    /// </summary>
    /// <param name="type">필요한 이미지의 enum타입</param>
    /// <returns>enum타입에 맞는 이미지</returns>
    public Sprite this[OpenCellType type] => openCellImages[(int)type];

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.RightClick.performed += OnRightClick;
        inputActions.Player.LeftClick.performed += OnLeftClick;
    }

    private void OnDisable()
    {
        inputActions.Player.LeftClick.performed -= OnLeftClick;
        inputActions.Player.RightClick.performed -= OnRightClick;
        inputActions.Player.Disable();
    }    

    /// <summary>
    /// 이 보드가 가질 모든 셀을 생성하고 배치하는 함수
    /// </summary>
    public void Initialize(int newWidth, int newHeight, int mineCount)
    {
        // 기존에 존재하던 셀 다 지우기
        ClearCells();   

        width = newWidth;
        height = newHeight;

        Vector3 basePos = transform.position;       // 기준 위치 설정(보드의 위치)

        // 보드의 피봇을 중심으로 셀이 생성되게 하기 위해 셀이 생성될 시작점 계산용도로 구하기
        Vector3 offset = new(-(width-1) * Distance * 0.5f, (height-1) * Distance * 0.5f);   

        // 셀들의 배열 생성
        cells = new Cell[width * height];

        // 셀들을 하나씩 생성하기 위한 이중 for
        for (int y = 0; y<height; y++)
        {
            for(int x = 0;x<width; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, transform);    // 이 보드를 부모로 삼고 생성
                Cell cell = cellObj.GetComponent<Cell>();                   // 생성한 오브젝트에서 Cell 컴포넌트 찾기
                cell.ID = y * width + x;                                    // ID 설정(ID를 통해 위치도 확인 가능)
                cell.Board = this;                                          // 보드 설정
                cellObj.name = $"Cell_{cell.ID}_({x},{y})";                 // 오브젝트 이름 지정
                cell.transform.position = basePos + offset + new Vector3(x * Distance, -y * Distance);  // 적절한 위치에 배치
                cells[cell.ID] = cell;                                      // cells 배열에 저장
            }
        }

        // 만들어진 셀에 지뢰를 mineCount만큼 설치하기
        int[] ids = new int[cells.Length];
        for(int i=0;i<cells.Length;i++)
        {
            ids[i] = i;
        }
        Shuffle(ids);
        for(int i=0;i<mineCount;i++)
        {
            cells[ids[i]].SetMine();
        }
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
            int randomIndex = Random.Range(0, count + 1 - i);
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

    Vector2Int IdToGrid(int id)
    {
        return new Vector2Int(id % width, id / width);
    }

    int GridToID(int x, int y)
    {
        if( x >= 0 && x < width && y >= 0 && y < height)
            return x + y * width;

        return Cell.ID_NOT_VALID;
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

    private void OnLeftClick(InputAction.CallbackContext _)
    {
        Debug.Log("왼쪽 클릭");
    }

    private void OnRightClick(InputAction.CallbackContext _)
    {
        Debug.Log("오른쪽 클릭");

    }
}
