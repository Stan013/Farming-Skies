using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItem : MonoBehaviour
{
    public TMP_Text itemNameText;
    public Image itemImage;
    public int itemIndex;
    public Card attachedItemCard;
    public Button expandButton;

    private string marketTransaction = "Sell";
    public Button transactionButton;
    public TMP_Text transactionText;

    public TMP_InputField transactionAmountInput;
    public Image transactionInputBackground;
    public Sprite invalidTransaction;
    public Sprite validTransaction;
    public Sprite ongoingTransaction;

    public int transactionAmount;
    public bool canTransaction;
    public int maxBuyAmount;

    private Coroutine holdCoroutine = null;
    private float holdTime = 0f;
    private float holdThreshold = 1f;

    public void SetMarketItem(Card itemCard)
    {
        if(itemCard != null)
        {
            attachedItemCard = itemCard;
            itemNameText.text = attachedItemCard.itemName;
            itemImage.sprite = attachedItemCard.cardSprite;
            itemIndex = GameManager.MM.itemsInMarket.Count;
            CheckValidTransactionAmount("0");
        }
    }

    public void CheckValidTransactionAmount(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (marketTransaction == "Sell")
            {
                if (value <= 0 || attachedItemCard.itemQuantity == 0)
                {
                    transactionAmount = 0;
                    transactionAmountInput.text = "0";
                    transactionInputBackground.sprite = invalidTransaction;
                    canTransaction = false;
                }
                else if (value > attachedItemCard.itemQuantity)
                {
                    transactionAmount = attachedItemCard.itemQuantity;
                    transactionAmountInput.text = attachedItemCard.itemQuantity.ToString();
                    transactionInputBackground.sprite = validTransaction;
                    canTransaction = true;
                }
                else
                {
                    transactionAmount = value;
                    transactionAmountInput.text = value.ToString();
                    transactionInputBackground.sprite = validTransaction;
                    canTransaction = true;
                }

            }
            else
            {
                maxBuyAmount = Mathf.FloorToInt(GameManager.UM.Balance / attachedItemCard.itemPrice);
                float transactionCost = value * attachedItemCard.itemPrice;
                if (transactionCost <= 0)
                {
                    transactionAmount = 0;
                    transactionAmountInput.text = "0";
                    transactionInputBackground.sprite = invalidTransaction;
                    canTransaction = false;
                }
                else if (transactionCost > GameManager.UM.Balance)
                {
                    transactionAmount = maxBuyAmount;
                    transactionAmountInput.text = maxBuyAmount.ToString();
                    transactionInputBackground.sprite = validTransaction;
                    canTransaction = true;
                }
                else
                {
                    transactionAmount = value;
                    transactionAmountInput.text = value.ToString();
                    transactionInputBackground.sprite = validTransaction;
                    canTransaction = true;
                }
            }
        }
        else
        {
            transactionAmount = 0;
            transactionAmountInput.text = "0";
            transactionInputBackground.sprite = invalidTransaction;
            canTransaction = false;
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
    }

    public void OnCraftButtonPress()
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

    public void OnCraftButtonRelease()
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            CheckValidTransactionAmount("0");
            holdCoroutine = null;
        }
    }

    private IEnumerator HandleCraftHold()
    {
        holdTime = 0f;
        while (holdTime < holdThreshold)
        {
            holdTime += Time.deltaTime;
            yield return null;
        }

        QuickTransaction();
    }

    private void QuickTransaction()
    {
        holdCoroutine = null;
        GameManager.UM.Balance -= attachedItemCard.cardCraftResources[0];
        GameManager.UM.Water -= attachedItemCard.cardCraftResources[1];
        GameManager.UM.Fertiliser -= attachedItemCard.cardCraftResources[2];
        GameManager.DM.AddCardToDeck(attachedItemCard.cardId);
        CheckValidTransactionAmount("0");
    }

    private string FormatNumber(float number)
    {
        if (number >= 1000000000)
        {
            return (number / 1000000000f).ToString("0.0") + "B";
        }
        if (number >= 1000000)
        {
            return (number / 1000000f).ToString("0.0") + "M";
        }
        else if (number >= 1000) 
        {
            return (number / 1000f).ToString("0.0") + "K";
        }
        else
        {
            return number.ToString("0");
        }
    }
}
 