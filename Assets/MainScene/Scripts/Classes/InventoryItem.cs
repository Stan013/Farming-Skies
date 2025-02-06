using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text itemQuantityText;
    public Image itemImage;
    public int itemIndex;
    public Card attachedItemCard;

    public void SetInventoryItem(Card itemCard, int dropAmount)
    {
        if(itemCard != null)
        {
            attachedItemCard = itemCard;
            attachedItemCard.itemQuantity = dropAmount;
            itemNameText.SetText(attachedItemCard.itemName);
            itemQuantityText.SetText(attachedItemCard.itemQuantity.ToString());
            itemImage.sprite = attachedItemCard.cardSprite;
            itemIndex = GameManager.INM.itemsInInventory.Count;
        }
    }
}
 