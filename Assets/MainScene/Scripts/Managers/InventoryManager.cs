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
    public GameObject fillerItem;
    public ExpandedInventoryItem expandedInventoryItem;
    public ScrollRect inventoryScroll;

    public void UnlockInventoryItem(Card itemCard)
    {
        InventoryItem inventoryItem = Instantiate(inventoryItemTemplate, Vector3.zero, Quaternion.identity);
        inventoryItem.SetInventoryItem(itemCard);
        inventoryItem.transform.localPosition = new Vector3(inventoryItem.transform.localPosition.x, inventoryItem.transform.localPosition.y, 0);
        inventoryItem.transform.localRotation = Quaternion.identity;
        AddItemToInventory(inventoryItem);
    }

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
            itemsInInventory[i].transform.SetSiblingIndex(i+1);
        }
    }

    public void FilterItemsInInventory(string filter)
    {
        foreach (InventoryItem inventoryItem in itemsInInventory)
        {
            if (inventoryItem.attachedItemCard.plantGroup != filter)
            {
                inventoryItem.gameObject.SetActive(false);
            }
            else
            {
                inventoryItem.gameObject.SetActive(true);
            }
        }
    }
}
