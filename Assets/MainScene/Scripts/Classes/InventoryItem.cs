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

    public void SetInventoryItem(Card itemCard)
    {
        if(itemCard != null)
        {
            attachedItemCard = itemCard;
            attachedPlant = itemCard.GetComponent<CardDrag>().dragModel.GetComponent<Plant>();
            itemNameText.text = attachedItemCard.itemName;
            itemQuantityText.text = attachedItemCard.itemQuantity.ToString();
            itemImage.sprite = attachedItemCard.cardSprite;
            itemIndex = GameManager.INM.itemsInInventory.Count;
        }
    }

    public void ExpandInventoryItem()
    {
        if (GameManager.INM.expandedInventoryItem.gameObject.activeSelf)
        {
            GameManager.INM.expandedInventoryItem.CollapseInventoryItem();
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject fillerItem = Instantiate(GameManager.INM.fillerItem, transform.parent);
            fillerItem.transform.SetSiblingIndex(1);
        }

        GameManager.INM.expandedInventoryItem.SetupExpandedItem(this);
        GameManager.INM.expandedInventoryItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        GameManager.INM.inventoryScroll.verticalNormalizedPosition = 1f;
    }
}
 