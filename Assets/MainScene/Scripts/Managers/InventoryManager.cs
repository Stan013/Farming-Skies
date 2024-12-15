using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> m_ItemsInInventory;
    public List<InventoryItem> itemsInInventory { get { return m_ItemsInInventory; } }
    [SerializeField] private GameObject inventoryWindow;
    private float increaseX = 450f;

    public void UpdateInventoryItems()
    {
        foreach (InventoryItem inventoryItem in itemsInInventory)
        {
            inventoryItem.SetItemQuantity();
        }
    }

    public void AddItemToInventory(InventoryItem inventoryItem)
    {
        itemsInInventory.Add(inventoryItem);
        inventoryItem.transform.SetParent(inventoryWindow.transform);
        inventoryItem.transform.localRotation = Quaternion.identity;
        inventoryItem.transform.localScale = Vector3.one;
        if (inventoryItem.itemIndex <= 4)
        {
            inventoryItem.transform.localPosition = new Vector3(-675f + increaseX * (inventoryItem.itemIndex), 160f, 0);
        }
        else
        {
            inventoryItem.transform.localPosition = new Vector3(-675f + increaseX * (inventoryItem.itemIndex-4), -270f, 0);
        }
    }

    public void CloseWindow()
    {
        inventoryWindow.SetActive(false);
    }
}
