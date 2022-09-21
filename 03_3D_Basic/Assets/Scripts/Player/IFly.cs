using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IFly
{
    /// <summary>
    /// 이 인터페이스를 상속받은 클래스를 날려버리는 함수
    /// </summary>
    /// <param name="flyVector">날아갈 방향과 힘의 크기</param>
    void Fly(Vector3 flyVector);
}
