using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Island;

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

    [Header("Game variables text")]
    public TMP_Text balanceText;
    public TMP_Text waterText;
    public TMP_Text fertiliserText;

    [Header("Island builder")]
    public TMP_Text buildCostText;
    public TMP_Text expenseCostText;
    public Slider transparencySlider;
    public Image constructionLabel;

    public void UpdateUI()
    {
        balanceText.text = FormatNumber(_balance).ToString();
        waterText.text = FormatNumber(_water).ToString();
        fertiliserText.text = FormatNumber(_fertiliser).ToString();
    }

    public string FormatNumber(float num)
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
                Balance -= island.islandBuildCost;
                GameManager.ISM.AddIslandToBought(island);
                constructionLabel.gameObject.SetActive(false);
                island.islandMatPotential = true;
                island.SetIslandMaterial();
                island.CreateIslandMaterial(IslandState.Cultivated);
            }
        }
    }

    public void FarmEvaluation()
    {
        GameManager.ISM.islandValueChange = GameManager.ISM.IslandValue - GameManager.ISM.oldIslandValue;
        GameManager.PM.plantValueChange = GameManager.PM.PlantValue - GameManager.PM.oldPlantValue;
        GameManager.PM.buildableValueChange = GameManager.PM.BuildablesValue - GameManager.PM.oldBuildableValue;
        GameManager.ISM.oldIslandValue = GameManager.ISM.IslandValue;
        GameManager.PM.oldPlantValue = GameManager.PM.PlantValue;
        GameManager.PM.oldBuildableValue = GameManager.PM.BuildablesValue;
    }

    public void LoadData(GameData data)
    {

        _balance = data.balance;
        _water = data.water;
        _fertiliser = data.fertiliser;
        UpdateUI();
    }

    public void SaveData(ref GameData data)
    {
        data.balance = _balance;
        data.water = _water;
        data.fertiliser = _fertiliser;
    }
}
