using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class MarketManager : MonoBehaviour
{
    public List<MarketItem> itemsInMarket;
    public GameObject marketWindow;
    public GameObject marketContentArea;
    public Scrollbar marketScrollbar;

    private int[] marketChanges = {-5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5};
    private float[] marketWeights = {1f, 1.2f, 1.4f, 1.6f, 1.7f, 1.8f, 1.7f, 1.6f, 1.4f, 1.2f, 1f};
    private int[] marketChangesDecimal = {1, 2, 3, 4, 5, 6, 7, 8, 9};
    private float[] marketWeightsDecimal = {2.5f, 2.4f, 2.3f, 2.1f, 1.8f, 1.6f, 1.4f, 1.2f, 1f};

    public void UpdateMarketItems()
    {
        foreach(MarketItem marketItem in itemsInMarket)
        {
            marketItem.itemQuantityText.SetText(marketItem.attachedItemCard.itemQuantity.ToString());
            marketItem.sellUI.inputAmount.text = "";
            marketItem.buyUI.inputAmount.text = "";
        }
    }

    public void UnlockMarketItem(Card itemCard)
    {
        MarketItem marketItem = Instantiate(itemCard.marketItem, Vector3.zero, Quaternion.identity);
        marketItem.SetMarketItem(itemCard);
        marketItem.transform.SetParent(marketContentArea.transform, false);
        itemsInMarket.Add(marketItem);
        itemsInMarket.Sort((a, b) => a.attachedItemCard.itemName.CompareTo(b.attachedItemCard.itemName));
        for (int i = 0; i < itemsInMarket.Count; i++)
        {
            itemsInMarket[i].transform.SetSiblingIndex(i);
        }
        itemCard.cardAddedToMarket = true;
    }

    public void UpdatePrices()
    {
        if(GameManager.TM.GetDate() != "01-01-2025")
        {
            foreach (MarketItem item in itemsInMarket)
            {
                float currentPrice = item.priceCurrent;
                float baseDemand = item.attachedItemCard.itemDemand;
                float baseSupply = item.attachedItemCard.itemSupply;
                int randomDemand = GetRandomPercentage(marketWeights, marketChanges);
                int randomSupply = GetRandomPercentage(marketWeights, marketChanges);
                int priceChange = randomDemand - randomSupply;
                float randomDecimal = GetRandomPercentage(marketWeightsDecimal, marketChangesDecimal);
                float totalPriceChangePercentage = priceChange + (randomDecimal / 10 * Mathf.Sign(priceChange));
                item.attachedItemCard.itemDemand = (float)Math.Round(baseDemand * (1 + randomDemand / 100f), 2, MidpointRounding.AwayFromZero);
                item.attachedItemCard.itemSupply = (float)Math.Round(baseSupply * (1 + randomSupply / 100f), 2, MidpointRounding.AwayFromZero);
                item.priceCurrent = (float)Math.Round(currentPrice * (1 + totalPriceChangePercentage / 100f), 2, MidpointRounding.AwayFromZero);
                item.UpdateMarketItem(item.attachedItemCard);
            }
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
            amount = Mathf.Min(amount, marketItem.attachedItemCard.itemQuantity);
            marketItem.attachedItemCard.itemQuantity -= amount;
            float totalEarned = amount * marketItem.priceCurrent;
            GameManager.UM.money += totalEarned;
        }
        else
        {
            float itemPrice = marketItem.priceCurrent;
            int affordableAmount = (int)(GameManager.UM.money / itemPrice);
            amount = Mathf.Min(amount, affordableAmount);
            marketItem.attachedItemCard.itemQuantity += amount;
            GameManager.UM.money -= amount * itemPrice;
        }
        UpdateMarketItems();
        GameManager.UM.UpdateUI();
    }
}
