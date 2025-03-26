using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    [Header("Drop variables")]
    public float dropChance;
    private System.Random random = new System.Random();

    public void Harvest()
    {
        foreach (Island island in GameManager.ISM.boughtIslands)
        {
            foreach (Plant plant in island.itemsOnIsland)
            {
                if (plant.nutrientsUsages[0] <= island.nutrientsAvailable[0])
                {
                    UpdatePlantYield(island, plant);
                    StartCoroutine(SpawnDropsWithInterval(plant));
                }
                else
                {
                    island.SetIslandState(Island.IslandState.Cultivated);
                }
            }
        }
    }

    public void UpdatePlantYield(Island island, Plant plant)
    {
        plant.yield = plant.baseYield;
        for (int i = 0; i < island.nutrientsAvailable.Count; i++)
        {
            if (i != 0)
            {
                if (island.nutrientsAvailable[i] >= plant.nutrientsUsages[i])
                {
                    if (random.NextDouble() <= dropChance)
                    {
                        plant.yield += plant.baseYield / 6;
                    }
                }
                else
                {
                    if (random.NextDouble() <= dropChance)
                    {
                        plant.yield -= plant.baseYield / 6;
                    }
                }
            }
            island.nutrientsAvailable[i] -= plant.nutrientsUsages[i];
        }
        island.UpdateNutrientsRequired();
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
