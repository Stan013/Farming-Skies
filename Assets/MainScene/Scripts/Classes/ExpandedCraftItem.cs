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
            _craftAmount = value;
            CheckValidCraftAmount(_craftAmount.ToString());
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

        balanceCostText.SetText(collapsedItem.attachedItemCard.cardCraftResources[0].ToString() + " ₴");
        waterCostText.SetText(collapsedItem.attachedItemCard.cardCraftResources[1].ToString() + " L");
        fertiliserCostText.SetText(collapsedItem.attachedItemCard.cardCraftResources[2].ToString() + " L");
        plantCostText.SetText(collapsedItem.attachedItemCard.cardCraftResources[3].ToString() + " X");
        CheckValidCraftAmount("0");
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
    }

    public void SetMin()
    {
        CraftAmount = 0;
        craftAmountInput.text = "0";
    }

    public void DecreaseAmount()
    {
        int amount = CraftAmount;
        amount = Mathf.Max(0, amount - 1);
        craftAmountInput.text = amount.ToString();
    }

    public void IncreaseAmount()
    {
        int amount = CraftAmount;
        amount = Mathf.Min(collapsedItem.maxCraftAmount, amount + 1);
        craftAmountInput.text = amount.ToString();
    }

    public void SetMax()
    {
        CraftAmount = collapsedItem.maxCraftAmount;
        craftAmountInput.text = collapsedItem.maxCraftAmount.ToString();
    }

    public void CheckValidCraftAmount(string input)
    {
        collapsedItem.CalculateMaxCraftableAmount();

        if (int.TryParse(input, out int value) && value > 0)
        {
            if (value != CraftAmount)
            {
                if (value > collapsedItem.maxCraftAmount)
                {
                    CraftAmount = collapsedItem.maxCraftAmount;
                }
                else
                {
                    CraftAmount = value;
                }

                craftButtonBackground.sprite = validCraft;
                canCraft = true;
                balanceCostText.SetText((collapsedItem.attachedItemCard.cardCraftResources[0] * CraftAmount).ToString() + " ₴");
                waterCostText.SetText((collapsedItem.attachedItemCard.cardCraftResources[1] * CraftAmount).ToString() + " L");
                fertiliserCostText.SetText((collapsedItem.attachedItemCard.cardCraftResources[2] * CraftAmount).ToString() + " L");
                plantCostText.SetText((collapsedItem.attachedItemCard.cardCraftResources[3] * CraftAmount).ToString() + " X");
            }
        }
        else
        {
            if (value != CraftAmount)
            {
                CraftAmount = 0;
                craftButtonBackground.sprite = invalidCraft;
                canCraft = false;
            }
        }

        craftAmountInput.text = CraftAmount.ToString();
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
        CheckValidCraftAmount("0");
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