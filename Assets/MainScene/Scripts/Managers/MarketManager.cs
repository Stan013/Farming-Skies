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
    private float[] marketWeights = {1f, 1.2f, 1.4f, 1.6f, 1.7f, 1.8f, 1.7f, 1.6f, 1.4f, 1.2f, 1f};
    private int[] marketChangesDecimal = {1, 2, 3, 4, 5, 6, 7, 8, 9};
    private float[] marketWeightsDecimal = {2.5f, 2.4f, 2.3f, 2.1f, 1.8f, 1.6f, 1.4f, 1.2f, 1f};

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

        int randomDemand = GetRandomPercentage(marketWeights, marketChanges);
        int randomSupply = GetRandomPercentage(marketWeights, marketChanges);

        int priceChange = randomDemand - randomSupply;
        float randomDecimal = GetRandomPercentage(marketWeightsDecimal, marketChangesDecimal);
        float totalPriceChangePercentage = priceChange + (randomDecimal / 10 * Mathf.Sign(priceChange));

        float newPrice = currentPrice * (1 + totalPriceChangePercentage / 100f);
        newPrice = Mathf.Max(1f, (float)Math.Round(newPrice, 2, MidpointRounding.AwayFromZero));
        int newDemand = (int)Math.Round(baseDemand * (1 + randomDemand / 100f), 2, MidpointRounding.AwayFromZero);
        int newSupply = (int)Math.Round(baseSupply * (1 + randomSupply / 100f), 2, MidpointRounding.AwayFromZero);

        item.attachedItemCard.itemPrice = newPrice;
        item.itemPrices.Insert(0, newPrice);
        item.attachedItemCard.itemDemand = newDemand;
        item.itemDemands.Insert(0, newDemand);
        item.attachedItemCard.itemSupply = newSupply;
        item.itemSupplies.Insert(0, newSupply);
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
