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
    public Vector3 playerPosition;
    public Vector3 cameraDirection;

    public int farmLevel;
    public float expense;
    public float balance;
    public int water;
    public int fertiliser;
    public int weeks;
    public List<string> cardsInDeck = new List<string>();

    public List<string> cardsInHand = new List<string>();

    public List<string> boughtIslands = new List<string>();
    public List<IslandData> islandDataMap = new List<IslandData>();

    public GameData()
    {
        farmLevel = 1;
        expense = 0f;
        balance = 0f;
        water = 0;
        fertiliser = 0;
        weeks = 1;
    }
}
