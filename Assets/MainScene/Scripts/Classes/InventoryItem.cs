using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    //Generate id for saving
    [SerializeField] private string id;
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemQuantityText;
    [SerializeField] private Image itemImage;
    public MarketItem marketItem;
    public int itemIndex;
    public string itemName;
    public Sprite itemSprite;

    public void SetItemQuantity()
    {
        itemQuantityText.SetText(marketItem.itemQuantity.ToString());
    }

    public void SetInventoryItem(Card itemCard)
    {
        if(itemCard != null)
        {
            itemCard.marketItem.itemQuantity = 1;
            itemName = itemCard.itemName;
            marketItem = itemCard.marketItem;
            itemNameText.SetText(itemName);
            itemQuantityText.SetText(marketItem.itemQuantity.ToString());
            itemImage.sprite = itemCard.cardSprite;
            itemIndex = GameManager.INM.itemsInInventory.Count;
        }
    }
}
 