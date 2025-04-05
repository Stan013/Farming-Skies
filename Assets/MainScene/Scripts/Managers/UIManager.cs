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
    private float _balance;
    private int _water;
    private int _fertiliser;
    private int _deck;
    private float _expense;
    private int _weeks;

    public float Balance
    {
        get => _balance;
        set
        {
            _balance = value;
            UpdateUI();
        }
    }
    public int Water
    {
        get => _water;
        set
        {
            _water = value;
            UpdateUI();
        }
    }
    public int Fertiliser
    {
        get => _fertiliser;
        set
        {
            _fertiliser = value;
            UpdateUI();
        }
    }
    public int Deck
    {
        get => _deck;
        set
        {
            _deck = value;
            UpdateUI();
        }
    }
    public float Expense
    {
        get => _expense;
        set
        {
            _expense = value;
            UpdateUI();
        }
    }
    public int Weeks
    {
        get => _weeks;
        set
        {
            _weeks = value;
            UpdateUI();
        }
    }

    [Header("Game variables text")]
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
        expenseText.text = FormatNumber(_expense).ToString();
        balanceText.text = FormatNumber(_balance).ToString();
        waterText.text = FormatNumber(_water).ToString();
        fertiliserText.text = FormatNumber(_fertiliser).ToString();
        deckText.text = FormatNumber(_deck).ToString();
        weekText.text = FormatNumber(_weeks).ToString();
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
        buildCostText.SetText(GameManager.IPM.potentialIsland.islandBuildCost.ToString() + "₴");
        expenseCostText.SetText(GameManager.IPM.potentialIsland.islandExpenseCost.ToString() + "₴");
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
                _balance -= island.islandBuildCost;
                GameManager.ISM.AddIslandToBought(island);
                constructionLabel.gameObject.SetActive(false);
                ;
            }
        }
    }

    public void LoadData(GameData data)
    {
        _expense = data.expense;
        _balance = data.balance;
        _water = data.water;
        _fertiliser = data.fertiliser;
    }

    public void SaveData(ref GameData data)
    {
        data.expense = _expense;
        data.balance = _balance;
        data.water = _water;
        data.fertiliser = _fertiliser;
    }
}
