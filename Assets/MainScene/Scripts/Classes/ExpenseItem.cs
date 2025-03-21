using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class ExpenseItem : MonoBehaviour
{
    public ExpenseCategory islandExpenses;
    public ExpenseCategory waterBarrelExpenses;
    public ExpenseCategory compostExpenses;

    public TMP_Text expenseItemNameText;
    public Image expenseItemImage;
    public TMP_Text expenseItemCostText;

    public Sprite islandImage;
    public Sprite waterBarrelImage;
    public Sprite compostImage;

    public void SetupIslandExpense(Island island)
    {
        islandExpenses.IslandsTotal += island.islandExpenseCost;
        expenseItemNameText.text = "(" + island.name + ")";
        expenseItemImage.sprite = islandImage;
        expenseItemCostText.text = "+ " + island.islandExpenseCost.ToString() + " ₴";
    }
}
