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
    public Structure attachedStructure;

    public Sprite islandIcon;

    public void SetupIslandExpense(Island island)
    {
        attachedIsland = island;
        GameManager.EM.expenseIslandsTotal += attachedIsland.islandExpenseCost;
        expenseItemIcon.sprite = islandIcon;
        expenseItemCostText.text = "+ " + attachedIsland.islandExpenseCost.ToString() + " ₴";
        GameManager.TAM.CalculateTaxes();
    }

    public void SetupStructureExpense(Structure structure)
    {
        attachedStructure = structure;
        GameManager.EM.expenseStructuresTotal += attachedStructure.structureTax;
        GameManager.EM.Expense += attachedStructure.structureTax;
        expenseItemIcon.sprite = attachedStructure.structureIcon;
        expenseItemCostText.text = "+ " + structure.structureTax.ToString() + " ₴";
    }
}
