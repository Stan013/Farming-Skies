using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketButton : MonoBehaviour
{
    [SerializeField] private bool isSelling;
    [SerializeField] private TMP_InputField m_InputAmount;
    public TMP_InputField inputAmount { get { return m_InputAmount; } }
    [SerializeField] private Button transactionButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button minButton;
    [SerializeField] private Button maxButton;
    [SerializeField] private MarketItem marketItem;
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
            maxAmount = Mathf.FloorToInt(GameManager.UM.balance / marketItem.priceCurrent);
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
    }
}
