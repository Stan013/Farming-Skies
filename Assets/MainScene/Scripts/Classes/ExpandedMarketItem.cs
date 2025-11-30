using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpandedMarketItem : MonoBehaviour
{
    public MarketItem collapsedItem;
    public InventoryItem attachedInventoryItem;
    public Image expandedImage;
    public TMP_Text expandedName;
    public TMP_Text expandedQuantity;

    public Button transactionButton;
    public TMP_Text transactionText;
    public Image transactionButtonBackground;
    public Sprite sellTransaction;
    public Sprite buyTransaction;
    public string marketTransaction;

    public Image transactionInputBackground;
    public Sprite validTransaction;
    public Sprite ongoingTransaction;
    public Sprite invalidTransaction;
    public bool canTransaction;

    public Button minButton;
    public Button minusButton;
    public TMP_InputField transactionAmountInput;
    public Button plusButton;
    public Button maxButton;

    public Image balanceChangeBackground;
    public Sprite balanceAddition;
    public Sprite balanceDeduction;
    public TMP_Text balanceChangeText;

    public int TransactionAmount
    {
        get => _transactionAmount;
        set
        {
            int maxAmount = (marketTransaction == "Buy")
                ? collapsedItem.maxBuyAmount
                : attachedInventoryItem.ItemQuantity;

            _transactionAmount = Mathf.Clamp(value, 0, maxAmount);
            UpdateTransactionAmount();
        }
    }
    public int _transactionAmount;
    public float balanceChange;

    public TMP_Text highestPriceText;
    public TMP_Text averagePriceText;
    public TMP_Text lowestPriceText;
    public TMP_Text expandedPrice;
    public Image expandedPriceIcon;

    public Sprite upIcon;
    public Sprite sameIcon;
    public Sprite downIcon;

    private Coroutine holdCoroutine = null;
    private float holdTime = 0f;
    private float holdThreshold = 1f;

    public void SetupExpandedItem(MarketItem item)
    {
        collapsedItem = item;
        attachedInventoryItem = GameManager.INM.FindInventoryItemByID(item.attachedItemCard.cardId);
        expandedImage.sprite = collapsedItem.attachedItemCard.cardSprite;
        expandedName.text = collapsedItem.attachedItemCard.itemName;
        expandedQuantity.text = attachedInventoryItem.ItemQuantity.ToString();
        highestPriceText.text = collapsedItem.itemPrices.Max().ToString("F2") + " ₴";
        averagePriceText.text = collapsedItem.itemPrices.Average().ToString("F2") + " ₴";
        lowestPriceText.text = collapsedItem.itemPrices.Min().ToString("F2") + " ₴";
        expandedPrice.text = collapsedItem.attachedItemCard.itemPrice.ToString("F2") + " ₴";
        collapsedItem.maxBuyAmount = Mathf.FloorToInt(GameManager.UM.Balance / collapsedItem.attachedItemCard.itemPrice);
        SetPriceIcons();
        TransactionAmount = 0;
    }

    public void CollapseMarketItem()
    {
        if(this.gameObject.activeSelf)
        {
            int itemIndex = collapsedItem.transform.GetSiblingIndex();
            int rowStartIndex = (itemIndex / 4) * 4;

            for (int i = 0; i < 3; i++)
            {
                GameObject fillItem = GameManager.MM.marketContentArea.transform.GetChild(1 + i).gameObject;
                Destroy(fillItem);
            }

            TransactionAmount = 0;
            collapsedItem.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }

        collapsedItem.ResetTransactionAmount();
    }

    public void DecreaseAmount()
    {
        TransactionAmount = Mathf.Max(0, TransactionAmount - 1);
    }

    public void IncreaseAmount()
    {
        int maxAmount;
        if (marketTransaction == "Buy")
        {
            collapsedItem.CalculateMaxBuyAmount();
            maxAmount = collapsedItem.maxBuyAmount;
        }
        else // Sell
        {
            maxAmount = attachedInventoryItem.ItemQuantity;
        }

        TransactionAmount = Mathf.Min(maxAmount, TransactionAmount + 1);
    }

    public void SetMax()
    {
        int maxAmount;
        if (marketTransaction == "Buy")
        {
            collapsedItem.CalculateMaxBuyAmount();
            maxAmount = collapsedItem.maxBuyAmount;
        }
        else // Sell
        {
            maxAmount = attachedInventoryItem.ItemQuantity;
        }

        TransactionAmount = maxAmount;
    }

    public void InputTransactionAmount()
    {
        if (transactionAmountInput.text == "")
        {
            transactionAmountInput.text = "";
            return;
        }

        int maxAmount;
        if (marketTransaction == "Buy")
        {
            collapsedItem.CalculateMaxBuyAmount();
            maxAmount = collapsedItem.maxBuyAmount;
        }
        else // Sell
        {
            maxAmount = attachedInventoryItem.ItemQuantity;
        }

        TransactionAmount = Mathf.Clamp(int.Parse(transactionAmountInput.text), 0, maxAmount);
    }

    public void UpdateTransactionAmount()
    {
        canTransaction = TransactionAmount > 0 &&
                         ((marketTransaction == "Buy" && TransactionAmount <= collapsedItem.maxBuyAmount) ||
                          (marketTransaction == "Sell" && TransactionAmount <= attachedInventoryItem.ItemQuantity));

        transactionInputBackground.sprite = canTransaction ? validTransaction : invalidTransaction;
        transactionAmountInput.text = TransactionAmount.ToString();

        if (TransactionAmount != 0)
        {
            balanceChange = (marketTransaction == "Buy")
                ? -TransactionAmount * collapsedItem.attachedItemCard.itemPrice
                : TransactionAmount * collapsedItem.attachedItemCard.itemPrice;
        }
        else
        {
            balanceChange = 0;
        }

        balanceChangeText.text = FormatNumber(balanceChange) + " ₴";
    }

    public void OnTransactionButtonPress()
    {
        if (canTransaction)
        {
            if (holdCoroutine == null)
            {
                holdCoroutine = StartCoroutine(HandleCraftHold());
            }
            transactionInputBackground.sprite = ongoingTransaction;
        }
    }

    public void OnTransactionButtonRelease()
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
        UpdateTransactionAmount();
    }

    private IEnumerator HandleCraftHold()
    {
        holdTime = 0f;
        while (holdTime < holdThreshold)
        {
            holdTime += Time.deltaTime;
            yield return null;
        }

        Transaction();
    }

    private void Transaction()
    {
        holdCoroutine = null;
        GameManager.UM.Balance += balanceChange;

        if (marketTransaction == "Sell")
        {
            attachedInventoryItem.ItemQuantity -= TransactionAmount;
        }
        else // Buy
        {
            attachedInventoryItem.ItemQuantity += TransactionAmount;
        }

        TransactionAmount = 0;
        expandedQuantity.text = attachedInventoryItem.ItemQuantity.ToString();
    }

    public void SetPriceIcons()
    {
        if (collapsedItem.itemPrices[0] < collapsedItem.itemPrices[collapsedItem.itemPrices.Count-1])
        {
            expandedPriceIcon.sprite = downIcon; // Price decrease
        }
        else if (collapsedItem.itemPrices[0] > collapsedItem.itemPrices[collapsedItem.itemPrices.Count - 1])
        {
            expandedPriceIcon.sprite = upIcon; // Price increase
        }
        else
        {
            expandedPriceIcon.sprite = sameIcon;
        }
    }

    public static string FormatNumber(float num)
    {
        if (num >= 1000000000)
            return (num / 1000000000f).ToString("0.##") + "B";
        if (num >= 1000000)
            return (num / 1000000f).ToString("0.##") + "M";
        if (num >= 1000)
            return (num / 1000f).ToString("0.##") + "K";

        return num.ToString("0.##");
    }
}