using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItem : MonoBehaviour
{
    public TMP_Text itemNameText;
    public Image itemImage;
    public int itemIndex;
    public Card attachedItemCard;
    public InventoryItem attachedInventoryItem;
    public Button expandButton;
    public MarketData marketData;

    public string marketTransaction = "Sell";
    public Button transactionButton;
    public TMP_Text transactionText;
    public int transactionAmount;
    public Sprite sellTransaction;
    public Sprite buyTransaction;

    public TMP_InputField transactionAmountInput;
    public Image transactionInputBackground;
    public Sprite invalidTransaction;
    public Sprite validTransaction;
    public Sprite ongoingTransaction;

    public List<float> itemPrices;
    public List<int> itemDemands;
    public List<int> itemSupplies;
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
            itemPrices.Add(itemCard.itemPrice);
            itemDemands.Add(itemCard.itemDemand);
            itemSupplies.Add(itemCard.itemSupply);
        }
    }

    public void ResetTransactionAmount()
    {
        transactionAmount = 0;
        transactionAmountInput.text = "";
    }

    public void CheckValidTransaction()
    {
        int input;
        if (transactionAmountInput.text == "")
        {
            input = 0;
        }
        else
        {
            input = int.Parse(transactionAmountInput.text);
        }

        if (input <= 0)
        {
            transactionAmount = 0;
            transactionInputBackground.sprite = invalidTransaction;
            canTransaction = false;
            return;
        }
        else
        {
            if (marketTransaction == "Sell")
            {
                if(input > attachedInventoryItem.ItemQuantity)
                {
                    transactionAmount = attachedInventoryItem.ItemQuantity;
                }
                else
                {
                    transactionAmount = input;
                }
            }
            else
            {
                if(maxBuyAmount <= 0)
                {
                    transactionAmount = 0;
                    transactionInputBackground.sprite = invalidTransaction;
                    canTransaction = false;
                }
                else
                {
                    if (input > maxBuyAmount)
                    {
                        transactionAmount = maxBuyAmount;
                    }
                    else
                    {
                        transactionAmount = input;
                    }

                    transactionInputBackground.sprite = validTransaction;
                    canTransaction = true;
                }
            }
        }

        transactionAmountInput.text = transactionAmount.ToString();
    }

    public void CalculateMaxBuyAmount()
    {
        float price = attachedItemCard.itemPrice;
        float balance = GameManager.UM.Balance;

        maxBuyAmount = Mathf.FloorToInt(balance / price);
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
        ResetTransactionAmount();
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
        ResetTransactionAmount();
    }

    public void ExpandMarketItem()
    {
        if (GameManager.MM.expandedMarketItem.gameObject.activeSelf)
        {
            GameManager.MM.expandedMarketItem.CollapseMarketItem();
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject fillerItem = Instantiate(GameManager.INM.fillerItem, transform.parent);
            fillerItem.transform.SetSiblingIndex(1);
        }

        GameManager.MM.expandedMarketItem.SetupExpandedItem(this);
        GameManager.MM.expandedMarketItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        GameManager.MM.marketScroll.verticalNormalizedPosition = 1f;
    }

    public void LoadMarketData(MarketData data)
    {
        Card itemCard = GameManager.CM.FindCardByID(data.cardID);
        attachedItemCard = itemCard;
        attachedInventoryItem = GameManager.INM.FindInventoryItemByID(data.cardID);
        itemNameText.text = attachedItemCard.itemName;
        itemImage.sprite = attachedItemCard.cardSprite;
        itemIndex = GameManager.MM.itemsInMarket.Count;

        itemPrices.Clear();
        itemDemands.Clear();
        itemSupplies.Clear();
        itemPrices = data.itemPrices;
        itemDemands = data.itemDemands;
        itemSupplies = data.itemSupplies;
        CheckValidTransaction();

        attachedItemCard.itemPrice = itemPrices[0];
        attachedItemCard.itemDemand = itemDemands[0];
        attachedItemCard.itemSupply = itemSupplies[0];
    }

    public MarketData SaveMarketData()
    {
        marketData = new MarketData(attachedItemCard.cardId, itemPrices, itemDemands, itemSupplies);
        return marketData;
    }
}
 