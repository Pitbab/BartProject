using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenuController : MonoBehaviour
{
    private TabChanger CurrentTab;

    [SerializeField]private List<TabChanger> tabs = new List<TabChanger>();

    private void Start()
    {
        CurrentTab = tabs[0];
        CurrentTab.StartCoroutine(CurrentTab.LeftTranslation());
        CurrentTab.SetColor(Color.green);
    }

    public void ChangeTab(TabChanger Tab)
    {
        if (CurrentTab != Tab)
        {
            CurrentTab.StartCoroutine(CurrentTab.RightTranslation());
            CurrentTab.SetColor(Color.white);
        }

        CurrentTab = Tab;
        CurrentTab.SetColor(Color.green);
    }

    public TabChanger GetCurrentTab()
    {
        return CurrentTab;
    }
}
