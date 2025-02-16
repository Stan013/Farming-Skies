using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftUI : MonoBehaviour
{
    public Button minButton, minusButton, plusButton, maxButton;
    public TMP_InputField craftAmountInput;
    public TMP_Text balanceCostText, waterCostText, fertilizerCostText;
    public GameObject craftCardButton;

    void Start()
    {
        minButton.onClick.AddListener(() => SetCraftAmount(0));
        minusButton.onClick.AddListener(() => SetCraftAmount(GameManager.CRM.cardCraftAmount - 1));
        plusButton.onClick.AddListener(() => SetCraftAmount(GameManager.CRM.cardCraftAmount + 1));
        maxButton.onClick.AddListener(() => SetCraftAmount(GameManager.CRM.maxCraftableAmount));
        craftAmountInput.onValueChanged.AddListener(OnCraftAmountInputChanged);
        GameManager.CRM.CalculateMaxCraftableAmount();
        UpdateCostDisplay();
    }

    private void SetCraftAmount(int amount)
    {
        GameManager.CRM.cardCraftAmount = Mathf.Clamp(amount, 0, GameManager.CRM.maxCraftableAmount);
        craftAmountInput.text = GameManager.CRM.cardCraftAmount.ToString();
        if(GameManager.CRM.cardCraftAmount == 1 && GameManager.TTM.tutorialCount == 13 && plusButton.GetComponent<Image>().color == Color.green)
        {
            plusButton.GetComponent<Image>().color = Color.white;
            craftCardButton.GetComponent<Image>().color = Color.green;
        }
        UpdateCostDisplay();
    }

    private void OnCraftAmountInputChanged(string input)
    {
        if (int.TryParse(input, out int value))
        {
            SetCraftAmount(value);
        }
        else
        {
            craftAmountInput.text = GameManager.CRM.cardCraftAmount.ToString();
        }
    }

    private void UpdateCostDisplay()
    {
        if (GameManager.CRM.selectedCard == null)
            return;

        float coinCost = GameManager.CRM.selectedCard.cardCraftResources[0] * GameManager.CRM.cardCraftAmount;
        int waterCost = GameManager.CRM.selectedCard.cardCraftResources[1] * GameManager.CRM.cardCraftAmount;
        int fertilizerCost = GameManager.CRM.selectedCard.cardCraftResources[2] * GameManager.CRM.cardCraftAmount;
        balanceCostText.text = coinCost.ToString() + " ₴";
        waterCostText.text = waterCost.ToString() + " L";
        fertilizerCostText.text = fertilizerCost.ToString() + " L";
    }
}
