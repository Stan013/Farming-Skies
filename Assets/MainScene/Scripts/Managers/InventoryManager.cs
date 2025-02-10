using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> itemsInInventory;
    public GameObject inventoryWindow;
    public GameObject inventoryContentArea;

    public void UpdateInventoryItems()
    {
        foreach (InventoryItem inventoryItem in itemsInInventory)
        {
            inventoryItem.itemQuantityText.SetText(inventoryItem.attachedItemCard.itemQuantity.ToString());
        }
    }

    public void AddItemToInventory(InventoryItem inventoryItem)
    {
        inventoryItem.transform.SetParent(inventoryContentArea.transform, false);
        itemsInInventory.Add(inventoryItem);
    }
}
