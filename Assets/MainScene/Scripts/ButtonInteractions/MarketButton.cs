using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketButton : MonoBehaviour
{
    public bool isSelling;
    public TMP_InputField inputAmountField;
    public Button transactionButton;
    public Button plusButton;
    public Button minusButton;
    public Button minButton;
    public Button maxButton;
    public MarketItem marketItem;
    public int inputAmount;
    public int maxAmount;

    private void Awake()
    {
        if (inputAmountField != null)
        {
            inputAmountField.contentType = TMP_InputField.ContentType.IntegerNumber;
            inputAmountField.onValueChanged.AddListener((input) => ValidateInput(input));
        }
    }

    private void Start()
    {
        UpdateMaxAmount();
        transactionButton.onClick.AddListener(OnTransactionButtonClicked);
        plusButton.onClick.AddListener(IncreaseInput);
        minusButton.onClick.AddListener(DecreaseInput);
        minButton.onClick.AddListener(SetToMinAmount);
        maxButton.onClick.AddListener(SetToMaxAmount);
        inputAmountField.onValueChanged.AddListener((input) => ValidateInput(input));
    }

    private void ValidateInput(string input)
    {
        int currentValue;
        if (int.TryParse(input, out currentValue))
        {
            currentValue = Mathf.Clamp(currentValue, 0, maxAmount);
            inputAmount = currentValue;
            inputAmountField.text = currentValue.ToString();
        }
        else
        {
            inputAmount = 0;
            inputAmountField.text = "0";
        }
    }

    private void UpdateMaxAmount()
    {
        if (isSelling && marketItem.attachedItemCard != null)
        {
            maxAmount = marketItem.attachedItemCard.itemQuantity;
        }
        else
        {
            maxAmount = Mathf.FloorToInt(GameManager.UM.balance / marketItem.priceCurrent);
        }
    }

    public void IncreaseInput()
    {
        UpdateMaxAmount();
        int currentValue;
        if (int.TryParse(inputAmountField.text, out currentValue))
        {
            currentValue = Mathf.Min(currentValue + 1, maxAmount);
            inputAmount = currentValue;
            inputAmountField.text = currentValue.ToString();
        }
    }

    public void DecreaseInput()
    {
        UpdateMaxAmount();
        int currentValue;
        if (int.TryParse(inputAmountField.text, out currentValue))
        {
            currentValue = Mathf.Max(currentValue - 1, 0);
            inputAmount = currentValue;
            inputAmountField.text = currentValue.ToString();
        }
    }

    public void SetToMinAmount()
    {
        UpdateMaxAmount();
        inputAmount = 0;
        inputAmountField.text = "0";
    }

    public void SetToMaxAmount()
    {
        UpdateMaxAmount();
        inputAmount = maxAmount;
        inputAmountField.text = maxAmount.ToString();
    }

    private void OnTransactionButtonClicked()
    {
        int amount;
        if (int.TryParse(inputAmountField.text, out amount))
        {
            if (amount <= 0)
            {
                return;
            }
            MarketManager marketManager = GameManager.MM;
            marketManager.ExecuteTransaction(marketItem, amount, isSelling);
        }
    }
}
