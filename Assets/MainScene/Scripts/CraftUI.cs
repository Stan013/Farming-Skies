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
    public Button craftCardButton;

    void Start()
    {
        craftCardButton.onClick.AddListener(() => CraftCard());
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
        if(GameManager.CRM.cardCraftAmount == 1 && plusButton.GetComponent<Image>().color == Color.green)
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

    public void CraftCard()
    {
        if (GameManager.CRM.selectedCard == null || GameManager.CRM.cardCraftAmount <= 0) return;

        float totalCoinCost = GameManager.CRM.selectedCard.cardCraftResources[0] * GameManager.CRM.cardCraftAmount;
        int totalWaterCost = GameManager.CRM.selectedCard.cardCraftResources[1] * GameManager.CRM.cardCraftAmount;
        int totalFertilizerCost = GameManager.CRM.selectedCard.cardCraftResources[2] * GameManager.CRM.cardCraftAmount;

        if (GameManager.UM.balance >= totalCoinCost &&
            GameManager.UM.water >= totalWaterCost &&
            GameManager.UM.fertilizer >= totalFertilizerCost)
        {
            GameManager.UM.balance -= totalCoinCost;
            GameManager.UM.water -= totalWaterCost;
            GameManager.UM.fertilizer -= totalFertilizerCost;

            for (int i = 0; i < GameManager.CRM.cardCraftAmount; i++)
            {
                GameManager.DM.AddCardToDeck(GameManager.CRM.selectedCard.cardId);
            }
            GameManager.CRM.CalculateMaxCraftableAmount();
            SetCraftAmount(0);
        }
        else
        {
            Debug.Log("Not enough resources to craft the card.");
        }
    }

    public void CheckSelectedCard(Card card)
    {
        if (card.cardName == "Nitrogen Fertilizer" || card.cardName == "Phosphorus Fertilizer" || card.cardName == "Potassium Fertilizer")
        {
            card.GetComponent<Image>().color = Color.green;
        }
    }
}
