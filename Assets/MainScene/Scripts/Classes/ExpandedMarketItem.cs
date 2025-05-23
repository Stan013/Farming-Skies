﻿using System.Collections;
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
    public Sprite validSell;
    public Sprite invalidSell;

    public string marketTransaction;
    public Image transactionInputBackground;
    public Sprite validTransaction;
    public Sprite ongoingTransaction;
    public Sprite invalidTransaction;
    public bool canTransaction;

    public Button minButton;
    public Button minusButton;
    public TMP_InputField transactionAmountInput;
    public int transactionAmount;
    public Button plusButton;
    public Button maxButton;

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
        CheckValidTransactionAmount("0");
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

            collapsedItem.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }
    }

    public void SetMin()
    {
        transactionAmount = 0;
        transactionAmountInput.text = "0";
        CheckValidTransactionAmount("0");
    }

    public void DecreaseAmount()
    {
        int amount = transactionAmount;
        amount = Mathf.Max(0, amount - 1);
        transactionAmountInput.text = amount.ToString();
        CheckValidTransactionAmount(amount.ToString());
    }

    public void IncreaseAmount()
    {
        int amount = transactionAmount;
        int maxAmount;

        if (marketTransaction == "Buy")
        {
            maxAmount = collapsedItem.maxBuyAmount;
        }
        else // Sell
        {
            maxAmount = attachedInventoryItem.ItemQuantity;
        }

        amount = Mathf.Min(maxAmount, amount + 1);
        transactionAmountInput.text = amount.ToString();
        CheckValidTransactionAmount(amount.ToString());
    }

    public void SetMax()
    {
        int maxAmount;

        if (marketTransaction == "Buy")
        {
            maxAmount = collapsedItem.maxBuyAmount;
        }
        else // Sell
        {
            maxAmount = attachedInventoryItem.ItemQuantity;
        }

        transactionAmount = maxAmount;
        transactionAmountInput.text = maxAmount.ToString();
        CheckValidTransactionAmount(maxAmount.ToString());
    }

    public void CheckValidTransactionAmount(string input)
    {
        if (!int.TryParse(input, out int value))
        {
            value = 0;
        }

        transactionAmount = value;
        transactionAmountInput.text = value.ToString();

        if (marketTransaction == "Sell")
        {
            int availableToSell = attachedInventoryItem.ItemQuantity;

            if (value <= 0 || availableToSell == 0)
            {
                transactionInputBackground.sprite = invalidTransaction;
                canTransaction = false;
                transactionButton.interactable = false;
                transactionButton.GetComponent<Image>().sprite = invalidSell;
            }
            else if (value > availableToSell)
            {
                transactionAmount = availableToSell;
                transactionAmountInput.text = availableToSell.ToString();

                transactionInputBackground.sprite = validTransaction;
                canTransaction = true;
                transactionButton.interactable = true;
                transactionButton.GetComponent<Image>().sprite = validSell;
            }
            else
            {
                transactionInputBackground.sprite = validTransaction;
                canTransaction = true;
                transactionButton.interactable = true;
                transactionButton.GetComponent<Image>().sprite = validSell;
            }
        }
        else // Buy
        {
            float price = collapsedItem.attachedItemCard.itemPrice;
            float balance = GameManager.UM.Balance;
            collapsedItem.maxBuyAmount = Mathf.FloorToInt(balance / price);
            float totalCost = value * price;

            if (value <= 0 || collapsedItem.maxBuyAmount == 0 || totalCost > balance)
            {
                if (collapsedItem.maxBuyAmount > 0)
                {
                    transactionAmount = collapsedItem.maxBuyAmount;
                    transactionAmountInput.text = collapsedItem.maxBuyAmount.ToString();

                    transactionInputBackground.sprite = validTransaction;
                    canTransaction = true;
                    transactionButton.interactable = true;
                    transactionButton.GetComponent<Image>().sprite = validSell;
                }
                else
                {
                    transactionInputBackground.sprite = invalidTransaction;
                    canTransaction = false;
                    transactionButton.interactable = false;
                    transactionButton.GetComponent<Image>().sprite = invalidSell;

                }
            }
            else
            {
                transactionInputBackground.sprite = validTransaction;
                canTransaction = true;
                transactionButton.interactable = true;
                transactionButton.GetComponent<Image>().sprite = validSell;
            }
        }
    }


    public void SwitchTransaction()
    {
        if (marketTransaction == "Sell")
        {
            transactionText.text = "Buy";
            marketTransaction = "Buy";
        }
        else
        {
            transactionText.text = "Sell";
            marketTransaction = "Sell";
        }
        CheckValidTransactionAmount("0");
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
        CheckValidTransactionAmount("0");
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

        if (marketTransaction == "Sell")
        {
            float sellTotal = transactionAmount * collapsedItem.attachedItemCard.itemPrice;
            GameManager.UM.Balance += sellTotal;
            attachedInventoryItem.ItemQuantity -= transactionAmount;
        }
        else
        {
            float buyTotal = transactionAmount * collapsedItem.attachedItemCard.itemPrice;
            GameManager.UM.Balance -= buyTotal;
            attachedInventoryItem.ItemQuantity += transactionAmount;
        }
        CheckValidTransactionAmount("0");
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

        return num.ToString("0");
    }
}