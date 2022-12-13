using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    bool isSelected = false;
    Button tabButton;
    Image tabImage;
    Transform childPanel;

    readonly Color UnSelectedColor = new Color(1, 1, 1, 0.2f);

    public Action<Tab> onTabSelected;

    public bool IsSelected
    {
        get => isSelected;
        set
        {            
            isSelected = value;
            TabSelected(isSelected);
        }
    }

    private void Awake()
    {
        tabButton = GetComponent<Button>();
        tabButton.onClick.AddListener(() =>
        {
            if(!IsSelected)
            {
                IsSelected = true;
            }
        });
        tabImage = GetComponent<Image>();

        childPanel = transform.GetChild(1);
        IsSelected = false;
    }

    void TabSelected(bool selected)
    {
        if( selected )
        {
            // 선택 되었을 때 처리
            tabImage.color = Color.white;
            onTabSelected?.Invoke(this);
            ChildPanelOpen();
        }
        else
        {
            // 선택되지 않았을 때 처리
            tabImage.color = UnSelectedColor;
            ChildPanelClose();
        }
    }

    public void ChildPanelOpen()
    {
        if( IsSelected )
            childPanel.gameObject.SetActive(true);
    }

    public void ChildPanelClose()
    {
        childPanel.gameObject.SetActive(false);
    }
}
