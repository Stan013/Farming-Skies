using System.Collections;
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

    [Header("Window Buttons")]
    public OpenWindowButton openButton;
    public CloseWindowButton closeButton;

    [Header("Game variables")]
    public float expense;
    public float money;
    public int water;
    public int fertilizer;

    [Header("Game variables text")]
    public TMP_Text expenseAmountText;
    public TMP_Text moneyAmountText;
    public TMP_Text waterAmountText;
    public TMP_Text cardAmountText;
    public TMP_Text dateAmountText;
    public TMP_Text fertilizerAmountText;

    [Header("UI Island builder")]
    public TMP_Text buildCostText;
    public TMP_Text expenseCostText;
    public Slider transparencySlider;
    public Image constructionLabel;

    public void Start()
    {
        openButton = GetComponent<OpenWindowButton>();
        closeButton = GetComponent<CloseWindowButton>();
    }

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
        fertilizerAmountText.SetText(fertilizer.ToString() + " L");
        cardAmountText.SetText(GameManager.DM.cardsInDeck.Count.ToString() + " x");
    }

    public void SetBuildIslandSlider()
    {
        buildCostText.SetText(GameManager.IPM.clickedIsland.islandBuildCost.ToString());
        expenseCostText.SetText(GameManager.IPM.clickedIsland.islandTaxCost.ToString());
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
                money -= island.islandBuildCost;
                expense += island.islandTaxCost;
                GameManager.ISM.AddIslandToBought(island);
                island.islandBought = true;
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
        fertilizer = data.fertilizer;
    }

    public void SaveData(ref GameData data)
    {
        data.tax = expense;
        data.balance = money;
        data.water = water;
        data.fertilizer = fertilizer;
    }
}
