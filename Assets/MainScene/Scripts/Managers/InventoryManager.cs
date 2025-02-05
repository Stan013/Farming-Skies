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
            inventoryItem.SetItemQuantity();
        }
    }

    public void AddItemToInventory(InventoryItem inventoryItem)
    {
        inventoryItem.transform.SetParent(inventoryContentArea.transform, false);
        itemsInInventory.Add(inventoryItem);
    }

    public void CloseWindow()
    {
        if(GameManager.TTM.tutorialCount == 10)
        {
            GameManager.UM.openInventoryButton.GetComponent<Image>().color = Color.white;
            GameManager.TTM.QuestCompleted = true;
        }
        inventoryWindow.SetActive(false);
    }
}
