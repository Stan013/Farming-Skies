using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject UIMenu;
    [SerializeField] private Button openUIButton;
    [SerializeField] private Button openQuestButton;
    [SerializeField] private Button openSettingsButton;
    [SerializeField] private Button openMarketButton;
    [SerializeField] private Button openInventoryButton;
    [SerializeField] private TMP_Text openUIText;
    [SerializeField] private TMP_Text openQuestText;
    [SerializeField] private TMP_Text taxAmountText;
    [SerializeField] private TMP_Text balanceAmountText;
    [SerializeField] private TMP_Text waterAmountText;
    [SerializeField] private TMP_Text cardAmountText;
    [SerializeField] private TMP_Text islandBuildCostText;
    [SerializeField] private TMP_Text dateAmountText;

    //UI On-screen
    public float tax;
    public float balance;
    public float water;

    //UI Off-screen
    private bool UIActive = false;
    private bool questActive = false;
    private float rentPercentage;

    //UI Island Build
    [SerializeField] private Slider transparencySlider;
    public Image constructionLabel;

    public void SetUIButtons(bool active, string buttonName)
    {
        switch (buttonName)
        {
            case "UI":
                openUIButton.interactable = active;
                break;
            case "Quest":
                openQuestButton.interactable = active;
                break;
            case "Settings":
                openSettingsButton.interactable = active;
                break;
            case "Market":
                openMarketButton.interactable = active;
                break;
            case "Inventory":
                openInventoryButton.interactable = active;
                break;
        }
    }

    public void OpenUIMenu()
    {
        if (!UIActive)
        {
            openUIText.SetText("<");
            openUIButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, openUIButton.GetComponent<RectTransform>().anchoredPosition.y);
            UIMenu.SetActive(true);
            UIActive = true;
        }
        else
        {
            openUIText.SetText(">");
            openUIButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-925, openUIButton.GetComponent<RectTransform>().anchoredPosition.y);
            UIMenu.SetActive(false);
            UIActive = false;
        }
    }

    public void OpenQuestMenu()
    {
        if (!questActive)
        {
            openQuestText.SetText("ʌ");
            openQuestButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(openQuestButton.GetComponent<RectTransform>().anchoredPosition.x, 88);
            GameManager.TTM.questMenu.SetActive(true);
            questActive = true;
        }
        else
        {
            openQuestText.SetText("v");
            openQuestButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(openQuestButton.GetComponent<RectTransform>().anchoredPosition.x, 505);
            GameManager.TTM.questMenu.SetActive(false);
            questActive = false;
        }
    }

    public void UpdateUI()
    {
        dateAmountText.SetText(GameManager.TM.UpdateDate());
        taxAmountText.SetText(tax.ToString() + " ₴");
        balanceAmountText.SetText(balance.ToString() + " ₴");
        waterAmountText.SetText(water.ToString() + " L");
        cardAmountText.SetText(GameManager.DM.cardsInDeck.Count.ToString() + " x");
    }

    public void SetBuildIslandSlider()
    {
        islandBuildCostText.SetText(GameManager.IPM.clickedIsland.islandBuildCost.ToString());
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
            if (alphaValue == 1f && island.islandStatus == "Unbought")
            {
                balance -= island.islandBuildCost;
                tax += (island.islandBuildCost / 100 * rentPercentage);
                GameManager.ISM.AddIslandToBought(island);
                island.islandStatus = "Bought";
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
    }

    public void SaveData(ref GameData data)
    {
        data.tax = tax;
        data.balance = balance;
        data.water = water;
    }
}
