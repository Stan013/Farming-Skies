using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Island;

public class Plant : MonoBehaviour
{
    public Drop drop;
    public int dropAmount;
    public Island attachedIsland;
    public InventoryItem attachedInventoryItem;
    public int baseYield;
    public int yield;
    public int predictedYield;
    public List<int> nutrientsUsages;
    public int buildableTaxCost;
    private System.Random random = new System.Random();
    public PlantData plantData;

    public void GiveDrop(Transform plot)
    {
        Drop plantDrop = Instantiate(drop, Vector3.zero, Quaternion.identity);
        plantDrop.transform.localScale = new Vector3(plantDrop.transform.localScale.x, plantDrop.transform.localScale.y, plantDrop.transform.localScale.z);
        plantDrop.transform.localPosition = new Vector3(plot.position.x, 5f, plot.position.z);
        plantDrop.transform.localRotation = Quaternion.identity;
        plantDrop.AddDropToInventory(attachedInventoryItem, this);   
    }

    public void UpdatePredictedYield()
    {
        predictedYield = baseYield;
        for (int i = 0; i < attachedIsland.nutrientsAvailable.Count; i++)
        {
            if (i != 0)
            {
                if (attachedIsland.nutrientsAvailable[i] >= nutrientsUsages[i])
                {
                    predictedYield = predictedYield + (baseYield / 6);
                }
                else
                {
                   predictedYield = predictedYield - (baseYield / 6);
                }
            }
        }
        attachedInventoryItem.totalBaseYield += baseYield;
        attachedInventoryItem.totalPredictedYield += predictedYield;
    }

    public void UpdatePlantYield()
    {
        yield = baseYield;
        for (int i = 0; i < attachedIsland.nutrientsAvailable.Count; i++)
        {
            if (i != 0)
            {
                if (attachedIsland.nutrientsAvailable[i] >= nutrientsUsages[i])
                {
                    if (random.NextDouble() <= GameManager.PM.dropChance)
                    {
                        yield += baseYield / 6;
                    }
                    attachedIsland.nutrientsAvailable[i] -= nutrientsUsages[i];
                }
                else
                {
                    if (random.NextDouble() <= GameManager.PM.dropChance)
                    {
                        yield -= baseYield / 6;
                    }
                }
            }
            else
            {
                attachedIsland.nutrientsAvailable[i] -= nutrientsUsages[i];
            }
        }
    }

    public PlantData SavePlantData()
    {
        if(attachedInventoryItem != null)
        {
            plantData = new PlantData(attachedInventoryItem.attachedItemCard.cardId, attachedIsland.islandID, transform.parent.name, attachedInventoryItem.attachedItemCard.cardType);
        }
        return plantData;
    }
}
