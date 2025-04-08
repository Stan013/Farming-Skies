using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    [Header("Inventory lists")]
    public List<InventoryItem> itemsInInventory;

    [Header("Inventory variables")]
    public GameObject inventoryContentArea;
    public InventoryItem inventoryItemTemplate;
    public GameObject fillerItem;
    public ExpandedInventoryItem expandedInventoryItem;
    public ScrollRect inventoryScroll;
    public string inventoryTab;
    public Button closeButton;

    public void UnlockInventoryItem(Card itemCard, Plant plant)
    {
        InventoryItem existingInventoryItem = GameManager.INM.itemsInInventory.FirstOrDefault(i => i.attachedItemCard.itemName == itemCard.itemName);
        if (existingInventoryItem == null)
        {
            InventoryItem inventoryItem = Instantiate(inventoryItemTemplate, Vector3.zero, Quaternion.identity, inventoryContentArea.transform);
            inventoryItem.SetInventoryItem(GameManager.CM.FindCardByID(itemCard.cardId));
            inventoryItem.transform.localPosition = new Vector3(inventoryItem.transform.localPosition.x, inventoryItem.transform.localPosition.y, 0);
            inventoryItem.transform.localRotation = Quaternion.identity;
            itemsInInventory.Add(inventoryItem);
            itemsInInventory.Sort((a, b) => a.attachedItemCard.itemName.CompareTo(b.attachedItemCard.itemName));
            for (int i = 0; i < itemsInInventory.Count; i++)
            {
                itemsInInventory[i].transform.SetSiblingIndex(i + 1);
            }
            plant.attachedInventoryItem = inventoryItem;
        }
    }

    public void FilterItemsInInventory(string filter)
    {
        inventoryTab = filter;
        if (expandedInventoryItem.gameObject.activeSelf)
        {
            expandedInventoryItem.CollapseInventoryItem();
        }
        if (filter == "Default")
        {
            foreach (InventoryItem inventoryItem in itemsInInventory)
            {
                inventoryItem.gameObject.SetActive(true);
            }
        }
        else
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

    public InventoryItem FindInventoryItemByID(string id)
    {
        return itemsInInventory.Find(item => item.attachedItemCard.cardId == id);
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < data.itemQuantities.Count; i++)
        {
            itemsInInventory[i].ItemQuantity = data.itemQuantities[i];
        }
    }

    public void SaveData(ref GameData data)
    {
        data.itemQuantities.Clear();
        foreach (InventoryItem item in itemsInInventory)
        {
            data.itemQuantities.Add(item.ItemQuantity);
        }
    }
}
