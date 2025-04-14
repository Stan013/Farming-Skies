using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpenseManager : MonoBehaviour, IDataPersistence
{
    [Header("General variables")]
    public string statsTab;
    public Button closeButton;
    public TMP_Text expenseText;
    private float _expense;
    public float Expense
    {
        get => _expense;
        set
        {
            _expense = value;
            expenseText.text = GameManager.UM.FormatNumber(_expense).ToString();
        }
    }

    [Header("Farm Level variables")]
    public Slider farmLevelBar;
    public TMP_Text farmLevelText;

    [Header("Farm Stat variables")]
    public float farmValue;
    public float oldFarmValue;
    public float farmValueChange;
    public TMP_Text farmValueText;
    public TMP_Text farmValueChangeText;
    public TMP_Text plantValueText;
    public TMP_Text plantValueChangeText;
    public TMP_Text buildableValueText;
    public TMP_Text buildableValueChangeText;

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

    public void AddExpenseBuildables(Plant plant)
    {
        ExpenseItem expenseBuildable = Instantiate(expenseItemTemplate, Vector3.zero, Quaternion.identity, expenseBuildablesContentArea.transform);
        expenseBuildable.SetupBuildableExpense(plant);
        expenseBuildable.transform.localPosition = new Vector3(expenseBuildable.transform.localPosition.x, expenseBuildable.transform.localPosition.y, 0);
        expenseBuildable.transform.localRotation = Quaternion.identity;
        expenseBuildables.Add(expenseBuildable);
    }

    public void OpenIslandManagement(string tab)
    {
        statsTab = tab;
        int currentValue = GameManager.ISM.boughtIslands.Count;
        farmLevelBar.value = 1 / GameManager.LM.farmLevelMax * GameManager.LM.FarmLevel;
        farmLevelText.text = "Level " + GameManager.LM.FarmLevel.ToString();
        SetFarmStatistics();
        switch (tab)
        {
            case "Statistics":
                break;
            case "Expenses":
                expenseIslandsTotalText.text = expenseIslandsTotal.ToString() + " ₴";
                expenseBuildablesTotalText.text = expenseBuildablesTotal.ToString() + " ₴";
                expenseProductionTotalText.text = expenseProductionTotal.ToString() + " ₴";
                break;
            case "Earnings":

                break;
        }
    }

    public void UpdateFarmValue()
    {
        farmValue = GameManager.ISM.IslandValue + GameManager.PM.PlantValue + GameManager.PM.BuildablesValue;
    }

    public void SetFarmStatistics()
    {
        farmValueText.text = farmValue.ToString() + " ₴";
        farmValueChangeText.text = "(+" + farmValueChange.ToString() + " ₴)";
        plantValueText.text = GameManager.PM.PlantValue.ToString() + " ₴";
        plantValueChangeText.text = "(+" + GameManager.PM.plantValueChange.ToString() + " ₴)";
        buildableValueText.text = GameManager.PM.BuildablesValue.ToString() + " ₴";
        buildableValueChangeText.text = "(+" + GameManager.PM.buildableValueChange.ToString() + " ₴)";
    }

    public void LoadData(GameData data)
    {
        GameManager.DPM.ClearChildren(expenseIslandsContentArea.transform);
        expenseIslands.Clear();
        for (int i = 0; i < data.islandExpenses.Count; i++)
        {
            AddExpenseIsland(GameManager.ISM.FindIslandByID(data.islandExpenses[i]));
        }
    }

    public void SaveData(ref GameData data)
    {
        data.islandExpenses.Clear();
        foreach (ExpenseItem expenseItem in expenseIslands)
        {
            data.islandExpenses.Add(expenseItem.attachedIsland.islandID);
        }
    }
}
