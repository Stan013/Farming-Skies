using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


[System.Serializable]
public class IslandData
{
    public List<string> usedPlotNames = new List<string>();
    public List<string> itemsOnIsland = new List<string>();
    public string islandId;

    public IslandData(List<string> usedPlotNamesList, List<string> plantIdList, string islandID)
    {
        usedPlotNames = usedPlotNamesList;
        itemsOnIsland = plantIdList;
        islandId = islandID;
    }
}

[System.Serializable]
public class GameData
{
    public GameManager.GameState CurrentState;
    public Vector3 playerPosition;
    public Vector3 cameraDirection;

    public float tax;
    public float balance;
    public float water;
    public List<int> date = new List<int> { 1, 1, 2025 };

    public List<string> starterCards = new List<string>();
    public List<string> cardsInDeck = new List<string>();
    public List<string> cardsInHand = new List<string>();

    public List<string> boughtIslands = new List<string>();
    public List<IslandData> islandDataMap = new List<IslandData>();

    public GameData()
    {
        tax = 1000f;
        balance = 500f;
        water = 3f;
        playerPosition = new Vector3(0, 0, 0);
        cameraDirection = new Vector3(0, 10, 0);
    }
}
