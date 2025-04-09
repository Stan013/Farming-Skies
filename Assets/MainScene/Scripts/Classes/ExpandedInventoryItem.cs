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
    public TMP_Text predictedYieldText;
    public TMP_Text predictedProduction;

    public void SetupExpandedItem(InventoryItem item)
    {
        collapsedItem = item;
        expandedImage.sprite = collapsedItem.attachedItemCard.cardSprite;
        expandedName.text = collapsedItem.attachedItemCard.itemName;
        expandedQuantity.text = collapsedItem.ItemQuantity.ToString();
        UpdatePredictedYield();
    }

    public void CollapseInventoryItem()
    {
        if(this.gameObject.activeSelf)
        {
            int itemIndex = collapsedItem.transform.GetSiblingIndex();
            int rowStartIndex = (itemIndex / 4) * 4;

            for (int i = 0; i < 3; i++)
            {
                GameObject fillItem = GameManager.INM.inventoryContentArea.transform.GetChild(1 + i).gameObject;
                Destroy(fillItem);
            }

            collapsedItem.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }
    }

    public void UpdatePredictedYield()
    {
        if (collapsedItem.totalPredictedYield < collapsedItem.totalBaseYield)
        {
            predictedYieldText.text = collapsedItem.totalPredictedYield.ToString() + " - " + collapsedItem.totalBaseYield.ToString();
        }
        else
        {
            predictedYieldText.text = collapsedItem.totalBaseYield.ToString() + " - " + collapsedItem.totalPredictedYield.ToString();
        }
    }
}