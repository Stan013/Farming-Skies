using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour, IDataPersistence
{
    //Manager
    public List<Island> availableIslands = new List<Island>();
    public List<Island> boughtIslands = new List<Island>();
    public List<Island> unboughtIslands = new List<Island>();

    public Material sowedMat;
    public Material sowedNeedsNPKMat;
    public Material cultivatedMat;
    public Material cultivatedNeedsNPKMat;
    public Material wateredMat;
    public Material wateredNeedsNPKMat;

    public void AddIslandToBought(Island reconstructedIsland)
    {
        if(GameManager.TTM.tutorial && reconstructedIsland.name == "0,0")
        {
            GameManager.TTM.QuestCompleted = true; 
        }
        reconstructedIsland.ToggleState(Island.IslandState.Sowed, Island.IslandState.Default);
        boughtIslands.Add(reconstructedIsland);
        unboughtIslands.Remove(reconstructedIsland);
    }

    public void SetCollisions(string cardType)
    {
        switch (cardType)
        {
            case "Utility":
                foreach (Island island in boughtIslands)
                {
                    island.GetComponent<BoxCollider>().enabled = true;
                }
                break;
            case "PlantSmall":
                foreach (Island island in boughtIslands)
                {
                    if (island.currentState == Island.IslandState.Watered)
                    {
                        foreach (GameObject plot in island.plotsSmallPlants)
                        {
                            plot.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                break;
            case "PlantMedium":
                foreach (Island island in boughtIslands)
                {
                    if (island.currentState == Island.IslandState.Watered)
                    {
                        foreach (GameObject plot in island.plotsMediumPlants)
                        {
                            plot.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                break;
            case "PlantBig":
                foreach (Island island in boughtIslands)
                {
                    if (island.currentState == Island.IslandState.Watered)
                    {
                        foreach (GameObject plot in island.plotsBigPlants)
                        {
                            plot.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                break;
            case "Machine":
                foreach (Island island in boughtIslands)
                {
                    island.GetComponent<BoxCollider>().enabled = false;
                    foreach (GameObject plot in island.plotsMediumPlants)
                    {
                        plot.GetComponent<BoxCollider>().enabled = true;
                    }
                }
                break;
            default:
                foreach (Island island in boughtIslands)
                {
                    island.GetComponent<BoxCollider>().enabled = false;
                    foreach (GameObject plot in island.plotsSmallPlants)
                    {
                        plot.GetComponent<BoxCollider>().enabled = false;
                    }
                    foreach (GameObject plot in island.plotsMediumPlants)
                    {
                        plot.GetComponent<BoxCollider>().enabled = false;
                    }
                    foreach (GameObject plot in island.plotsBigPlants)
                    {
                        plot.GetComponent<BoxCollider>().enabled = false;
                    }
                }
                break;
        }
    }

    public Island CheckPotentialIsland()

    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out Island island))
            {
                if (island.islandBoughtStatus)
                {
                    return island;
                }
            }
        }
        return null;
    }

    public Island FindIslandById(string id)
    {
        return availableIslands.Find(island => island.islandId == id);
    }

    public void SetPurchasableIslands(bool purchasable)
    {
        foreach(Island island in unboughtIslands)
        {
            island.islandCanBought = purchasable;
        }
    }

    public void IslandNutrients()
    {
        foreach(Island island in boughtIslands)
        {
            if (island.nitrogen <= 0 || island.phosphorus <= 0 || island.potassium <= 0)
            {
                island.needsNPK = true;
                switch (island.currentState)
                {
                    case Island.IslandState.Sowed:
                        island.SetMaterial(sowedNeedsNPKMat);
                        break;
                    case Island.IslandState.Watered:
                        island.SetMaterial(wateredNeedsNPKMat);
                        break;
                    case Island.IslandState.Cultivated:
                        island.SetMaterial(cultivatedNeedsNPKMat);
                        break;
                }
            }
        }
    }

    public bool CheckWater(Plant plant)
    {
        if(GameManager.UM.water >= plant.water)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LoadData(GameData data)
    {
        boughtIslands.Clear();
        var islandDataCount = 0;
        foreach (string islandID in data.boughtIslands)
        {
            var island = FindIslandById(islandID);
            unboughtIslands.Remove(island);
            boughtIslands.Add(island);
            island.islandBoughtStatus = true;
            island.islandBottom.GetComponent<Renderer>().material = island.bottomDefaultMat;
            island.ToggleState(Island.IslandState.Sowed, Island.IslandState.Default);
            island.usedPlots.Clear();
            island.itemsOnIsland.Clear();
            if (data.islandDataMap[islandDataCount].islandId == island.islandId)
            {
                foreach (string plotName in data.islandDataMap[islandDataCount].usedPlotNames)
                {
                    var usedPlot = island.FindPlotByName(plotName);
                    var plantCard = island.FindItemOnIslandByCardId(data.islandDataMap[islandDataCount].itemsOnIsland[islandDataCount]);
                    GameObject plant = Instantiate(plantCard.GetComponent<CardDrag>().dragModel, Vector3.zero, Quaternion.identity);
                    plant.transform.SetParent(usedPlot.transform);
                    plant.transform.localPosition = new Vector3(0, -0.25f, 0);
                    island.MakeUsedPlot(usedPlot, GameManager.HM.dragCard, plant);
                }
                islandDataCount++;
            }
        }

    }

    public void SaveData(ref GameData data)
    {
        data.boughtIslands.Clear();
        data.islandDataMap.Clear();
        foreach (Island island in boughtIslands)
        {
            List<string> usedPlotNamesList = new List<string>();
            foreach(GameObject plot in island.usedPlots)
            {
                usedPlotNamesList.Add(plot.name);
            }
            List<string> plantIdList = new List<string>();
            foreach(Plant plant in island.itemsOnIsland)
            {
                plantIdList.Add(plant.plantCardID);
            }
            IslandData islandData = new IslandData(usedPlotNamesList, plantIdList, island.islandId);
            data.boughtIslands.Add(island.islandId);
            data.islandDataMap.Add(islandData);
        }
    }
}
