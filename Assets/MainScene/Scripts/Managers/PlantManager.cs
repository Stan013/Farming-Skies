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
