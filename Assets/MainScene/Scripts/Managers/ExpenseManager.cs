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
    private int _expense;
    public int Expense
    {
        get => _expense;
        set
        {
            if (Mathf.Approximately(_expense, value)) return;

            if (expenseRoutine != null)
                StopCoroutine(expenseRoutine);

            expenseRoutine = StartCoroutine(GameManager.UM.AnimateInt(
                _expense, value,
                v => expenseText.text = GameManager.UM.FormatNumber(v, true) + " ₴"
            ));

            _expense = value;
        }
    }
    
    public int farmingPermit;
    public bool farmingUnlocked;
    public int buildingPermit;
    public bool buildingUnlocked;
    public int animalPermit;
    public bool animalUnlocked;
    public int productionPermit;
    public bool productionUnlocked;


    [Header("UI animation")]
    private Coroutine expenseRoutine;

    // [Header("Farm Stat variables")]
    // public TMP_Text islandValueText;
    // public TMP_Text islandValueChangeText;
    // public TMP_Text plantValueText;
    // public TMP_Text plantValueChangeText;
    // public TMP_Text structureValueText;
    // public TMP_Text structureValueChangeText;

    [Header("Expense tab variables")]
    public ExpenseItem expenseItemTemplate;
    public List<ExpenseItem> expenseIslands;
    public GameObject expenseIslandsContentArea;
    public float expenseIslandsTotal;
    public TMP_Text expenseIslandsTotalText;

    public List<ExpenseItem> expenseStructures;
    public GameObject expenseStructuresContentArea;
    public float expenseStructuresTotal;
    public TMP_Text expenseStructuresTotalText;

    public List<ExpenseItem> expenseAnimals;
    public GameObject expenseAnimalsContentArea;
    public float expenseAnimalsTotal;
    public TMP_Text expenseAnimalsTotalText;

    public List<ExpenseItem> expenseProduction;
    public GameObject expenseProductionContentArea;
    public float expenseProductionTotal;
    public TMP_Text expenseProductionTotalText;

    // public void UpdateChangeFarmValue()
    // {
    //     islandValueChangeText.text = "(+" + GameManager.ISM.islandValueChange.ToString() + " ₴)";
    //     plantValueChangeText.text = "(+" + GameManager.PM.plantValueChange.ToString() + " ₴)";
    //     structureValueChangeText.text = "(+" + GameManager.PM.structureValueChange.ToString() + " ₴)";
    // }

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
        ExpenseItem expenseStructure = Instantiate(expenseItemTemplate, Vector3.zero, Quaternion.identity, expenseStructuresContentArea.transform);
        expenseStructure.SetupBuildableExpense(plant);
        expenseStructure.transform.localPosition = new Vector3(expenseStructure.transform.localPosition.x, expenseStructure.transform.localPosition.y, 0);
        expenseStructure.transform.localRotation = Quaternion.identity;
        expenseStructures.Add(expenseStructure);
    }

    public void OpenExpensenseManagement(string tab)
    {
        statsTab = tab;
        int currentValue = GameManager.ISM.boughtIslands.Count;
        // farmLevelBar.maxValue = GameManager.LM.farmLevelMax;
        // farmLevelBar.value = GameManager.LM.FarmLevel;
        // farmLevelBarText.text = GameManager.LM.FarmLevel + " / " + GameManager.LM.farmLevelMax;
        // farmLevelText.text = "Level " + GameManager.LM.FarmLevel.ToString();
        switch (tab)
        {
            case "Statistics":
                break;
            case "Expenses":
                expenseIslandsTotalText.text = expenseIslandsTotal.ToString() + " ₴";
                expenseStructuresTotalText.text = expenseStructuresTotal.ToString() + " ₴";
                expenseProductionTotalText.text = expenseProductionTotal.ToString() + " ₴";
                break;
            case "Earnings":

                break;
        }
        //UpdateChangeFarmValue();
    }

    // public void SetFarmStats()
    // {
    //     GameManager.ISM.islandValue = GameManager.ISM.islandValueChange;
    //     GameManager.ISM.islandValueChange = 0;
    //     islandValueText.text = GameManager.ISM.islandValue.ToString() + " ₴";
    //     GameManager.PM.plantValue = GameManager.PM.plantValueChange;
    //     GameManager.PM.plantValueChange = 0;
    //     plantValueText.text = GameManager.PM.plantValue.ToString() + " ₴";
    //     GameManager.PM.structureValue = GameManager.PM.structureValueChange;
    //     GameManager.PM.structureValueChange = 0;
    //     structureValueText.text = GameManager.PM.structureValue.ToString() + " ₴";
    // }

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
