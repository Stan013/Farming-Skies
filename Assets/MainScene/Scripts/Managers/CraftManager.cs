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
    //public ExpandedInventoryItem expandedInventoryItem;
    public string craftingTab;

    public Button craftButton;
    public TMP_Text craftButtonText;
    public Sprite invalidCraft;
    public Sprite validCraft;
    public Sprite successCraft;
    public bool isCrafting;
    public int biggestResourceCost;
    public bool craftSuccess;
    public float craftSpeed;

    public int cardCraftAmount;
    public int maxCraftableAmount;

    public void UnlockCraftItem(Card attachedCard)
    {
        CraftItem craftItem = Instantiate(craftItemTemplate, Vector3.zero, Quaternion.identity);
        craftItem.SetCraftItem(attachedCard);
        craftItem.transform.localPosition = new Vector3(craftItem.transform.localPosition.x, craftItem.transform.localPosition.y, 0);
        craftItem.transform.localRotation = Quaternion.identity;
        AddItemToCrafting(craftItem);
    }

    public void AddItemToCrafting(CraftItem craftItem)
    {
        craftItem.transform.SetParent(craftContentArea.transform, false);
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
        foreach(CraftItem craftItem in itemsInCrafting)
        {
            if(craftItem.attachedItemCard.cardType != filter)
            {
                craftItem.gameObject.SetActive(false);
            }
            else
            {
                craftItem.gameObject.SetActive(true);
            }
        }
    }

    public CraftItem GetCraftItemByID(string id)
    {
        return itemsInCrafting.Find(craftItem => craftItem.attachedItemCard.cardId == id);
    }
}
