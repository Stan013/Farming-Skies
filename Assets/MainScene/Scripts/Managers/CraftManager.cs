using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftManager : MonoBehaviour
{
    [Header("Craft lists")]
    public List<Card> craftableCards;
    public List<CraftItem> itemsInCrafting;

    [Header("Craft variables")]
    public GameObject craftContentArea;
    public CraftItem craftItemTemplate;
    public ExpandedCraftItem expandedCraftItem;
    public ScrollRect craftScroll;
    public string craftingTab;

    public void UnlockCraftItem(Card attachedCard)
    {
        CraftItem craftItem = Instantiate(craftItemTemplate, Vector3.zero, Quaternion.identity, craftContentArea.transform);
        craftItem.SetCraftItem(attachedCard);
        craftItem.transform.localPosition = new Vector3(craftItem.transform.localPosition.x, craftItem.transform.localPosition.y, 0);
        craftItem.transform.localRotation = Quaternion.identity;
        itemsInCrafting.Add(craftItem);
        itemsInCrafting.Sort((a, b) => a.attachedItemCard.itemName.CompareTo(b.attachedItemCard.itemName));
        for (int i = 0; i < itemsInCrafting.Count; i++)
        {
            itemsInCrafting[i].transform.SetSiblingIndex(i + 1);
        }
    }

    public void FilterItemsInCrafting(string filter)
    {
        craftingTab = filter;
        if (filter == "Default")
        {
            foreach (CraftItem craftItem in itemsInCrafting)
            {
                craftItem.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (CraftItem craftItem in itemsInCrafting)
            {
                if (craftItem.attachedItemCard.cardType != filter)
                {
                    craftItem.gameObject.SetActive(false);
                }
                else
                {
                    craftItem.gameObject.SetActive(true);
                }
            }
        }
        if (expandedCraftItem.gameObject.activeSelf)
        {
            expandedCraftItem.CollapseCraftItem();
        }
    }

    public CraftItem FindCraftItemByID(string id)
    {
        return itemsInCrafting.Find(craftItem => craftItem.attachedItemCard.cardId == id);
    }
}
