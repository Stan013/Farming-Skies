using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpenseManager : MonoBehaviour
{
    [Header("General variables")]
    public string statsTab;
    public Button closeButton;

    [Header("Farm Level variables")]
    public Slider farmLevelBar;
    public TMP_Text farmLevelText;

    [Header("Farm Stat variables")]
    public float farmValue;
    public float farmValueChange;
    public TMP_Text farmValueText;
    public TMP_Text farmValueChangeText;
    public float plantValue;
    public float plantValueChange;
    public TMP_Text plantValueText;
    public TMP_Text plantValueChangeText;
    public float productionValue;
    public float productionValueChange;
    public TMP_Text productionValueText;
    public TMP_Text productionValueChangeText;

    [Header("Expense tab variables")]
    public ExpenseItem expenseItemTemplate;
    public List<ExpenseItem> expenseIslands;
    public GameObject expenseIslandsContentArea;
    public float expenseIslandsTotal;
    public TMP_Text expenseIslandsTotalText;
    public List<ExpenseItem> expenseBuildables;
    public GameObject expenseBuildablesContentArea;
    public float expenseBuildablesTotal;
    public TMP_Text expenseBuildablesTotalText;
    public List<ExpenseItem> expenseProduction;
    public GameObject expenseProductionContentArea;
    public float expenseProductionTotal;
    public TMP_Text expenseProductionTotalText;

    public void AddExpenseIsland(Island island)
    {
        ExpenseItem expenseIsland = Instantiate(expenseItemTemplate, Vector3.zero, Quaternion.identity, expenseIslandsContentArea.transform);
        expenseIsland.SetupIslandExpense(island);
        expenseIsland.transform.localPosition = new Vector3(expenseIsland.transform.localPosition.x, expenseIsland.transform.localPosition.y, 0);
        expenseIsland.transform.localRotation = Quaternion.identity;
        expenseIslands.Add(expenseIsland);
    }

    public void OpenIslandManagement(string tab)
    {
        statsTab = tab;
        int currentValue = GameManager.ISM.boughtIslands.Count;
        farmLevelBar.value = 1 / GameManager.LM.farmLevelMax * currentValue;
        farmLevelText.text = "Level " + GameManager.LM.FarmLevel.ToString();
        SetFarmStatistics();
        switch (tab)
        {
            case "Statistics":

                break;
            case "Expenses":
                expenseIslandsTotalText.text = expenseIslandsTotal.ToString();
                expenseBuildablesTotalText.text = expenseBuildablesTotal.ToString();
                expenseProductionTotalText.text = expenseProductionTotal.ToString();
                break;
            case "Earnings":

                break;
        }
    }

    public void SetFarmStatistics()
    {
        farmValueText.text = farmValue.ToString();
        farmValueChangeText.text = farmValueChange.ToString();
        plantValueText.text = plantValue.ToString();
        plantValueChangeText.text = plantValueChange.ToString();
        productionValueText.text = productionValue.ToString();
        productionValueChangeText.text = productionValueChange.ToString();
    }
}
