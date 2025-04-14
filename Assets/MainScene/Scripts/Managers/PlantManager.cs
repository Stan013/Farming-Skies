using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour, IDataPersistence
{
    [Header("Drop variables")]
    public float dropChance;
    private float _plantValue;
    public float PlantValue
    {
        get => _plantValue;
        set
        {
            _plantValue = value;
            GameManager.EM.UpdateFarmValue();
        }
    }
    public float oldPlantValue;
    public float plantValueChange;

    private float _buildablesValue;
    public float BuildablesValue
    {
        get => _buildablesValue;
        set
        {
            _buildablesValue = value;
            GameManager.EM.UpdateFarmValue();
        }
    }
    public float oldBuildableValue;
    public float buildableValueChange;

    public void Harvest()
    {
        foreach (Island island in GameManager.ISM.boughtIslands)
        {
            foreach (Plant plant in island.smallPlantsOnIsland.Concat(island.mediumPlantsOnIsland).Concat(island.largePlantsOnIsland))
            {
                plant.attachedInventoryItem.ItemQuantity += plant.yield;
                if (plant.nutrientsUsages[0] <= island.nutrientsAvailable[0])
                {
                    plant.UpdatePlantYield();
                    StartCoroutine(SpawnDropsWithInterval(plant));
                }
                else
                {
                    island.currentState = Island.IslandState.Cultivated;
                }
            }
        }
    }

    private IEnumerator SpawnDropsWithInterval(Plant plant)
    {
        int dropCount = Mathf.FloorToInt(plant.yield / 3);
        float interval = 0.25f;

        for (int i = 0; i < dropCount; i++)
        {
            plant.GiveDrop(plant.transform.parent);
            yield return new WaitForSeconds(interval);
        }
    }

    public GameObject FindPlotOnIslandByID(Island island, string plotID, string plantSize)
    {
        switch (plantSize)
        {
            case "Small crops":
                return island.availableSmallPlots.Find(plot => plot.name == plotID);
            case "Medium crops":
                return island.availableMediumPlots.Find(plot => plot.name == plotID);
            case "Large crops":
                return island.availableLargePlots.Find(plot => plot.name == plotID);
            case "Buildables":
                return island.availableMediumPlots.Find(plot => plot.name == plotID);
        }
        return null;
    }

    public void SetPlantData(Island island, List<PlantData> plantsMap)
    {
        for (int i = 0; i < plantsMap.Count; i++)
        {
            GameObject plot = FindPlotOnIslandByID(island, plantsMap[i].plotID, plantsMap[i].plantSize);
            GameObject plant = Instantiate(GameManager.CM.FindCardByID(plantsMap[i].plantID).GetComponent<CardDrag>().dragModel, Vector3.zero, Quaternion.identity, plot.transform);
            plant.transform.localPosition = new Vector3(0, -0.25f, 0);
            plant.transform.localRotation = Quaternion.identity;
            island.MakeUsedPlot(plot, GameManager.CM.FindCardByID(plantsMap[i].plantID), plant.GetComponent<Plant>());
        }
    }

    public List<PlantData> GetPlantData(Island island)
    {
        if(island.islandBought)
        {
            List<PlantData> plantsMap = new List<PlantData>();
            foreach (Plant plant in island.itemsOnIsland)
            {
                plantsMap.Add(plant.SavePlantData());
            }
            return plantsMap;
        }
        return null;
    }
    
    public void LoadData(GameData data)
    {
        PlantValue = data.plantValue;
        BuildablesValue = data.buildableValue;
    }

    public void SaveData(ref GameData data)
    {
        data.plantValue = PlantValue;
        data.buildableValue = BuildablesValue;
    }
}
