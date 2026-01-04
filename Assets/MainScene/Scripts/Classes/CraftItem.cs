using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftItem : MonoBehaviour
{
    public TMP_Text itemNameText;
    public Image itemImage;
    public int itemIndex;
    public Card attachedItemCard;
    public Button expandButton;

    public Button craftButton;
    public TMP_InputField craftAmountInput;
    public Image craftInputBackground;
    public Sprite invalidCraft;
    public Sprite validCraft;
    public Sprite ongoingCraft;

    public int craftAmount;
    public bool canCraft;
    public int maxCraftAmount;

    private Coroutine holdCoroutine = null;
    private float holdTime = 0f;
    private float holdThreshold = 1f;

    public void SetCraftItem(Card itemCard)
    {
        if (itemCard != null)
        {
            attachedItemCard = itemCard;
            itemNameText.text = attachedItemCard.itemName;
            itemImage.sprite = attachedItemCard.cardSprite;
            itemIndex = GameManager.INM.itemsInInventory.Count;
        }
    }

    public void ResetCraftAmount()
    {
        foreach(CraftItem craftItem in GameManager.CRM.itemsInCrafting)
        {
            if(craftItem.gameObject.activeSelf)
            {
                craftItem.craftAmount = 0;
                craftItem.craftAmountInput.text = "";
            }
        }
    }

    public void CheckValidCraft() 
    {
        int input;
        if(craftAmountInput.text == "")
        {
            input = 0;
        }
        else
        {
            input = int.Parse(craftAmountInput.text);
        }

        if (input <= 0) 
        {
            craftAmount = 0;
            craftInputBackground.sprite = invalidCraft; 
            canCraft = false;
            return;
        }
        else 
        {
            CalculateMaxCraftableAmount();
            if (maxCraftAmount <= 0)
            {
                craftAmount = 0;
                craftInputBackground.sprite = invalidCraft;
                canCraft = false;
            }
            else
            {
                if(input > maxCraftAmount)
                {
                    craftAmount = maxCraftAmount;
                }
                else
                {
                    craftAmount = input;
                }

                craftInputBackground.sprite = validCraft;
                canCraft = true;
            }
        } 
        
        craftAmountInput.text = craftAmount.ToString(); 
    }

    public void CalculateMaxCraftableAmount()
    {
        List<int> validMaxValues = new List<int>();
        if (attachedItemCard.cardType != "Structure" && attachedItemCard.cardType != "Utilities")
        {
            if (attachedItemCard.cardCraftResources[3] > 0f)
            {
                validMaxValues.Add(Mathf.FloorToInt(attachedItemCard.inventoryItem.ItemQuantity / attachedItemCard.cardCraftResources[3]));
            }
        }
        else
        {
            if (attachedItemCard.cardCraftResources[0] > 0f)
            {
                validMaxValues.Add(Mathf.FloorToInt(GameManager.UM.Balance / attachedItemCard.cardCraftResources[0]));
            }
        }


        if (attachedItemCard.cardCraftResources[1] > 0f)
        {
            validMaxValues.Add(GameManager.UM.Water / attachedItemCard.cardCraftResources[1]);
        }
        if (attachedItemCard.cardCraftResources[2] > 0f)
        {
            validMaxValues.Add(GameManager.UM.Fertiliser / attachedItemCard.cardCraftResources[2]);
        }

        maxCraftAmount = (validMaxValues.Count > 0) ? Mathf.Min(validMaxValues.ToArray()) : 0;
    }

    public void OnCraftButtonPress()
    {
        if (canCraft)
        {
            if (holdCoroutine == null)
            {
                holdCoroutine = StartCoroutine(HandleCraftHold());
            }
            craftInputBackground.sprite = ongoingCraft;
        }
    }

    public void OnCraftButtonRelease()
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
        ResetCraftAmount();
    }

    private IEnumerator HandleCraftHold()
    {
        holdTime = 0f;
        while (holdTime < holdThreshold)
        {
            holdTime += Time.deltaTime;
            yield return null;
        }

        QuickCraft();
    }

    private void QuickCraft()
    {
        holdCoroutine = null;
        for (int i = 0; i < craftAmount; i++)
        {
            if (attachedItemCard.cardType != "Structure" && attachedItemCard.cardType != "Utilities")
            {
                attachedItemCard.inventoryItem.ItemQuantity -= attachedItemCard.cardCraftResources[3];
            }
            else
            {
                GameManager.UM.Balance -= attachedItemCard.cardCraftResources[0];
            }
            
            GameManager.UM.Water -= attachedItemCard.cardCraftResources[1];
            GameManager.UM.Fertiliser -= attachedItemCard.cardCraftResources[2];
            GameManager.DM.AddCardToDeck(attachedItemCard.cardId);
        }

        ResetCraftAmount();
    }

    public void ExpandCraftItem()
    {
        if(GameManager.CRM.expandedCraftItem.gameObject.activeSelf)
        {
            GameManager.CRM.expandedCraftItem.CollapseCraftItem();
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject fillerItem = Instantiate(GameManager.INM.fillerItem, transform.parent);
            fillerItem.transform.SetSiblingIndex(1);
        }

        GameManager.CRM.expandedCraftItem.SetupExpandedItem(this);
        GameManager.CRM.expandedCraftItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        GameManager.CRM.craftScroll.verticalNormalizedPosition = 1f;
    }
}
