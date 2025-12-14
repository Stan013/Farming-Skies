using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class MarketManager : MonoBehaviour, IDataPersistence
{
    [Header("Market lists")]
    public List<MarketItem> itemsInMarket;

    [Header("Market variables")]
    public GameObject marketContentArea;
    public MarketItem marketItemTemplate;
    public ScrollRect marketScroll;
    public ExpandedMarketItem expandedMarketItem;
    public string marketTab;
    public Button closeButton;

    private int[] marketChanges = {-5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5};
    private float[] marketWeights = {2.78f, 5.56f, 8.33f, 11.11f, 13.89f, 16.67f, 13.89f, 11.11f, 8.33f, 5.56f, 2.78f};
    private int[] marketChangesDecimal = {1, 2, 3, 4, 5, 6, 7, 8, 9};
    private float[] marketWeightsDecimal = {16.67f, 15f, 13f, 12f, 11f, 10f, 9f, 8f, 5.33f};

    public void SetupMarket()
    {
        foreach(Card card in GameManager.CM.unlockedCards)
        {
            if(!card.cardAddedToMarket)
            {
                UnlockMarketItem(card);
            }
        }
    }

    public void UnlockMarketItem(Card itemCard)
    {
        MarketItem marketItem = Instantiate(marketItemTemplate, Vector3.zero, Quaternion.identity, marketContentArea.transform);
        marketItem.SetMarketItem(itemCard);
        marketItem.transform.localRotation = Quaternion.identity;
        marketItem.transform.localPosition = new Vector3(marketItem.transform.localPosition.x, marketItem.transform.localPosition.y, 0);
        itemsInMarket.Add(marketItem);
        itemsInMarket.Sort((a, b) => a.attachedItemCard.itemName.CompareTo(b.attachedItemCard.itemName));
        for (int i = 0; i < itemsInMarket.Count; i++)
        {
            itemsInMarket[i].transform.SetSiblingIndex(i+1);
        }
        itemCard.cardAddedToMarket = true;
    }

    public void FilterItemsInMarket(string filter)
    {
        marketTab = filter;
        if (expandedMarketItem.gameObject.activeSelf)
        {
            expandedMarketItem.CollapseMarketItem();
        }
        if (filter == "Default")
        {
            foreach (MarketItem marketItem in itemsInMarket)
            {
                marketItem.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (MarketItem marketItem in itemsInMarket)
            {
                if (marketItem.attachedItemCard.plantGroup != filter)
                {
                    marketItem.gameObject.SetActive(false);
                }
                else
                {
                    marketItem.gameObject.SetActive(true);
                }
            }
        }
    }

    public int GetRandomPercentage(float[] weights, int[] values)
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        float cumulative = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (roll <= cumulative)
                return values[i];
        }

        return values[values.Length - 1];
    }

    public void MarketUpdate()
    {
        foreach (MarketItem marketItem in itemsInMarket)
        {
            if(marketItem.attachedInventoryItem == null)
            {
                marketItem.attachedInventoryItem = GameManager.INM.FindInventoryItemByID(marketItem.attachedItemCard.cardId);
            }
            GenerateNewMarket(marketItem);
        }
    }

    public void GenerateNewMarket(MarketItem item)
    {
        float currentPrice = item.attachedItemCard.itemPrice;
        float baseDemand = item.attachedItemCard.itemDemand;
        float baseSupply = item.attachedItemCard.itemSupply;

        int demandRoll = GetRandomPercentage(marketWeights, marketChanges);
        int supplyRoll = GetRandomPercentage(marketWeights, marketChanges);

        int priceChangeIntegerPart = demandRoll - supplyRoll;

        int decimalRoll = GetRandomPercentage(marketWeightsDecimal, marketChangesDecimal);
        float decimalAdjustment = decimalRoll / 10f;

        float totalPriceChangePercentage;

        if (priceChangeIntegerPart > 0)
        {
            totalPriceChangePercentage = priceChangeIntegerPart + decimalAdjustment;
        }
        else if (priceChangeIntegerPart < 0)
        {
            totalPriceChangePercentage = priceChangeIntegerPart - decimalAdjustment;
        }
        else
        {
            totalPriceChangePercentage = UnityEngine.Random.value < 0.5f ? decimalAdjustment : -decimalAdjustment;
        }

        float newPrice = currentPrice * (1 + totalPriceChangePercentage / 100f);

        newPrice = Mathf.Max(1f, (float)Math.Round(newPrice, 2, MidpointRounding.AwayFromZero));

        int newDemand = (int)Math.Round(baseDemand * (1 + demandRoll / 100f), MidpointRounding.AwayFromZero);
        int newSupply = (int)Math.Round(baseSupply * (1 + supplyRoll / 100f), MidpointRounding.AwayFromZero);

        item.attachedItemCard.itemPrice = newPrice;
        item.itemPrices.Insert(0, newPrice);

        item.attachedItemCard.itemDemand = newDemand;
        item.itemDemands.Insert(0, newDemand);

        item.attachedItemCard.itemSupply = newSupply;
        item.itemSupplies.Insert(0, newSupply);

        Debug.Log(
            $"[MARKET UPDATE] {item.attachedItemCard.cardId}\n" +
            $"Demand roll: {demandRoll} → {newDemand}\n" +
            $"Supply roll: {supplyRoll} → {newSupply}\n" +
            $"Total price change: {totalPriceChangePercentage:F2}%\n" +
            $"Price history (new → old): {string.Join(", ", item.itemPrices)}\n" +
            $"Demand history (new → old): {string.Join(", ", item.itemDemands)}\n" +
            $"Supply history (new → old): {string.Join(", ", item.itemSupplies)}"
        );
    }

    public MarketItem FindMarketItemByID(string id)
    {
        return itemsInMarket.Find(marketItem => marketItem.attachedItemCard.cardId == id);
    }

    public void LoadData(GameData data)
    {
        SetupMarket();
        for (int i = 0; i < data.marketsMap.Count; i++)
        {
            itemsInMarket[i].LoadMarketData(data.marketsMap[i]);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.marketsMap.Clear();
        foreach (MarketItem item in itemsInMarket)
        {
            data.marketsMap.Add(item.SaveMarketData());
        }
    }
}
