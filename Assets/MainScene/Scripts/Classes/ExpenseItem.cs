using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpenseItem : MonoBehaviour
{
    public ExpenseUI expenseUI;
    public TMP_Text islandName;
    public TMP_Text islandCost;
    public TMP_Text buildableName;
    public TMP_Text buildableCost;

    public void SetupIslandExpense(Island island)
    {
        expenseUI.islandTotal += island.islandTaxCost;
        islandName.text = "Island (" + island.name + "):";
        islandCost.text = island.islandTaxCost.ToString() + " ₴";
    }

/*    public void SetupBuildableExpense(Buildable buildable)
    {
        buildableName.text = ;
        buildableCost.text =  + " ₴";
    }*/
}
