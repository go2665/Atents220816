using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용 가능한 오브젝트가 가지는 인터페이스
/// </summary>
interface IUseableObject
{
    /// <summary>
    /// 오브젝트가 사용될 때 실행될 함수
    /// </summary>
    void Use();
}
