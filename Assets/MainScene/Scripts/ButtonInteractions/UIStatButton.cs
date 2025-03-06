using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatButton : MonoBehaviour
{
    public GameObject infoMenu;
    public GameObject expenseIslandInfo;
    public TMP_Text expenseTotalText;
    public GameObject expenseBuildableInfo;
    public TMP_Text buildableTotalText;
    public GameObject dateInfo;
    public TMP_Text nextEventText;

    public void OpenInfoMenu(string action)
    {
        if (infoMenu.activeSelf)
        {
            CloseInfoMenu(action);
        }
        else
        {
            infoMenu.SetActive(true);
        }

        switch (action)
        {
            case "Date":
                expenseIslandInfo.SetActive(true);
                expenseBuildableInfo.SetActive(true);
                nextEventText.text = GameManager.TM.CheckDate();
                break;
            case "Expense":
                expenseIslandInfo.SetActive(true);
                expenseBuildableInfo.SetActive(true);
                break;
        }
    }

    public void CloseInfoMenu(string action)
    {
        infoMenu.SetActive(false);
        switch (action)
        {
            case "Expense":
                expenseIslandInfo.SetActive(false);
                expenseBuildableInfo.SetActive(false);
                break;
        }
    }
}
