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
                    island.Water -= plant.water;
                    plant.GiveDrop(plant.transform.parent.GetComponent<Transform>());
                    CalculateDropAmount(island, plant);
                }
                else
                {
                    island.ToggleState(Island.IslandState.Cultivated, Island.IslandState.Sowed);
                }
            }
        }
    }

    public void CalculateDropAmount(Island island, Plant plant)
    {
        if(island.Nitrogen >= plant.nitrogen)
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
        if (island.Phosphorus >= plant.phosphorus)
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
        if (island.Potassium >= plant.potassium)
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
        island.Nitrogen -= plant.nitrogen;
        island.Phosphorus -= plant.phosphorus;
        island.Potassium -= plant.potassium;
        island.CheckWarningIcon();
    }
}
