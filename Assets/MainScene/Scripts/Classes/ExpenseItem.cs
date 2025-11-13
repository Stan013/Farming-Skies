using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpenseItem : MonoBehaviour
{
    public Image expenseItemIcon;
    public TMP_Text expenseItemCostText;
    public Island attachedIsland;
    public Plant attachedBuildable;

    public Sprite islandIcon;
    public Sprite waterBarrelIcon;
    public Sprite compostBinIcon;

    public void SetupIslandExpense(Island island)
    {
        attachedIsland = island;
        GameManager.EM.expenseIslandsTotal += attachedIsland.islandExpenseCost;
        GameManager.EM.Expense += attachedIsland.islandExpenseCost;
        expenseItemIcon.sprite = islandIcon;
        expenseItemCostText.text = "+ " + attachedIsland.islandExpenseCost.ToString() + " ₴";
    }

    public void SetupBuildableExpense(Plant buildable)
    {
        attachedBuildable = buildable;
        GameManager.EM.expenseStructuresTotal += attachedBuildable.structureTax;
        GameManager.EM.Expense += attachedBuildable.structureTax;
        if (buildable.name.Contains("Water Barrel"))
        {
            expenseItemIcon.sprite = waterBarrelIcon;
        }
        else
        {
            expenseItemIcon.sprite = compostBinIcon;
        }
        expenseItemCostText.text = "+ " + buildable.structureTax.ToString() + " ₴";
    }
}
