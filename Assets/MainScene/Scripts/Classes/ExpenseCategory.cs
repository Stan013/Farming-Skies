using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpenseCategory : MonoBehaviour
{
    public TMP_Text expenseItemText;

    public int _islandsTotal;
    public int IslandsTotal
    {
        get => _islandsTotal;
        set
        {
            if (_islandsTotal != value)
            {
                _islandsTotal = value;
                expenseItemText.SetText(_islandsTotal + " ₴");
            }
        }
    }

    public int _waterBarrelsTotal;
    public int WaterBarrelsTotal
    {
        get => _waterBarrelsTotal;
        set
        {
            if (_waterBarrelsTotal != value)
            {
                _waterBarrelsTotal = value;
                expenseItemText.SetText(_waterBarrelsTotal + " ₴");
            }
        }
    }

    public int _compostBinsTotal;
    public int CompostBinsTotal
    {
        get => _compostBinsTotal;
        set
        {
            if (_compostBinsTotal != value)
            {
                _compostBinsTotal = value;
                expenseItemText.SetText(_compostBinsTotal + " ₴");
            }
        }
    }
}
