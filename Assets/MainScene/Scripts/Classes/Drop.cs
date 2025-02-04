using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public string dropType;
    public string plantCardID;
    private float moveSpeed = 5f;
    private float rotationSpeed = 500f;

    public void AddDropToInventoryMarket(Drop plantDrop)
    {
        if (plantDrop != null)
        {
            switch (dropType)
            {
                case "Machine":
                    if (plantDrop.name == "WaterDrop")
                    {
                        GameManager.UM.water += 1;
                    }
                    if (plantDrop.name == "FertilizerDrop")
                    {
                        GameManager.UM.fertilizer += 1;
                    }
                    break;
                case "Product":
                    break;
                default:
                    Card itemCard = GameManager.CM.FindCardById(plantDrop.plantCardID);
                    InventoryItem existingInventoryItem = GameManager.INM.itemsInInventory.FirstOrDefault(i => i.itemName == itemCard.itemName);
                    if (existingInventoryItem == null)
                    {
                        InventoryItem inventoryItem = Instantiate(itemCard.inventoryItem, Vector3.zero, Quaternion.identity);
                        inventoryItem.SetInventoryItem(itemCard);
                        GameManager.INM.AddItemToInventory(inventoryItem);
                        MarketItem matchingMarketItem = GameManager.MM.itemsInMarket.FirstOrDefault(m => m.itemName == inventoryItem.itemName);
                        if (matchingMarketItem != null)
                        {
                            inventoryItem.marketItem = matchingMarketItem;
                            matchingMarketItem.itemQuantity = 1;
                        }
                    }
                    else
                    {
                        MarketItem matchingMarketItem = GameManager.MM.itemsInMarket.FirstOrDefault(m => m.itemName == existingInventoryItem.itemName);
                        if (matchingMarketItem != null)
                        {
                            matchingMarketItem.itemQuantity += 1;
                        }
                    }
                    break;
            }
            MoveDrop(plantDrop);
        }
    }

    public void MoveDrop(Drop plantDrop)
    {
        StartCoroutine(MoveDropCoroutine(plantDrop));
    }

    private IEnumerator MoveDropCoroutine(Drop plantDrop)
    {
        Vector3 targetPosition = new Vector3(plantDrop.transform.position.x, 15f, plantDrop.transform.position.z);

        while (Vector3.Distance(plantDrop.transform.position, targetPosition) > 0.1f)
        {
            plantDrop.transform.position = Vector3.MoveTowards(plantDrop.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            plantDrop.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
            yield return null;
        }
        if (plantDrop.transform.position.y > 10f)
        {
            Destroy(plantDrop.gameObject);
        }
    }
}
