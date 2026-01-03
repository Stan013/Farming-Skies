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

    public GameObject balanceCost;
    public GameObject plantCost;
    public GameObject waterCost;
    public GameObject fertiliserCost;

    public TMP_Text balanceCostText;
    public TMP_Text plantCostText;
    public TMP_Text waterCostText;
    public TMP_Text fertiliserCostText;

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

    public int CraftAmount
    {
        get => _craftAmount;
        set
        {
            _craftAmount = Mathf.Clamp(value, 0, collapsedItem.maxCraftAmount);
            UpdateCraftAmount();
        }
    }
    private int _craftAmount;
    public bool canCraft;

    private Coroutine holdCoroutine = null;
    private float holdTime = 0f;
    private float holdThreshold = 1f;

    public void SetupExpandedItem(CraftItem item)
    {
        collapsedItem = item;
        expandedImage.sprite = collapsedItem.attachedItemCard.cardSprite;
        expandedName.text = collapsedItem.attachedItemCard.itemName;

        if(item.attachedItemCard.cardType != "Structure" && item.attachedItemCard.cardType != "Utilities")
        {
            balanceCost.SetActive(false);
            plantCost.SetActive(true);
            plantCost.GetComponent<Image>().sprite = collapsedItem.attachedItemCard.cardCraftIcon;
        }
        else
        {
            balanceCost.SetActive(true);
            plantCost.SetActive(false);
        }

        CraftAmount = 0;
    }

    public void CollapseCraftItem()
    {
        if(this.gameObject.activeSelf)
        {
            int itemIndex = collapsedItem.transform.GetSiblingIndex();
            int rowStartIndex = (itemIndex / 4) * 4;

            for (int i = 0; i < 3; i++)
            {
                GameObject fillItem = GameManager.CRM.craftContentArea.transform.GetChild(1 + i).gameObject;
                Destroy(fillItem);
            }

            CraftAmount = 0;
            collapsedItem.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }

        if(collapsedItem != null)
        {
            collapsedItem.ResetCraftAmount();
        }
    }

    public void DecreaseAmount()
    {
        CraftAmount = Mathf.Max(0, CraftAmount - 1); 
    }

    public void IncreaseAmount()
    {
        collapsedItem.CalculateMaxCraftableAmount();
        CraftAmount = Mathf.Min(collapsedItem.maxCraftAmount, CraftAmount + 1);
    }

    public void SetMin()
    {
        CraftAmount = 0;
    }

    public void SetMax()
    {
        collapsedItem.CalculateMaxCraftableAmount();
        CraftAmount = collapsedItem.maxCraftAmount;
    }

    public void InputCraftAmount()
    {
        if (craftAmountInput.text == "")
        {
            craftAmountInput.text = "";
            return;
        }

        collapsedItem.CalculateMaxCraftableAmount();
        CraftAmount = int.Parse(craftAmountInput.text);
    }

    public void UpdateCraftAmount()
    {
        canCraft = CraftAmount > 0 && CraftAmount <= collapsedItem.maxCraftAmount;

        craftButtonBackground.sprite = canCraft ? validCraft : invalidCraft;
        craftAmountInput.text = CraftAmount.ToString();

        if(CraftAmount != 0)
        {
            balanceCostText.text = (collapsedItem.attachedItemCard.cardCraftResources[0] * CraftAmount).ToString() + " ₴";
            waterCostText.text = (collapsedItem.attachedItemCard.cardCraftResources[1] * CraftAmount).ToString() + " L";
            fertiliserCostText.text = (collapsedItem.attachedItemCard.cardCraftResources[2] * CraftAmount).ToString() + " L";
            plantCostText.text = (collapsedItem.attachedItemCard.cardCraftResources[3] * CraftAmount).ToString() + " X";
        }
        else
        {
            balanceCostText.SetText(collapsedItem.attachedItemCard.cardCraftResources[0].ToString() + " ₴");
            waterCostText.SetText(collapsedItem.attachedItemCard.cardCraftResources[1].ToString() + " L");
            fertiliserCostText.SetText(collapsedItem.attachedItemCard.cardCraftResources[2].ToString() + " L");
            plantCostText.SetText(collapsedItem.attachedItemCard.cardCraftResources[3].ToString() + " X");
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
            holdCoroutine = null;
        }
        UpdateCraftAmount();
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
        for (int i = 0; i < CraftAmount; i++)
        {
            GameManager.UM.Balance -= collapsedItem.attachedItemCard.cardCraftResources[0];
            GameManager.UM.Water -= collapsedItem.attachedItemCard.cardCraftResources[1];
            GameManager.UM.Fertiliser -= collapsedItem.attachedItemCard.cardCraftResources[2];
            GameManager.DM.AddCardToDeck(collapsedItem.attachedItemCard.cardId);
        }

        collapsedItem.CalculateMaxCraftableAmount();
        CraftAmount = 0;

        if (collapsedItem.attachedItemCard.cardCraftResources[3] > 0f)
        {
            collapsedItem.attachedItemCard.inventoryItem.ItemQuantity  -= collapsedItem.attachedItemCard.cardCraftResources[3];
        }
    }
}