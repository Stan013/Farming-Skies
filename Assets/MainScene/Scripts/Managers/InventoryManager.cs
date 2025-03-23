using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory lists")]
    public List<InventoryItem> itemsInInventory;

    [Header("Inventory variables")]
    public GameObject inventoryContentArea;
    public InventoryItem inventoryItemTemplate;
    public GameObject fillerInventoryItem;
    public ExpandedInventoryItem expandedInventoryItem;

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
        itemsInInventory.Sort((a, b) => a.attachedItemCard.itemName.CompareTo(b.attachedItemCard.itemName));
        for (int i = 0; i < itemsInInventory.Count; i++)
        {
            itemsInInventory[i].transform.SetSiblingIndex(i);
        }
    }
}
