using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private Card m_ItemCard;
    [SerializeField] private string DropType;
    public Card itemCard { get { return m_ItemCard; } }
    private float moveSpeed = 5f;
    private Drop plantDrop;

    public void MoveToInventory()
    {
        plantDrop = GetComponent<Drop>();
        StartCoroutine(MoveDrop());
    }

    private IEnumerator MoveDrop()
    {
        if(plantDrop != null && itemCard != null)
        {
            switch (DropType)
            {
                case "Machine":
                    if(plantDrop.name == "WaterDrop")
                    {
                        GameManager.UM.water += 1;
                    }
                    break;
                case "Product":
                    break;
                default:
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
            Vector3 targetPosition = new Vector3(plantDrop.transform.position.x, 15f, plantDrop.transform.position.z);
            while (Vector3.Distance(plantDrop.transform.position, targetPosition) > 0.1f)
            {
                plantDrop.transform.position = Vector3.MoveTowards(plantDrop.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            if (plantDrop.transform.position.y > 10f)
            {
                Destroy(plantDrop.gameObject);
            }
        }
    }
}
