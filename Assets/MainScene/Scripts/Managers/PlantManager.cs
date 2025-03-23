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
                    CalculateDropAmount(island, plant);
                    plant.GiveDrop(plant.transform.parent.GetComponent<Transform>()); //add based on yield
                }
                else
                {
                    island.SetIslandState(Island.IslandState.Cultivated);
                }
            }
        }
    }

    public void CalculateDropAmount(Island island, Plant plant)
    {
        int baseYield = plant.yield;
        for(int i = 0; i < island.nutrientsAvailable.Count; i++)
        {
            if(i != 0)
            {
                if (island.nutrientsAvailable[i] >= plant.nutrientsUsages[i])
                {
                    if (random.NextDouble() <= dropChance)
                    {
                        switch (baseYield)
                        {
                            case 6:
                                plant.yield++;
                                break;
                            case 12:
                                plant.yield += 2;
                                break;
                            case 18:
                                plant.yield += 3;
                                break;
                            case 24:
                                plant.yield += 4;
                                break;
                        }
                    }
                }
                else
                {
                    if (random.NextDouble() <= dropChance)
                    {
                        switch (baseYield)
                        {
                            case 6:
                                plant.yield--;
                                break;
                            case 12:
                                plant.yield -= 2;
                                break;
                            case 18:
                                plant.yield -= 3;
                                break;
                            case 24:
                                plant.yield -= 4;
                                break;
                        }
                    }
                }
            }
            island.nutrientsAvailable[i] -= plant.nutrientsUsages[i];
        }
        island.UpdateNutrientsRequired();
    }
}
