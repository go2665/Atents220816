public enum OpenCellType
{
    Empty = 0,      // 주변에 지뢰가 없다.
    Number1,        // 주변에 지뢰가 1개이다.
    Number2,
    Number3,
    Number4,
    Number5,
    Number6,
    Number7,
    Number8,
    Mine_NotFound,  // 못찾은 지뢰
    Mine_Explosion, // 밟은 지뢰
    Mine_Mistake    // 지뢰가 아닌데 지뢰라고 표시한 경우
}

