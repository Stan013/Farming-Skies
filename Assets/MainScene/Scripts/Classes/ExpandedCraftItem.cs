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

    public TMP_Text balanceCost;
    public TMP_Text waterCost;
    public TMP_Text fertiliserCost;

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

    private Coroutine holdCoroutine = null;
    private float holdTime = 0f;
    private float holdThreshold = 1f;

    public void SetupExpandedItem(CraftItem item)
    {
        collapsedItem = item;
        expandedImage.sprite = collapsedItem.attachedItemCard.cardSprite;
        expandedName.text = collapsedItem.attachedItemCard.itemName;
        balanceCost.text = collapsedItem.attachedItemCard.cardCraftResources[0].ToString() + "₴";
        waterCost.text = collapsedItem.attachedItemCard.cardCraftResources[1].ToString() + "₴";
        fertiliserCost.text = collapsedItem.attachedItemCard.cardCraftResources[2].ToString() + "₴";
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

        craftAmount = 0;
        collapsedItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }

    public void SetMin()
    {
        craftAmount = 0;
        craftAmountInput.text = "0";
    }

    public void DecreaseAmount()
    {
        int amount = craftAmount;
        amount = Mathf.Max(0, amount - 1);
        craftAmountInput.text = amount.ToString();
    }

    public void IncreaseAmount()
    {
        int amount = craftAmount;
        amount = Mathf.Min(collapsedItem.maxCraftAmount, amount + 1);
        craftAmountInput.text = amount.ToString();
    }

    public void SetMax()
    {
        craftAmount = collapsedItem.maxCraftAmount;
        craftAmountInput.text = collapsedItem.maxCraftAmount.ToString();
    }

    public void CheckValidCraftAmount(string input)
    {
        collapsedItem.CalculateMaxCraftableAmount();
        if (int.TryParse(input, out int value))
        {
            if (value > collapsedItem.maxCraftAmount)
            {
                craftAmount = collapsedItem.maxCraftAmount;
                craftAmountInput.text = collapsedItem.maxCraftAmount.ToString();
                craftButtonBackground.sprite = validCraft;
                canCraft = true;
            }
            else
            {
                if (value <= 0)
                {
                    craftAmountInput.text = "0";
                    craftButtonBackground.sprite = invalidCraft;
                    canCraft = false;
                }
                else
                {
                    craftAmount = value;
                    craftAmountInput.text = value.ToString();
                    craftButtonBackground.sprite = validCraft;
                    canCraft = true;
                }
            }
        }
        else
        {
            craftAmountInput.text = "0";
            craftButtonBackground.sprite = invalidCraft;
            canCraft = false;
        }
        balanceCost.text = (collapsedItem.attachedItemCard.cardCraftResources[0] * craftAmount).ToString();
        waterCost.text = (collapsedItem.attachedItemCard.cardCraftResources[1] * craftAmount).ToString();
        fertiliserCost.text = (collapsedItem.attachedItemCard.cardCraftResources[2] * craftAmount).ToString();
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
        GameManager.UM.Balance -= collapsedItem.attachedItemCard.cardCraftResources[0];
        GameManager.UM.Water -= collapsedItem.attachedItemCard.cardCraftResources[1];
        GameManager.UM.Fertiliser -= collapsedItem.attachedItemCard.cardCraftResources[2];
        GameManager.DM.AddCardToDeck(collapsedItem.attachedItemCard.cardId);
        CheckValidCraftAmount("0");
    }
}