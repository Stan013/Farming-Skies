using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public Drop drop;
    public int dropAmount;
    public string plantCardID;
    public bool addedDropToInventory;

    public Island attachedIsland;
    public int baseYield;
    public int yield;
    public int maxYield;
    public List<int> nutrientsUsages;
    public int buildableTaxCost;

    public void GiveDrop(Transform plot)
    {
        Drop plantDrop = Instantiate(drop, Vector3.zero, Quaternion.identity);
        plantDrop.transform.localScale = new Vector3(plantDrop.transform.localScale.x, plantDrop.transform.localScale.y, plantDrop.transform.localScale.z);
        plantDrop.transform.localPosition = new Vector3(plot.position.x, 5f, plot.position.z);
        plantDrop.transform.localRotation = Quaternion.identity;
        if(!addedDropToInventory)
        {
            Card attachedCard = GameManager.CM.FindCardByID(plantCardID);
            attachedCard.itemQuantity = yield;
            plantDrop.AddDropToInventory(attachedCard, this);
        }
    }
}
