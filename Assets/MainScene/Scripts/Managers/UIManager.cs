﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IDataPersistence
{
    [Header("Menu objects")]
    public GameObject UIMenu;
    public GameObject infoMenu;

    [Header("Mode objects")]
    public Image modeIndicator;
    public Sprite[] modeIcons;

    [Header("UI Buttons")]
    public OpenUIButton UIbutton;
    public OpenQuestButton questButton;
    public GameObject nextWeekButton;

    [Header("Next Week UI")]
    public Slider nextWeekSlider;

    [Header("Window Buttons")]
    public OpenWindowButton openButton;
    public CloseWindowButton closeButton;

    [Header("Game variables")]
    public float expense;
    public float money;
    public int water;
    public int fertiliser;

    [Header("Game variables text")]
    public TMP_Text expenseAmountText;
    public TMP_Text moneyAmountText;
    public TMP_Text waterAmountText;
    public TMP_Text cardAmountText;
    public TMP_Text dateAmountText;
    public TMP_Text fertiliserAmountText;

    [Header("UI Island builder")]
    public TMP_Text buildCostText;
    public TMP_Text expenseCostText;
    public Slider transparencySlider;
    public Image constructionLabel;

    public void SetUIButtons(bool active, Button button)
    {
        button.interactable = active;
    }

    public void UpdateUI()
    {
        dateAmountText.SetText(GameManager.TM.GetDate());
        expenseAmountText.SetText(expense.ToString() + " ₴");
        moneyAmountText.SetText(money.ToString() + " ₴");
        waterAmountText.SetText(water.ToString() + " L");
        fertiliserAmountText.SetText(fertiliser.ToString() + " L");
        cardAmountText.SetText(GameManager.DM.cardsInDeck.Count.ToString() + " x");
    }

    public void SetBuildIslandSlider()
    {
        buildCostText.SetText(GameManager.IPM.clickedIsland.islandBuildCost.ToString() + " ₴");
        expenseCostText.SetText(GameManager.IPM.clickedIsland.islandTaxCost.ToString() + " ₴");
        if (constructionLabel != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(constructionLabel.canvas.transform as RectTransform, mousePosition, constructionLabel.canvas.worldCamera, out anchoredPosition);
            constructionLabel.rectTransform.anchoredPosition = anchoredPosition;
        }
    }

    public void UpdateBuildIslandSlider(Island island)
    {
        if (island.topMat != null && island.bottomMat != null)
        {
            float alphaValue = Mathf.Clamp01(island.topMat.color.a);
            transparencySlider.value = alphaValue;
            if (alphaValue == 1f && island.islandBought == false)
            {
                money -= island.islandBuildCost;
                expense += island.islandTaxCost;
                GameManager.ISM.AddIslandToBought(island);
                constructionLabel.gameObject.SetActive(false);
                GameManager.UM.UpdateUI();
            }
        }
    }

    public void LoadData(GameData data)
    {
        expense = data.tax;
        money = data.balance;
        water = data.water;
        fertiliser = data.fertiliser;
    }

    public void SaveData(ref GameData data)
    {
        data.tax = expense;
        data.balance = money;
        data.water = water;
        data.fertiliser = fertiliser;
    }
}
