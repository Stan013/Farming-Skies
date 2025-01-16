using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class MarketManager : MonoBehaviour
{
    [SerializeField] private List<MarketItem> m_ItemsInMarket;
    public List<MarketItem> itemsInMarket { get { return m_ItemsInMarket; } }
    [SerializeField] private GameObject marketWindow;
    private float increaseX = 450f;
    private int[] marketChanges = {-5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5};
    private float[] marketWeights = {1f, 1.2f, 1.4f, 1.6f, 1.7f, 1.8f, 1.7f, 1.6f, 1.4f, 1.2f, 1f};
    private int[] marketChangesDecimal = {1, 2, 3, 4, 5, 6, 7, 8, 9};
    private float[] marketWeightsDecimal = {2.5f, 2.4f, 2.3f, 2.1f, 1.8f, 1.6f, 1.4f, 1.2f, 1f};

    public void UpdateMarketItems()
    {
        foreach(MarketItem marketItem in itemsInMarket)
        {
            marketItem.SetItemQuantity();
            marketItem.sellButton.inputAmount.text = "";
            marketItem.buyButton.inputAmount.text = "";
        }
    }

    public void UnlockMarketItem(Card itemCard)
    {
        MarketItem marketItem = Instantiate(itemCard.marketItem, Vector3.zero, Quaternion.identity);
        marketItem.SetMarketItem(itemCard);
        itemsInMarket.Add(marketItem);
        marketItem.transform.SetParent(marketWindow.transform);
        marketItem.transform.localRotation = Quaternion.identity;
        marketItem.transform.localScale = Vector3.one;
        if (marketItem.itemIndex < 4)
        {
            marketItem.transform.localPosition = new Vector3(-675f + increaseX * (marketItem.itemIndex), 160f, 0);
        }
        else
        {
            marketItem.transform.localPosition = new Vector3(-675f + increaseX * (marketItem.itemIndex-4), -270f, 0);
        }
        itemCard.cardAddedToMarket = true;
    }

    public void UpdatePrices()
    {
        foreach (MarketItem item in itemsInMarket)
        {
            float currentPrice = item.priceCurrent;
            float baseDemand = item.itemDemand;
            float baseSupply = item.itemSupply;
            int randomDemand = GetRandomPercentage(marketWeights, marketChanges);
            int randomSupply = GetRandomPercentage(marketWeights, marketChanges);
            int priceChange = randomDemand - randomSupply;
            float randomDecimal = GetRandomPercentage(marketWeightsDecimal, marketChangesDecimal);
            float totalPriceChangePercentage = priceChange + (randomDecimal / 10);
            item.itemDemand = (float)Math.Round(baseDemand * (1 + randomDemand / 100f), 2, MidpointRounding.AwayFromZero);
            item.itemSupply = (float)Math.Round(baseSupply * (1 + randomSupply / 100f), 2, MidpointRounding.AwayFromZero);
            item.priceCurrent = (float)Math.Round(currentPrice * (1 + totalPriceChangePercentage / 100f), 2, MidpointRounding.AwayFromZero);
            item.allItemPrices.Add(item.priceCurrent);
            item.UpdateLowHighPrices();
            item.UpdateMarketItem();
        }
    }

    public int GetRandomPercentage(float[] marketWeightsValues, int[] marketChangesValues)
    {
        float totalWeight = 0f;
        for (int i = 0; i < marketWeightsValues.Length; i++)
        {
            totalWeight += marketWeightsValues[i];
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < marketWeightsValues.Length; i++)
        {
            cumulativeWeight += marketWeightsValues[i];
            if (randomValue < cumulativeWeight)
            {
                return marketChangesValues[i];
            }
        }
        return 0;
    }

    public void ExecuteTransaction(MarketItem marketItem, int amount, bool isSelling)
    {
        if (amount <= 0)
        {
            return;
        }
        if (isSelling)
        {
            amount = Mathf.Min(amount, marketItem.itemQuantity);
            marketItem.itemQuantity -= amount;
            float totalEarned = amount * marketItem.priceCurrent;
            GameManager.UM.balance += totalEarned;
        }
        else
        {
            float itemPrice = marketItem.priceCurrent;
            int affordableAmount = (int)(GameManager.UM.balance / itemPrice);
            amount = Mathf.Min(amount, affordableAmount);
            marketItem.itemQuantity += amount;
            GameManager.UM.balance -= amount * itemPrice;
        }
        UpdateMarketItems();
    }

    public void CloseWindow()
    {
        marketWindow.SetActive(false);
    }
}
