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
    public Plant attachedPlant;
    public Button expandButton;

    public void SetInventoryItem(Card itemCard, int dropAmount)
    {
        if(itemCard != null)
        {
            attachedItemCard = itemCard;
            attachedPlant = itemCard.GetComponent<CardDrag>().dragModel.GetComponent<Plant>();
            attachedItemCard.itemQuantity = dropAmount;
            itemNameText.text = attachedItemCard.itemName;
            itemQuantityText.text = attachedItemCard.itemQuantity.ToString();
            itemImage.sprite = attachedItemCard.cardSprite;
            itemIndex = GameManager.INM.itemsInInventory.Count;
        }
    }

    public void ExpandInventoryItem()
    {
        int itemIndex = transform.GetSiblingIndex();
        int rowStartIndex = (itemIndex / 4) * 4;

        for (int i = 0; i < 3; i++)
        {
            Instantiate(GameManager.INM.fillerInventoryItem, transform.parent)
                .transform.SetSiblingIndex(rowStartIndex+1); 
        }

        GameManager.INM.expandedInventoryItem.SetupExpandedItem(this);
        GameManager.INM.expandedInventoryItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }
}
 