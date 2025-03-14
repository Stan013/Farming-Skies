using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketButton : MonoBehaviour
{
    public bool isSelling;
    public TMP_InputField inputAmount;
    public Button transactionButton;
    public Button plusButton;
    public Button minusButton;
    public Button minButton;
    public Button maxButton;
    public MarketItem marketItem;
    private int minAmount = 0;
    private int maxAmount;

    private void Awake()
    {
        if (inputAmount != null)
        {
            inputAmount.contentType = TMP_InputField.ContentType.IntegerNumber;
            inputAmount.onValueChanged.AddListener((input) => ValidateInput(input));
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
        inputAmount.onValueChanged.AddListener((input) => ValidateInput(input));
    }

    private void ValidateInput(string input)
    {
        int currentValue;
        if (int.TryParse(input, out currentValue))
        {
            currentValue = Mathf.Clamp(currentValue, minAmount, maxAmount);
            inputAmount.text = currentValue.ToString();
        }
        else
        {
            inputAmount.text = minAmount.ToString();
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
            maxAmount = Mathf.FloorToInt(GameManager.UM.money / marketItem.priceCurrent);
        }
    }

    public void IncreaseInput()
    {
        UpdateMaxAmount();
        int currentValue;
        if (int.TryParse(inputAmount.text, out currentValue))
        {
            currentValue = Mathf.Min(currentValue + 1, maxAmount);
            inputAmount.text = currentValue.ToString();
        }
        else
        {
            inputAmount.text = "0";
        }
    }

    public void DecreaseInput()
    {
        UpdateMaxAmount();
        int currentValue;
        if (int.TryParse(inputAmount.text, out currentValue))
        {
            currentValue = Mathf.Max(currentValue - 1, minAmount);
            inputAmount.text = currentValue.ToString();
        }
        else
        {
            inputAmount.text = "0";
        }
    }

    public void SetToMinAmount()
    {
        UpdateMaxAmount();
        inputAmount.text = minAmount.ToString();
    }

    public void SetToMaxAmount()
    {
        UpdateMaxAmount();
        inputAmount.text = maxAmount.ToString();
    }

    private void OnTransactionButtonClicked()
    {
        int amount;
        if (int.TryParse(inputAmount.text, out amount))
        {
            if (amount <= 0)
            {
                return;
            }
            MarketManager marketManager = GameManager.MM;
            marketManager.ExecuteTransaction(marketItem, amount, isSelling);
        }
        if (GameManager.TTM.tutorial && GameManager.TTM.tutorialCount == 19)
        {
            minButton.enabled = true;
            minusButton.enabled = true;
            plusButton.enabled = true;
            maxButton.enabled = true;
            inputAmount.enabled = true;
            transactionButton.GetComponent<Image>().color = new Color(0.84f, 0.84f, 0.84f);
        }
    }
}
