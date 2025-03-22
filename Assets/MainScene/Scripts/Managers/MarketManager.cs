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

    public GameObject spawnResources;
    public Resource coinIcon;

    public void UpdateMarketItems()
    {
        foreach(MarketItem marketItem in itemsInMarket)
        {
            marketItem.itemQuantityText.SetText(marketItem.attachedItemCard.itemQuantity.ToString());
            marketItem.sellUI.inputAmountField.text = "0";
            marketItem.buyUI.inputAmountField.text = "0";
        }
    }

    public void UnlockMarketItem(Card itemCard)
    {
        MarketItem marketItem = Instantiate(itemCard.marketItem, Vector3.zero, Quaternion.identity, marketContentArea.transform);
        marketItem.transform.localRotation = Quaternion.identity;
        marketItem.transform.localPosition = new Vector3(marketItem.transform.localPosition.x, marketItem.transform.localPosition.y, 0);
        marketItem.SetMarketItem(itemCard);
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
        if(GameManager.UM.weeks != 0)
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
        if (amount <= 0) return;
        Vector3 startPos;
        Vector3 endPos;
        float totalBalance = 0f;
        if (isSelling)
        {
            amount = Mathf.Min(amount, marketItem.attachedItemCard.itemQuantity);
            marketItem.attachedItemCard.itemQuantity -= amount;
            totalBalance = amount * marketItem.priceCurrent;
            GameManager.UM.balance += totalBalance;
            startPos = marketItem.sellUI.transactionButton.transform.position;
            endPos = GameManager.UM.transform.position;
        }
        else
        {
            float itemPrice = marketItem.priceCurrent;
            int affordableAmount = (int)(GameManager.UM.balance / itemPrice);
            amount = Mathf.Min(amount, affordableAmount);
            marketItem.attachedItemCard.itemQuantity += amount;
            totalBalance = amount * itemPrice;
            GameManager.UM.balance -= totalBalance;
            startPos = GameManager.UM.transform.position;
            endPos = marketItem.buyUI.transactionButton.transform.position;
        }

        int coinCount = Mathf.Max(1, Mathf.FloorToInt(totalBalance / 50));
        StartCoroutine(SpawnCoins(startPos, endPos, coinCount, marketItem));
        UpdateMarketItems();
        GameManager.UM.UpdateUI();
    }


    private IEnumerator SpawnCoins(Vector3 start, Vector3 end, int coinCount, MarketItem marketItem)
    {
        for (int i = 0; i < coinCount; i++)
        {
            Resource coin = Instantiate(coinIcon, Vector3.zero, Quaternion.identity, spawnResources.transform);
            coin.transform.localPosition = new Vector3(marketItem.transform.position.x + UnityEngine.Random.Range(-250f, 250f), marketItem.transform.position.y + UnityEngine.Random.Range(-250f, 250f), 0f);
            coin.transform.localRotation = Quaternion.identity;
            StartCoroutine(MoveCoin(coin, end));
        }
        yield return null;
    }

    private IEnumerator MoveCoin(Resource coin, Vector3 target)
    {
        float speed = Vector3.Distance(coin.transform.position, target);

        while (Vector3.Distance(coin.transform.position, target) > 0.01f)
        {
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        coin.transform.position = target;
        Destroy(coin.gameObject);
    }

    public MarketItem FindMarketItemByName(string itemName)
    {
        return itemsInMarket.Find(marketItem => marketItem.attachedItemCard.itemName == itemName);
    }

}
