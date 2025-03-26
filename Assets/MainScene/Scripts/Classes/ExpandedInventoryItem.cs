using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpandedInventoryItem : MonoBehaviour
{
    public InventoryItem collapsedItem;
    public Image expandedImage;
    public TMP_Text expandedName;
    public TMP_Text expandedQuantity;
    public TMP_Text predictedYield;
    public TMP_Text predictedProduction;

    public void SetupExpandedItem(InventoryItem item)
    {
        collapsedItem = item;
        expandedImage.sprite = collapsedItem.attachedItemCard.cardSprite;
        expandedName.text = collapsedItem.attachedItemCard.itemName;
        expandedQuantity.text = collapsedItem.attachedItemCard.itemQuantity.ToString();
        predictedYield.text = collapsedItem.attachedPlant.baseYield + " - " + item.attachedPlant.maxYield;
    }

    public void CollapseInventoryItem()
    {
        int itemIndex = collapsedItem.transform.GetSiblingIndex();
        int rowStartIndex = (itemIndex / 4) * 4;

        for (int i = 0; i < 3; i++)
        {
            GameObject fillItem = GameManager.INM.inventoryContentArea.transform.GetChild(1+i).gameObject;
            Destroy(fillItem);
        }

        collapsedItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }
}