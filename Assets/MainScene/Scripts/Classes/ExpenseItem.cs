using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class ExpenseItem : MonoBehaviour
{
    public ExpenseUI expenseUI;
    public TMP_Text islandName;
    public TMP_Text islandCost;
    public TMP_Text buildableName;
    public TMP_Text buildableCost;

    public void SetupIslandExpense(Island island)
    {
        GameManager.UM.AddExpense(island.islandTaxCost);
        expenseUI.islandTotal += island.islandTaxCost;
        islandName.text = "Island (" + island.name + "):";
        islandCost.text = island.islandTaxCost.ToString() + " ₴";
    }

    public void SetupBuildableExpense(Plant buildable)
    {
        GameManager.UM.AddExpense(buildable.buildableTaxCost);
        expenseUI.buildableTotal += buildable.buildableTaxCost;
        buildableName.text = buildable.name.Replace("(Clone)(Clone)", "").Trim();
        buildableCost.text = buildable.buildableTaxCost.ToString() + " ₴";
    }
}
