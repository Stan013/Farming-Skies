using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class MarketItem : MonoBehaviour
{
    public TMP_Text itemNameText;
    public Image itemImage;
    public int itemIndex;
    public Card attachedItemCard;
    public InventoryItem attachedInventoryItem;
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
                if (value <= 0 || attachedInventoryItem.ItemQuantity == 0)
                {
                    transactionAmount = 0;
                    transactionAmountInput.text = "0";
                    transactionInputBackground.sprite = invalidTransaction;
                    canTransaction = false;
                }
                else if (value > attachedInventoryItem.ItemQuantity)
                {
                    if (attachedInventoryItem.ItemQuantity == 0)
                    {
                        transactionInputBackground.sprite = invalidTransaction;
                        canTransaction = false;
                    }
                    else
                    {
                        transactionInputBackground.sprite = validTransaction;
                        canTransaction = true;
                    }
                    transactionAmount = attachedInventoryItem.ItemQuantity;
                    transactionAmountInput.text = attachedInventoryItem.ItemQuantity.ToString();
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
                if (transactionCost <= 0 || maxBuyAmount == 0)
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
        CheckValidTransactionAmount("0");
    }

    public void OnTransactionButtonPress()
    {
        if (canTransaction)
        {
            if (holdCoroutine == null)
            {
                holdCoroutine = StartCoroutine(HandleTransactionHold());
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

    private IEnumerator HandleTransactionHold()
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

        if(marketTransaction == "Sell")
        {
            float sellTotal = transactionAmount * attachedItemCard.itemPrice;
            GameManager.UM.Balance += sellTotal;
            attachedInventoryItem.ItemQuantity -= transactionAmount;
        }
        else
        {
            float buyTotal = transactionAmount * attachedItemCard.itemPrice;
            GameManager.UM.Balance -= buyTotal;
            attachedInventoryItem.ItemQuantity += transactionAmount;
        }
        CheckValidTransactionAmount("0");
    }

    public void ExpandMarketItem()
    {
        if (GameManager.MM.expandedMarketItem.gameObject.activeSelf)
        {
            GameManager.MM.expandedMarketItem.CollapseMarketItem();
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject fillerItem = Instantiate(GameManager.MM.fillerItem, transform.parent);
            fillerItem.transform.SetSiblingIndex(1);
        }

        GameManager.MM.expandedMarketItem.SetupExpandedItem(this);
        GameManager.MM.expandedMarketItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        GameManager.MM.marketScroll.verticalNormalizedPosition = 1f;
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
 