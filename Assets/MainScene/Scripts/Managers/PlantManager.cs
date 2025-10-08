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

    private float _structureValue;
    public float StructureValue
    {
        get => _structureValue;
        set
        {
            _structureValue = value;
            GameManager.EM.UpdateFarmValue();
        }
    }
    public float oldStructureValue;
    public float structureValueChange;

    public void Harvest()
    {
        foreach (Island island in GameManager.ISM.boughtIslands)
        {
            foreach (Plant plant in island.smallPlantsOnIsland.Concat(island.mediumPlantsOnIsland).Concat(island.largePlantsOnIsland))
            {
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
            case "Structure":
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
        StructureValue = data.buildableValue;
    }

    public void SaveData(ref GameData data)
    {
        data.plantValue = PlantValue;
        data.buildableValue = StructureValue;
    }
}
