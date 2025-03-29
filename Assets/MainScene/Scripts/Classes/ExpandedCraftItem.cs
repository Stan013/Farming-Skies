using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpandedCraftItem : MonoBehaviour
{
    public CraftItem collapsedItem;
    public Image expandedImage;
    public TMP_Text expandedName;

    public Button craftButton;
    public Image craftButtonBackground;
    public Sprite invalidCraft;
    public Sprite validCraft;
    public Sprite ongoingCraft;

    public Button minButton;
    public Button minusButton;
    public TMP_InputField craftAmountInput;
    public Button plusButton;
    public Button maxButton;

    public int craftAmount;
    public bool canCraft;
    public int maxCraftAmount;

    private Coroutine holdCoroutine = null;
    private float holdTime = 0f;
    private float holdThreshold = 1f;

    public void SetupExpandedItem(CraftItem item)
    {
        collapsedItem = item;
        expandedImage.sprite = collapsedItem.attachedItemCard.cardSprite;
        expandedName.text = collapsedItem.attachedItemCard.itemName;
        CheckValidCraftAmount("0");
    }

    public void CollapseCraftItem()
    {
        int itemIndex = collapsedItem.transform.GetSiblingIndex();
        int rowStartIndex = (itemIndex / 4) * 4;

        for (int i = 0; i < 3; i++)
        {
            GameObject fillItem = GameManager.CRM.craftContentArea.transform.GetChild(1+i).gameObject;
            Destroy(fillItem);
        }

        collapsedItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }

    public void SetMin()
    {
        craftAmountInput.text = "0";
    }

    public void DecreaseAmount()
    {
        int amount = craftAmount;
        amount = Mathf.Max(1, amount - 1);
        craftAmountInput.text = amount.ToString();
    }

    public void IncreaseAmount()
    {
        int amount = craftAmount;
        amount = Mathf.Min(maxCraftAmount, amount + 1);
        craftAmountInput.text = amount.ToString();
    }

    public void SetMax()
    {
        craftAmountInput.text = maxCraftAmount.ToString();
    }

    public void CheckValidCraftAmount(string input)
    {
        collapsedItem.CalculateMaxCraftableAmount();

        if (int.TryParse(input, out int value))
        {
            if (value <= 0 || value > maxCraftAmount)
            {
                craftAmountInput.text = Mathf.Clamp(value, 0, maxCraftAmount).ToString();
                craftButtonBackground.sprite = invalidCraft;
                canCraft = false;
            }
            else
            {
                collapsedItem.craftAmount = value;
                craftButtonBackground.sprite = validCraft;
                canCraft = true;
            }
        }
        else
        {
            craftAmountInput.text = "0";
            craftButtonBackground.sprite = invalidCraft;
            canCraft = false;
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
            craftButtonBackground.sprite = ongoingCraft;
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

        CraftCard();
    }

    private void CraftCard()
    {
        holdCoroutine = null;
        GameManager.UM.balance -= collapsedItem.attachedItemCard.cardCraftResources[0];
        GameManager.UM.water -= collapsedItem.attachedItemCard.cardCraftResources[1];
        GameManager.UM.fertiliser -= collapsedItem.attachedItemCard.cardCraftResources[2];
        GameManager.DM.AddCardToDeck(collapsedItem.attachedItemCard.cardId);
        CheckValidCraftAmount("0");
    }
}