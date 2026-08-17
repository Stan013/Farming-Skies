using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Island;

public class Structure : MonoBehaviour
{
    public Drop drop;
    public int dropAmount;
    public Island attachedIsland;
    public Card attachedCard;
    public InventoryItem attachedInventoryItem;

    public Sprite structureIcon;
    public StructureData structureData;
    public int structureAge;
    public int structureLifespan;
    public int structureTax;

    public void GiveDrop(Transform plot)
    {
        Drop stuctureDrop = Instantiate(drop, Vector3.zero, Quaternion.identity);
        stuctureDrop.transform.localScale = new Vector3(stuctureDrop.transform.localScale.x, stuctureDrop.transform.localScale.y, stuctureDrop.transform.localScale.z);
        stuctureDrop.transform.localPosition = new Vector3(plot.position.x, 5f, plot.position.z);
        stuctureDrop.transform.localRotation = Quaternion.identity;
        stuctureDrop.AddDropToInventory(attachedInventoryItem);   
    } 

    public StructureData SaveStructureData()
    {
        structureData = new StructureData(attachedCard.cardId, attachedIsland.islandID, transform.parent.name, attachedCard.cardType);
        return structureData;
    }
}
