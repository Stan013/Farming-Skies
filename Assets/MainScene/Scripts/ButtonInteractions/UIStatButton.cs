using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatButton : MonoBehaviour
{
    public GameObject infoMenu;
    public string previousAction;

    public GameObject expenseIslandInfo;
    public TMP_Text islandTotalText;
    public int islandTotal;
    public GameObject expenseBuildableInfo;
    public TMP_Text buildableTotalText;
    public int buildableTotal;

    public GameObject dateInfo;
    public TMP_Text nextEventText;

    public void OpenInfoMenu(string action)
    {
        if (infoMenu.activeSelf && previousAction == action)
        {
            CloseInfoMenu(action);
        }
        else
        {
            if(previousAction != action)
            {
                expenseIslandInfo.SetActive(false);
                expenseBuildableInfo.SetActive(false);
                dateInfo.SetActive(false);
            }
            infoMenu.SetActive(true);
        }

        switch (action)
        {
            case "Date":
                if (GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color != Color.white)
                {
                    GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color = Color.white;
                }
                dateInfo.SetActive(true);
                nextEventText.text = GameManager.TM.CheckDate();
                break;
            case "Expense":
                if (GameManager.UM.expenseAmountText.transform.parent.GetComponent<Image>().color != Color.white)
                {
                    GameManager.UM.expenseAmountText.transform.parent.GetComponent<Image>().color = Color.white;
                }
                expenseIslandInfo.SetActive(true);
                expenseBuildableInfo.SetActive(true);
                islandTotalText.text = islandTotal.ToString() + " ₴";
                buildableTotalText.text = buildableTotal.ToString() + " ₴";
                break;
        }
        previousAction = action;
    }

    public void CloseInfoMenu(string action)
    {
        infoMenu.SetActive(false);
        switch (action)
        {
            case "Date":
                dateInfo.SetActive(false);
                break;
            case "Expense":
                expenseIslandInfo.SetActive(false);
                expenseBuildableInfo.SetActive(false);
                break;
        }
    }
}
