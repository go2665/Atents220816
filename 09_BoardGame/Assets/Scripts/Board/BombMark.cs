using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMark : MonoBehaviour
{
    /// <summary>
    /// 공격이 성공했을 때 보일 프리팹
    /// </summary>
    public GameObject successMark;

    /// <summary>
    /// 공격이 실패했을 때 보일 프리팹
    /// </summary>
    public GameObject failureMark;

    /// <summary>
    /// 테스트 용도로 공격한 지점 표시하는 프리팹
    /// </summary>
    public GameObject testInfoPrefab;

    /// <summary>
    /// testInfoPrefab을 보여줄지 결정하는 변수. true면 보여준다.
    /// </summary>
    public bool isShowTestInfo = true;

    /// <summary>
    /// 공격받은 위치에 포탄 명중 여부를 표시해주는 함수
    /// </summary>
    /// <param name="position">공격받은 위치(그리드를 다시 월드로 변경한 것)</param>
    /// <param name="isSuccess">배에 명중되었으면 true, 아니면 false로 입력</param>
    public void SetBombMark(Vector3 position, bool isSuccess)
    {
        GameObject markPrefab = isSuccess ? successMark : failureMark;      // isSuccess가 true면 "O", false면 "X" 선택
        GameObject markInstance = Instantiate(markPrefab, transform);       // 생성해서 위치 설정
        markInstance.transform.position = position + Vector3.up * 2.0f;

        if(isShowTestInfo)  // 테스트용 오브젝트 보여 줄거면
        {
            GameObject testObj = Instantiate(testInfoPrefab, markInstance.transform);    // 생성해서 위치 설정
            testObj.transform.position = position + Vector3.up * 0.5f;
        }
    }
}
