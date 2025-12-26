using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class MarketData
{
    public string cardID;
    public List<float> itemPrices;
    public List<int> itemDemands;
    public List<int> itemSupplies;

    public MarketData(string cardIDData, List<float> itemPricesData, List<int> itemDemandsData, List<int> itemSuppliesData)
    {
        cardID = cardIDData;
        itemPrices = itemPricesData;
        itemDemands = itemDemandsData;
        itemSupplies = itemSuppliesData;
    }
}

[System.Serializable]
public class PlantData
{
    public string plantID;
    public string islandID;
    public string plotID;
    public string plantSize;

    public PlantData(string plantIDData, string islandIDData, string plotIDData, string plantSizeData)
    {
        plantID = plantIDData;
        islandID = islandIDData;
        plotID = plotIDData;
        plantSize = plantSizeData;
    }
}

[System.Serializable]
public class IslandData
{
    public bool islandAvailable;
    public bool islandBought;
    public string islandID;
    public int islandState;
    public List<int> nutrientsAvailable;

    // Plant Manager Data
    public List<PlantData> plantsMap;

    public IslandData(bool islandAvailableData, bool islandBoughtData, string islandIDData, int islandStateData, List<int> nutrientsAvailableData, List<PlantData> plantsMapData)
    {
        islandAvailable = islandAvailableData;
        islandBought = islandBoughtData;
        islandID = islandIDData;
        islandState = islandStateData;
        nutrientsAvailable = nutrientsAvailableData;
        plantsMap = plantsMapData;
    } 
}

[System.Serializable]
public class GameData
{
    // Game Manager Data
    public GameManager.GameMode gameMode;

    // Card Manager Data
    public List<string> unlockedCardsID = new List<string>();
    public List<string> inspectedCardsID = new List<string>();

    // Input Manager Data
    public Vector3 playerPosition;

    // Quest Manager Data
    public bool questActive;
    public int questCount;

    // Level Manager Data
    public int farmLevel;

    // UI Manager Data
    public float expense;
    public float balance;
    public int water;
    public int fertiliser;

    // Time Manager Data
    public int weeks;

    // Deck Manager Data
    public List<string> cardsInDeck = new List<string>();

    // Hand Manager Data
    public List<string> cardsInHand = new List<string>();

    // Island Manager Data
    public List<IslandData> islandsMap = new List<IslandData>();
    public int islandValue;

    // Inventory Manager Data
    public List<int> itemQuantities = new List<int>();

    // Market Manager Data
    public List<MarketData> marketsMap = new List<MarketData>();

    // Plant Manager Data
    public float plantValue;
    public float buildableValue;

    // Expense Manager Data
    public List<string> islandExpenses = new List<string>();
    public List<string> buildableExpenses = new List<string>();

    // Event Manager Data
    public int lastEvent;

    public GameData(float startBalance, int startWater, int startFertiliser)
    {
        questCount = 0;
        farmLevel = 0;
        expense = 0f;
        balance = startBalance;
        water = startWater;
        fertiliser = startFertiliser;
        weeks = 0;
    }
}
