using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;

    public int width = 16;
    public int height = 16;

    Cell[] cells;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        GameObject cellObj = Instantiate(cellPrefab, transform);
        Cell cell = cellObj.GetComponent<Cell>();

        // cell의 ID는 0부터 1씩 증가한다.(생성되는 순서대로 설정된다)
        // cell의 위치를 자동으로 배치 (왼쪽위가 (0,0), 왼쪽->오른쪽 : +x, 위->아래 : +y)
        // Board의 pivot을 중심으로 모든 셀들이 배치되어야 한다.
        // 생성된 cell은 cells에 모두 저장된다.

    }

}
