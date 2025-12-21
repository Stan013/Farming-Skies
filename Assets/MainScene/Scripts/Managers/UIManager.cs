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
    public GameObject islandBuilder;
    public TMP_Text buildCostText;
    public TMP_Text expenseCostText;
    public Slider buildSlider;
    public Image constructionLabel;
    public GameObject missingFundsLabel;

    public void SetupUI()
    {
        selectionUI.SetActive(true);
        GameManager.INM.closeButton.interactable = true;
        GameManager.MM.closeButton.interactable = true;
        GameManager.CRM.closeButton.interactable = true;
        GameManager.ISM.closeButton.interactable = true;
        GameManager.EM.closeButton.interactable = true;
        GameManager.EVM.closeButton.interactable = true;
        foreach (Transform child in selectionUI.transform)
        {
            child.GetComponent<Button>().interactable = true;
        }
    }

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
        buildCostText.SetText(GameManager.IPM.hoverIsland.islandBuildCost.ToString() + "₴");
        expenseCostText.SetText(GameManager.IPM.hoverIsland.islandExpenseCost.ToString() + "₴");
        if (constructionLabel != null)
        {
            constructionLabel.rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public void UpdateBuildIslandSlider(Island island)
    {
        if (island.topMat != null && island.bottomMat != null)
        {
            float alphaValue = Mathf.Clamp01(island.topMat.color.a);
            buildSlider.value = alphaValue;
            if (alphaValue == 1f && island.islandBought == false)
            {
                Balance -= island.islandBuildCost;
                GameManager.ISM.AddIslandToBought(island);
                constructionLabel.gameObject.SetActive(false);
            }
        }
    }

    public void FarmEvaluation()
    {
        GameManager.ISM.islandValueChange = GameManager.ISM.IslandValue - GameManager.ISM.oldIslandValue;
        GameManager.PM.plantValueChange = GameManager.PM.PlantValue - GameManager.PM.oldPlantValue;
        GameManager.PM.structureValueChange = GameManager.PM.StructureValue - GameManager.PM.oldStructureValue;
        GameManager.ISM.oldIslandValue = GameManager.ISM.IslandValue;
        GameManager.PM.oldPlantValue = GameManager.PM.PlantValue;
        GameManager.PM.oldStructureValue = GameManager.PM.StructureValue;
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
