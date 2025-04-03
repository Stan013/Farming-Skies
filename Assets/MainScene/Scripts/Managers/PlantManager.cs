using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    [Header("Drop variables")]
    public float dropChance;

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
                    island.SetIslandState(Island.IslandState.Cultivated);
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
}
