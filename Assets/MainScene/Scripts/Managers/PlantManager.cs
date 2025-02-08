using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    private System.Random random = new System.Random();

    public void Harvest()
    {
        foreach (Island island in GameManager.ISM.boughtIslands)
        {
            foreach (Plant plant in island.itemsOnIsland)
            {
                if (GameManager.ISM.CheckIslandWater(plant))
                {
                    island.water -= plant.water;
                    plant.GiveDrop(plant.transform.parent.GetComponent<Transform>());
                    CalculateDropAmount(island, plant);
                }
                else
                {
                    island.ToggleState(Island.IslandState.Cultivated, Island.IslandState.Sowed);
                }
            }
            GameManager.ISM.CheckIslandNutrients(island);
            island.UpdateIslandStats();
        }
    }

    public void CalculateDropAmount(Island island, Plant plant)
    {
        if(island.nitrogen >= plant.nitrogen)
        {
            if (random.NextDouble() <= 0.75)
            {
                plant.yield++;
            }
        }
        else
        {
            if (random.NextDouble() <= 0.75)
            {
                plant.yield--;
            }
        }
        if (island.phosphorus >= plant.phosphorus)
        {
            if (random.NextDouble() <= 0.75)
            {
                plant.yield++;
            }
        }
        else
        {
            if (random.NextDouble() <= 0.75)
            {
                plant.yield--;
            }
        }
        if (island.potassium >= plant.potassium)
        {
            if (random.NextDouble() <= 0.75)
            {
                plant.yield++;
            }
        }
        else
        {
            if (random.NextDouble() <= 0.75)
            {
                plant.yield--;
            }
        }
        island.nitrogen -= plant.nitrogen;
        island.phosphorus -= plant.phosphorus;
        island.potassium -= plant.potassium;
    }
}
