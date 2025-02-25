﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IDataPersistence
{
    [Header("Menu objects")]
    public GameObject UIMenu;

    [Header("UI Buttons")]
    public bool UIActive = false;
    public Button openUIButton;
    public TMP_Text openUIText;
    public bool questActive = false;
    public Button openQuestButton;
    public TMP_Text openQuestText;

    public Button openSettingsButton;
    public Button openMarketButton;
    public Button openInventoryButton;
    public Button openCraftButton;
    public Button nextDayButton;
    public Button closeButton;

    [Header("Game variables")]
    public float tax;
    public float balance;
    public int water;
    public int fertilizer;

    [Header("Game variables text")]
    public TMP_Text taxAmountText;
    public TMP_Text balanceAmountText;
    public TMP_Text waterAmountText;
    public TMP_Text cardAmountText;
    public TMP_Text dateAmountText;
    public TMP_Text fertilizerAmountText;

    [Header("UI Island builder")]
    public TMP_Text buildCostText;
    public TMP_Text taxCostText;
    public Slider transparencySlider;
    public Image constructionLabel;

    public void SetUIButtons(bool active, Button button)
    {
        button.interactable = active;
    }

    public void OpenUIMenu()
    {
        if (!UIActive)
        {
            openUIText.SetText("<");
            openUIButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-470, openUIButton.GetComponent<RectTransform>().anchoredPosition.y);
            UIMenu.SetActive(true);
            UIActive = true;
        }
        else
        {
            openUIText.SetText(">");
            openUIButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-930, openUIButton.GetComponent<RectTransform>().anchoredPosition.y);
            UIMenu.SetActive(false);
            UIActive = false;
        }
        if (openUIButton.GetComponent<Image>().color == Color.green)
        {
            openUIButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void OpenQuestMenu()
    {
        if (!questActive)
        {
            openQuestText.SetText("ʌ");
            openQuestButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(openQuestButton.GetComponent<RectTransform>().anchoredPosition.x, 140);
            GameManager.TTM.questMenu.SetActive(true);
            questActive = true;
        }
        else
        {
            openQuestText.SetText("v");
            openQuestButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(openQuestButton.GetComponent<RectTransform>().anchoredPosition.x, 510);
            GameManager.TTM.questMenu.SetActive(false);
            questActive = false;
        }
        if (openQuestButton.GetComponent<Image>().color == Color.green)
        {
            openQuestButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void UpdateUI()
    {
        dateAmountText.SetText(GameManager.TM.UpdateDate());
        taxAmountText.SetText(tax.ToString() + " ₴");
        balanceAmountText.SetText(balance.ToString() + " ₴");
        waterAmountText.SetText(water.ToString() + " L");
        fertilizerAmountText.SetText(fertilizer.ToString() + " L");
        cardAmountText.SetText(GameManager.DM.cardsInDeck.Count.ToString() + " x");
    }

    public void SetBuildIslandSlider()
    {
        buildCostText.SetText(GameManager.IPM.clickedIsland.islandBuildCost.ToString());
        taxCostText.SetText(GameManager.IPM.clickedIsland.islandTaxCost.ToString());
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
                island.sign.SetActive(true);
                island.ToggleState(Island.IslandState.Default, Island.IslandState.Highlighted);
                balance -= island.islandBuildCost;
                tax += island.islandTaxCost;
                GameManager.ISM.AddIslandToBought(island);
                island.islandBought = true;
                constructionLabel.gameObject.SetActive(false);
                GameManager.UM.UpdateUI();
            }
        }
    }

    public void LoadData(GameData data)
    {
        tax = data.tax;
        balance = data.balance;
        water = data.water;
        fertilizer = data.fertilizer;
    }

    public void SaveData(ref GameData data)
    {
        data.tax = tax;
        data.balance = balance;
        data.water = water;
        data.fertilizer = fertilizer;
    }
}
