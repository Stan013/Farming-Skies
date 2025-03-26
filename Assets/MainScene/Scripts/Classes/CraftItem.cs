using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class CraftItem : MonoBehaviour
{
    public TMP_Text itemNameText;
    public Image itemImage;
    public int itemIndex;
    public Card attachedItemCard;
    public Button expandButton;

    public Button craftButton;
    public TMP_InputField craftAmountInput;
    public int craftAmount;
    public Image craftInputBackground;
    public Sprite invalidCraft;
    public Sprite validCraft;
    public Sprite ongoingCraft;
    public bool canCraft;

    private Coroutine holdCoroutine = null;
    private float holdTime = 0f;
    private float holdThreshold = 1f;

    public void CheckValidCraftAmount(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (value == 0)
            {
                craftAmountInput.text = "0";
                craftInputBackground.GetComponent<Image>().sprite = invalidCraft;
                canCraft = false;
            }
            else
            {
                craftAmount = value;
                craftInputBackground.GetComponent<Image>().sprite = validCraft;
                canCraft = true;
            }
        }
        else
        {
            craftAmountInput.text = "0";
            craftInputBackground.GetComponent<Image>().sprite = invalidCraft;
            canCraft = false;
        }
    }

    public void SetCraftItem(Card itemCard)
    {
        if (itemCard != null)
        {
            attachedItemCard = itemCard;
            itemNameText.text = attachedItemCard.itemName;
            itemImage.sprite = attachedItemCard.cardSprite;
            itemIndex = GameManager.INM.itemsInInventory.Count;
            CheckValidCraftAmount("0");
        }
    }

    public void OnCraftButtonPress()
    {
        if(canCraft)
        {
            if (holdCoroutine == null)
            {
                holdCoroutine = StartCoroutine(HandleCraftHold());
            }
            craftInputBackground.GetComponent<Image>().sprite = ongoingCraft;
        }
    }

    public void OnCraftButtonRelease()
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            CheckValidCraftAmount("0");
            holdCoroutine = null;
        }
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
        CheckValidCraftAmount("0");
        Debug.Log("Quick Crafting performed with amount: " + craftAmount);
    }

/*    public void ExpandInventoryItem()
    {
        int itemIndex = transform.GetSiblingIndex();
        int rowStartIndex = (itemIndex / 4) * 4;

        for (int i = 0; i < 3; i++)
        {
            Instantiate(GameManager.INM.fillerInventoryItem, transform.parent)
                .transform.SetSiblingIndex(rowStartIndex + 1);
        }

        GameManager.INM.expandedInventoryItem.SetupExpandedItem(this);
        GameManager.INM.expandedInventoryItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }*/
}
