using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    /// <summary>
    /// 이 패널에 붙어있는 모든 탭
    /// </summary>
    Tab[] tabs;

    /// <summary>
    /// 이 패널에서 선택 된 탭
    /// </summary>
    Tab selectedTab;

    /// <summary>
    /// 이 패널에 붙어있는 토글 버튼
    /// </summary>
    ToggleButton toggle;

    private void Awake()
    {
        // 모든 자식 탭 찾기
        tabs = GetComponentsInChildren<Tab>();  
        
        foreach(var tab in tabs)
        {
            // 탭이 선택되었을 때 실행될 델리게이트에 람다 함수 등록
            tab.onTabSelected += (newSelectedTab) =>
            {
                if (newSelectedTab != selectedTab)  // 서로 다른 탭일 때만 변경
                {
                    selectedTab.IsSelected = false; // 이전 탭 끄기
                    selectedTab = newSelectedTab;
                    selectedTab.IsSelected = true;  // 새 탭 열기                    
                }
                toggle.SetToggleState(true);        // 탭이 눌러지면 무조건 토글버튼 켜기
            };
        }

        // 자식 토글 버튼 찾기
        toggle = GetComponentInChildren<ToggleButton>();
        // 토글 버튼이 on/off될 때 실행될 델리게이트에 람다 함수 등록
        toggle.onToggleChange += (isOn) =>
        {
            if(isOn && selectedTab != null)         // 토글 버튼이 켜지고 선택된 탭이 있을 때                                                    
            {
                // 선택된 탭 체크 이유 : 토글 버튼의 start가  RankPanel의 Start보다 먼저 실행되었을 때 문제 발생
                selectedTab.ChildPanelOpen();       // 선택된 탭을 연다
            }
            else
            {
                foreach(var tab in tabs)            // 토글 버튼이 꺼지면 모든 탭을 닫는다.
                {
                    tab.ChildPanelClose();
                }
            }
        };
    }

    private void Start()
    {
        // 게임 메니저에서 알려주는 타이밍에 따라 열고 닫기 설정
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameClear += Open;
        gameManager.onGameOver += Open;
        gameManager.onGameReset += Close;

        // 무조건 하나의 탭은 선택된 탭이 되도록 처리
        selectedTab = tabs[0];
        selectedTab.IsSelected = true;

        // 기본적으로는 닫혀있다.
        Close();
    }

    /// <summary>
    /// 랭크 패널 열기
    /// </summary>
    void Open()
    {
        this.gameObject.SetActive(true);
    }

    // 랭크 패널 닫기
    void Close()
    {
        this.gameObject.SetActive(false);
    }
}
