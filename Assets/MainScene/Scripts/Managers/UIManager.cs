using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IDataPersistence
{
    [Header("UI menus")]
    public GameObject levelUI;
    public GameObject resourceUI;
    public GameObject timeUI;
    public GameObject selectionUI;

    [Header("Game variables")]
    public int farmLevel;
    public float expense;
    public float balance;
    public int water;
    public int fertiliser;
    public int weeks;

    [Header("Game variables text")]
    public TMP_Text farmLevelText;
    public TMP_Text expenseText;
    public TMP_Text balanceText;
    public TMP_Text waterText;
    public TMP_Text fertiliserText;
    public TMP_Text deckText;
    public TMP_Text weekText;

    [Header("Island builder")]
    public TMP_Text buildCostText;
    public TMP_Text expenseCostText;
    public Slider transparencySlider;
    public Image constructionLabel;

    public void UpdateUI()
    {
        farmLevelText.SetText("Level " + FormatNumber(farmLevel).ToString());
        expenseText.SetText(FormatNumber(expense).ToString() + " ₴");
        balanceText.SetText(FormatNumber(balance).ToString() + " ₴");
        waterText.SetText(FormatNumber(water).ToString() + " L");
        fertiliserText.SetText(FormatNumber(fertiliser).ToString() + " L");
        deckText.SetText(FormatNumber(GameManager.DM.cardsInDeck.Count).ToString() + " x");
    }

    public static string FormatNumber(float num)
    {
        if (num >= 1000000000)
            return (num / 1000000000f).ToString("0.#") + "B";
        if (num >= 1000000)
            return (num / 1000000f).ToString("0.#") + "M";
        if (num >= 1000)
            return (num / 1000f).ToString("0.#") + "K";

        return num.ToString("0");
    }

    public void SetBuildIslandSlider()
    {
        buildCostText.SetText(GameManager.IPM.potentialIsland.islandBuildCost.ToString() + " ₴");
        expenseCostText.SetText(GameManager.IPM.potentialIsland.islandExpenseCost.ToString() + " ₴");
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
                balance -= island.islandBuildCost;
                GameManager.ISM.AddIslandToBought(island);
                constructionLabel.gameObject.SetActive(false);
                GameManager.UM.UpdateUI();
            }
        }
    }

    public void LoadData(GameData data)
    {
        expense = data.expense;
        balance = data.balance;
        water = data.water;
        fertiliser = data.fertiliser;
    }

    public void SaveData(ref GameData data)
    {
        data.expense = expense;
        data.balance = balance;
        data.water = water;
        data.fertiliser = fertiliser;
    }
}
