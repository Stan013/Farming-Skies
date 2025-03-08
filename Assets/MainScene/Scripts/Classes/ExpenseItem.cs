using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpenseItem : MonoBehaviour
{
    public UIStatButton UIstats;
    public TMP_Text islandName;
    public TMP_Text islandCost;
    public TMP_Text buildableName;
    public TMP_Text buildableCost;
    public int expenseIndex;

    public void SetupIslandExpense(Island island)
    {
        expenseIndex++;
        UIstats.islandTotal += island.islandTaxCost;
        islandName.text = expenseIndex.ToString() + ".         Island (" + island.name + "):";
        islandCost.text = island.islandTaxCost + " ₴";
    }

/*    public void SetupBuildableExpense(Buildable buildable)
    {
        buildableName.text = ;
        buildableCost.text =  + " ₴";
    }*/
}
