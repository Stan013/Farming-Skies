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

    public void CheckValidCraftAmount(string input)
    {
        CalculateMaxCraftableAmount();

        if (int.TryParse(input, out int value))
        {
            if (value <= 0 || value > maxCraftAmount)
            {
                craftAmountInput.text = Mathf.Clamp(value, 0, maxCraftAmount).ToString();
                craftInputBackground.sprite = invalidCraft;
                canCraft = false;
            }
            else
            {
                craftAmount = value;
                craftInputBackground.sprite = validCraft;
                canCraft = true;
            }
        }
        else
        {
            craftAmountInput.text = "0";
            craftInputBackground.sprite = invalidCraft;
            canCraft = false;
        }
    }

    public void CalculateMaxCraftableAmount()
    {
        List<int> validMaxValues = new List<int>();
        if (attachedItemCard.cardCraftResources[0] > 0f)
        {
            validMaxValues.Add(Mathf.FloorToInt(GameManager.UM.balance / attachedItemCard.cardCraftResources[0]));
        }
        if (attachedItemCard.cardCraftResources[1] > 0f)
        {
            validMaxValues.Add(GameManager.UM.water / attachedItemCard.cardCraftResources[1]);
        }
        if (attachedItemCard.cardCraftResources[2] > 0f)
        {
            validMaxValues.Add(GameManager.UM.fertiliser / attachedItemCard.cardCraftResources[2]);
        }
        if (attachedItemCard.cardType != "Utilities" && attachedItemCard.itemQuantity > 0)
        {
            validMaxValues.Add(attachedItemCard.itemQuantity / attachedItemCard.cardDropsRequired);
        }

        maxCraftAmount = (validMaxValues.Count > 0) ? Mathf.Min(validMaxValues.ToArray()) : 0;
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
        GameManager.UM.balance -= attachedItemCard.cardCraftResources[0];
        GameManager.UM.water -= attachedItemCard.cardCraftResources[1];
        GameManager.UM.fertiliser -= attachedItemCard.cardCraftResources[2];
        GameManager.DM.AddCardToDeck(attachedItemCard.cardId);
        CheckValidCraftAmount("0");
    }

    public void ExpandCraftItem()
    {
        int itemIndex = transform.GetSiblingIndex();
        int rowStartIndex = (itemIndex / 4) * 4;

        for (int i = 0; i < 3; i++)
        {
            Instantiate(GameManager.INM.fillerItem, transform.parent)
                .transform.SetSiblingIndex(rowStartIndex + 1);
        }

        GameManager.CRM.expandedCraftItem.SetupExpandedItem(this);
        GameManager.CRM.expandedCraftItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }
}
