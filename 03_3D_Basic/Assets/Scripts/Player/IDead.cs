using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 인터페이스를 가진 클래스는 죽을 수 있다.
/// </summary>
interface IDead
{
    /// <summary>
    /// 죽었을 때 실행할 델리게이트(죽었을 때 다른 클래스들이 할 일)
    /// </summary>
    Action onDie { get; set; } 

    /// <summary>
    /// 죽었을 때 실행할 기능(죽었을 때 이 인터페이스를 상속받은 클래스가 할 일)
    /// </summary>
    void Die(); 
}
