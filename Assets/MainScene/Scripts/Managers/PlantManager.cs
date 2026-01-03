using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour, IDataPersistence
{
    [Header("Drop variables")]
    public float dropChance;
    public float plantValue;
    public float plantValueChange;
    public float structureValue;
    public float structureValueChange;

    public void Harvest()
    {
        foreach (Island island in GameManager.ISM.boughtIslands)
        {
            foreach (Plant plant in island.smallPlantsOnIsland.Concat(island.mediumPlantsOnIsland).Concat(island.largePlantsOnIsland))
            {
                if (plant.nutrientsUsages[0] <= island.nutrientsAvailable[0])
                {
                    plant.plantAge += 1;
                    plant.UpdatePlantYield();
                    StartCoroutine(SpawnDropsWithInterval(plant));
                }
                else
                {
                    plant.driedOut += 1;
                    island.nutrientsAvailable[0] = 0;
                    island.nutrientsAvailable[1] -= plant.nutrientsUsages[1];
                    island.nutrientsAvailable[2] -= plant.nutrientsUsages[2];
                    island.nutrientsAvailable[3] -= plant.nutrientsUsages[3];
                    island.SetIslandMaterial(Island.IslandState.Cultivated, island.cultivatedMatTop, island.sowedMatBot);
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
        plantValueChange = data.plantValue;
        structureValueChange = data.buildableValue;
    }

    public void SaveData(ref GameData data)
    {
        data.plantValue = plantValueChange;
        data.buildableValue = structureValueChange;
    }
}
