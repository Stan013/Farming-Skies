using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

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

    // Inventory Manager Data
    public List<int> itemQuantities = new List<int>();

    public GameData()
    {
        questCount = 0;
        farmLevel = 0;
        expense = 0f;
        balance = 0f;
        water = 0;
        fertiliser = 0;
        weeks = 0;
    }
}
