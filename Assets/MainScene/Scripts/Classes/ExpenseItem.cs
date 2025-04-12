using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpenseItem : MonoBehaviour
{
    public Image expenseItemIcon;
    public TMP_Text expenseItemCostText;

    public Sprite islandIcon;
    public Sprite waterBarrelIcon;
    public Sprite compostBinIcon;

    public void SetupIslandExpense(Island island)
    {
        GameManager.EM.expenseIslandsTotal += island.islandExpenseCost;
        GameManager.UM.Expense += island.islandExpenseCost;
        expenseItemIcon.sprite = islandIcon;
        expenseItemCostText.text = "+ " + island.islandExpenseCost.ToString() + " ₴";
    }

    public void SetupBuildableExpense(Plant buildable)
    {
        GameManager.EM.expenseBuildablesTotal += buildable.buildableTaxCost;
        GameManager.UM.Expense += buildable.buildableTaxCost;
        if (buildable.name.Contains("Water Barrel"))
        {
            expenseItemIcon.sprite = waterBarrelIcon;
        }
        else
        {
            expenseItemIcon.sprite = compostBinIcon;
        }
        expenseItemCostText.text = "+ " + buildable.buildableTaxCost.ToString() + " ₴";
    }
}
