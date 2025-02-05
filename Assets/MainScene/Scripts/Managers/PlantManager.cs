using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    public void Harvest()
    {
        foreach (Island island in GameManager.ISM.boughtIslands)
        {
            foreach (Plant plant in island.itemsOnIsland)
            {
                if(!GameManager.TTM.tutorial)
                {
                    CalculateDropAmount(island, plant);
                }
                plant.GiveDrop(plant.transform.parent.GetComponent<Transform>());
                island.totalWater += plant.water;
                island.totalNitrogen += plant.nitrogen;
                island.totalPhosphorus += plant.phosphorus;
                island.totalPotassium += plant.potassium;
                island.totalMagnesium += plant.magnesium;
                island.totalSulfur += plant.sulfur;
                island.totalCalcium += plant.calcium;
            }
        }
    }

    public void CalculateDropAmount(Island island, Plant plant)
    {
        if(island.nitrogen >= plant.nitrogen)
        {
            plant.yield++;
        }
        //if(island.nitrogen >= plant)
    }
}
