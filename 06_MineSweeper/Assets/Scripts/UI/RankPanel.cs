using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    Tab[] tabs;
    Tab selectedTab;
    ToggleButton toggle;

    private void Awake()
    {
        tabs = GetComponentsInChildren<Tab>();  
        
        foreach(var tab in tabs)
        {
            tab.onTabSelected += (newSelectedTab) =>
            {
                if (newSelectedTab != selectedTab)   // 서로 다른 탭일 때만 변경
                {
                    selectedTab.IsSelected = false;     // 이전 탭 끄기
                    selectedTab = newSelectedTab;
                    selectedTab.IsSelected = true;      // 새 탭 열기
                }
            };
        }

        toggle = GetComponentInChildren<ToggleButton>();
        toggle.onToggleChange += (isOn) =>
        {
            if(isOn && selectedTab != null)     // 토글 버튼이 켜지고 선택된 탭이 있을 때
            {
                selectedTab.ChildPanelOpen();   // 선택된 탭을 연다
            }
            else
            {
                foreach(var tab in tabs)        // 토글 버튼이 꺼지면 모든 탭을 닫는다.
                {
                    tab.ChildPanelClose();
                }
            }
        };

    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameClear += Open;
        gameManager.onGameOver += Open;
        gameManager.onGameReset += Close;

        selectedTab = tabs[0];
        selectedTab.IsSelected = true;

        Close();
    }

    void Open()
    {
        this.gameObject.SetActive(true);
    }

    void Close()
    {
        this.gameObject.SetActive(false);
    }
}
