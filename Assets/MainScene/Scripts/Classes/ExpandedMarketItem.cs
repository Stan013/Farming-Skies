using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpandedMarketItem : MonoBehaviour
{
    public MarketItem collapsedItem;
    public Image expandedImage;
    public TMP_Text expandedName;
    public TMP_Text expandedQuantity;

    public Button sellButton;
    public Button buyButton;
    public string marketTransaction;
    public Image transactionInputBackground;
    public Sprite validTransaction;
    public Sprite ongoingTransaction;
    public Sprite invalidTransaction;
    public bool canTransaction;

    public Button minButton;
    public Button minusButton;
    public TMP_InputField transactionAmountInput;
    public int transactionAmount;
    public Button plusButton;
    public Button maxButton;

    public TMP_Text highestPriceText;
    public int highestPrice;
    public TMP_Text lowestPriceText;
    public int lowestPrice; 

    public TMP_Text expandedDemandAmount;
    public Image expandedDemandIcon;
    public TMP_Text expandedPrice;
    public Image expandedPriceIcon;
    public TMP_Text expandedSupplyAmount;
    public Image expandedSupplyIcon;

    private Coroutine holdCoroutine = null;
    private float holdTime = 0f;
    private float holdThreshold = 1f;

    public void SetupExpandedItem(MarketItem item)
    {
        collapsedItem = item;
        expandedImage.sprite = collapsedItem.attachedItemCard.cardSprite;
        expandedName.text = collapsedItem.attachedItemCard.itemName;
    }

    public void CollapseMarketItem()
    {
        int itemIndex = collapsedItem.transform.GetSiblingIndex();
        int rowStartIndex = (itemIndex / 4) * 4;

        for (int i = 0; i < 3; i++)
        {
            GameObject fillItem = GameManager.MM.marketContentArea.transform.GetChild(1+i).gameObject;
            Destroy(fillItem);
        }

        collapsedItem.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }

    public void SetMarketTransaction(string input)
    {
        marketTransaction = input;
    }

    public void SetMin()
    {
        transactionAmount = 0;
        transactionAmountInput.text = "0";
    }

    public void DecreaseAmount()
    {
        int amount = transactionAmount;
        amount = Mathf.Max(0, amount - 1);
        transactionAmountInput.text = amount.ToString();
    }

    public void IncreaseAmount()
    {
        int amount = transactionAmount;
        amount = Mathf.Min(collapsedItem.maxBuyAmount, amount + 1);
        transactionAmountInput.text = amount.ToString();
    }

    public void SetMax()
    {
        transactionAmount = collapsedItem.maxBuyAmount;
        transactionAmountInput.text = collapsedItem.maxBuyAmount.ToString();
    }

    public void CheckValidTransactionAmount(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (marketTransaction == "Sell")
            {
                if (value <= 0 || collapsedItem.attachedItemCard.itemQuantity == 0)
                {
                    transactionAmount = 0;
                    transactionAmountInput.text = "0";
                    transactionInputBackground.sprite = invalidTransaction;
                    canTransaction = false;
                }
                else if (value > collapsedItem.attachedItemCard.itemQuantity)
                {
                    if (collapsedItem.attachedItemCard.itemQuantity == 0)
                    {
                        transactionInputBackground.sprite = invalidTransaction;
                        canTransaction = false;
                    }
                    else
                    {
                        transactionInputBackground.sprite = validTransaction;
                        canTransaction = true;
                    }
                    transactionAmount = collapsedItem.attachedItemCard.itemQuantity;
                    transactionAmountInput.text = collapsedItem.attachedItemCard.itemQuantity.ToString();
                }
                else
                {
                    transactionAmount = value;
                    transactionAmountInput.text = value.ToString();
                    transactionInputBackground.sprite = validTransaction;
                    canTransaction = true;
                }

            }
            else
            {
                collapsedItem.maxBuyAmount = Mathf.FloorToInt(GameManager.UM.Balance / collapsedItem.attachedItemCard.itemPrice);
                float transactionCost = value * collapsedItem.attachedItemCard.itemPrice;
                if (transactionCost <= 0)
                {
                    transactionAmount = 0;
                    transactionAmountInput.text = "0";
                    transactionInputBackground.sprite = invalidTransaction;
                    canTransaction = false;
                }
                else if (transactionCost > GameManager.UM.Balance)
                {
                    transactionAmount = collapsedItem.maxBuyAmount;
                    transactionAmountInput.text = collapsedItem.maxBuyAmount.ToString();
                    transactionInputBackground.sprite = validTransaction;
                    canTransaction = true;
                }
                else
                {
                    transactionAmount = value;
                    transactionAmountInput.text = value.ToString();
                    transactionInputBackground.sprite = validTransaction;
                    canTransaction = true;
                }
            }
        }
        else
        {
            transactionAmount = 0;
            transactionAmountInput.text = "0";
            transactionInputBackground.sprite = invalidTransaction;
            canTransaction = false;
        }
    }

    public void OnTransactionButtonPress()
    {
        if (canTransaction)
        {
            if (holdCoroutine == null)
            {
                holdCoroutine = StartCoroutine(HandleCraftHold());
            }
            transactionInputBackground.sprite = ongoingTransaction;
        }
    }

    public void OnTransactionButtonRelease()
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            CheckValidTransactionAmount("0");
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

        //CraftCard();
    }
}