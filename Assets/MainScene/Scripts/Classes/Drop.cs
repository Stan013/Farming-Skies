using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public string dropType;
    public int dropAmount;
    public string plantCardID;
    private int moveSpeed = 5;
    private int rotationSpeed = 500;

    public void AddDropToInventoryMarket()
    {
        switch (dropType)
        {
            case "Buildable":
                if (name == "WaterDrop")
                {
                    GameManager.UM.water += 1;
                }
                if (name == "fertiliserDrop")
                {
                    GameManager.UM.fertiliser += 1;
                }
                break;
            case "Product":
                break;
            case "Plant":
                Card itemCard = GameManager.CM.FindCardByID(plantCardID);
                InventoryItem existingInventoryItem = GameManager.INM.itemsInInventory.FirstOrDefault(i => i.attachedItemCard.itemName == itemCard.itemName);
                if (existingInventoryItem == null)
                {
                    InventoryItem inventoryItem = Instantiate(GameManager.INM.inventoryItemTemplate, Vector3.zero, Quaternion.identity);
                    inventoryItem.SetInventoryItem(itemCard, dropAmount);
                    inventoryItem.transform.localPosition = new Vector3(inventoryItem.transform.localPosition.x, inventoryItem.transform.localPosition.y, 0);
                    inventoryItem.transform.localRotation = Quaternion.identity;
                    GameManager.INM.AddItemToInventory(inventoryItem);
                    GameManager.MM.UnlockMarketItem(itemCard);
                }
                else
                {
                    existingInventoryItem.attachedItemCard.itemQuantity += dropAmount;
                }
                break;
        }
        MoveDrop();
    }

    public void MoveDrop()
    {
        StartCoroutine(MoveDropCoroutine());
    }

    private IEnumerator MoveDropCoroutine()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, 15f, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
            yield return null;
        }
        if (transform.position.y > 10f)
        {
            Destroy(gameObject);
        }
    }
}
