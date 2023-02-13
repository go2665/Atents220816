using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeploymentPanel : MonoBehaviour
{
    /// <summary>
    /// 이 패널 아래에 있는 모든 토글 버튼
    /// </summary>
    DeploymentToggle[] toggles = null;

    private void Awake()
    {
        toggles = GetComponentsInChildren<DeploymentToggle>();
        foreach (DeploymentToggle toggle in toggles)
        {
            toggle.onTogglePress += OnTogglePress;  // 모든 토글 버튼의 델리게이트에 함수 등록
        }
    }

    /// <summary>
    /// 토글 버튼이 눌러진 상태가 되었을 때 실행되는 함수
    /// </summary>
    /// <param name="toggle">눌러진 토글 버튼</param>
    private void OnTogglePress(DeploymentToggle toggle)
    {
        foreach(var others in toggles)  
        {
            if( toggle != others )  // 자신을 제외한 나머지 토글 버튼들을
            {
                others.UnToggle();  // 눌러진 상태 해제
            }
        }
    }
}
