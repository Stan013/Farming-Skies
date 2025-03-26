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

    [Header("Inventory variables")]
    public GameObject craftContentArea;
    public CraftItem craftItemTemplate;
    //public ExpandedInventoryItem expandedInventoryItem;

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
        itemsInCrafting.Add(craftItem);
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

    /*    public void SetupCraftingMode()
        {
            if (cardsInCrafting.Count == 0)
            {
                for (int i = 0; i < selectionSlots.Count; i++)
                {
                    Card newCard = Instantiate(craftableCards[i], selectionSlots[i].transform);
                    newCard.GetComponent<CardInspect>().enabled = false;
                    newCard.SetCardState(Card.CardState.InCraft);
                    newCard.transform.localPosition = cardPositions[i];
                    newCard.transform.localScale = (i == 2) ? centerScale : (i == 1 || i == 3) ? sideScale : farSideScale;
                    newCard.transform.localRotation = Quaternion.identity;
                    newCard.cardCraftIndex = i;
                    cardsInCrafting.Add(newCard);
                }
            }
            selectedCard = cardsInCrafting[2];
            craftUI.SetupCraftingUI();
            CalculateMaxCraftableAmount();
            SetCardCraftAmount(0);
            CheckValidCraft();
        }*/


    /*    public void ChangeSelectedCard(int direction)
        {
            if (isTransitioning) return;
            isTransitioning = true;
            Card newCard = null;
            if (direction == -1)
            {
                if (craftableCards.Count > 5)
                {
                    Card firstCard = cardsInCrafting[0];
                    if (firstCard.cardCraftIndex + 5 >= craftableCards.Count)
                    {
                        newCard = Instantiate(craftableCards[firstCard.cardCraftIndex - 1], selectionSlots[4].transform);
                        newCard.cardCraftIndex = firstCard.cardCraftIndex - 1;
                    }
                    else
                    {
                        newCard = Instantiate(craftableCards[firstCard.cardCraftIndex + 5], selectionSlots[4].transform);
                        newCard.cardCraftIndex = firstCard.cardCraftIndex + 5;
                    }
                    newCard.GetComponent<CardInspect>().enabled = false;
                    newCard.SetCardState(Card.CardState.InCraft);
                    newCard.transform.localPosition = cardPositions[4];
                    newCard.transform.localScale = farSideScale;
                    newCard.transform.localRotation = Quaternion.identity;
                    cardsInCrafting.RemoveAt(0);
                    cardsInCrafting.Add(newCard);
                    Destroy(firstCard.gameObject);
                }
                else
                {
                    Card firstCard = cardsInCrafting[0];
                    cardsInCrafting.RemoveAt(0);
                    cardsInCrafting.Add(firstCard);
                }
            }
            else if (direction == 1)
            {
                if (craftableCards.Count > 5)
                {
                    Card lastCard = cardsInCrafting[4];
                    if (lastCard.cardCraftIndex + 1 >= craftableCards.Count)
                    {
                        newCard = Instantiate(craftableCards[lastCard.cardCraftIndex - 5], selectionSlots[0].transform);
                        newCard.cardCraftIndex = lastCard.cardCraftIndex - 5;
                    }
                    else
                    {
                        newCard = Instantiate(craftableCards[lastCard.cardCraftIndex + 1], selectionSlots[0].transform);
                        newCard.cardCraftIndex = lastCard.cardCraftIndex + 1;
                    }
                    newCard.GetComponent<CardInspect>().enabled = false;
                    newCard.SetCardState(Card.CardState.InCraft);
                    newCard.transform.localPosition = cardPositions[0];
                    newCard.transform.localScale = farSideScale;
                    newCard.transform.localRotation = Quaternion.identity;
                    cardsInCrafting.RemoveAt(cardsInCrafting.Count - 1);
                    cardsInCrafting.Insert(0, newCard);
                    Destroy(lastCard.gameObject);
                }
                else
                {
                    Card lastCard = cardsInCrafting[cardsInCrafting.Count - 1];
                    cardsInCrafting.RemoveAt(cardsInCrafting.Count - 1);
                    cardsInCrafting.Insert(0, lastCard);
                }
            }
            UpdateCraftingCards(true);
            CalculateMaxCraftableAmount();
            SetCardCraftAmount(0);
            CheckValidCraft();
            StartCoroutine(ResetTransitionAfterDelay(0.3f));
        }*/

    /*    public void UpdateCraftingCards(bool animate)
        {
            Vector3[] scales = { farSideScale, sideScale, centerScale, sideScale, farSideScale };
            for (int i = 0; i < cardsInCrafting.Count; i++)
            {
                cardsInCrafting[i].transform.SetParent(selectionSlots[i].transform);
                cardsInCrafting[i].transform.SetAsLastSibling();
                UpdateCardSlot(cardsInCrafting[i], scales[i], cardPositions[i], animate);
            }
            selectedCard = cardsInCrafting[2];
        }*/

    public void UpdateCardSlot(Card card, Vector3 scale, Vector3 position, bool animate)
    {
        if (animate)
        {
            StartCoroutine(AnimateCraftingCards(card.transform, position, scale, 0.3f));
        }
        else
        {
            card.transform.localPosition = position;
            card.transform.localScale = scale;
        }
    }

    private IEnumerator AnimateCraftingCards(Transform cardTransform, Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPos = cardTransform.localPosition;
        Vector3 startScale = cardTransform.localScale;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            cardTransform.localPosition = Vector3.Lerp(startPos, targetPosition, t);
            cardTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardTransform.localPosition = targetPosition;
        cardTransform.localScale = targetScale;
    }

/*    public void CalculateMaxCraftableAmount()
    {
        List<int> validMaxValues = new List<int>();
        if (selectedCard.cardCraftResources[0] > 0f)
        {
            validMaxValues.Add(Mathf.FloorToInt(GameManager.UM.balance / selectedCard.cardCraftResources[0]));
        }
        if (selectedCard.cardCraftResources[1] > 0)
        {
            validMaxValues.Add(GameManager.UM.water / selectedCard.cardCraftResources[1]);
        }
        if (selectedCard.cardCraftResources[2] > 0)
        {
            validMaxValues.Add(GameManager.UM.fertiliser / selectedCard.cardCraftResources[2]);
        }
        if (selectedCard.cardType != "Utility" && selectedCard.itemQuantity > 0)
        {
            validMaxValues.Add(selectedCard.itemQuantity / selectedCard.cardDropsRequired);
        }
        if (validMaxValues.Count > 0)
        {
            maxCraftableAmount = Mathf.Min(validMaxValues.ToArray());
        }
        else
        {
            maxCraftableAmount = 0;
        }
    }*/

/*    public void SetCardCraftAmount(int amount)
    {
        cardCraftAmount = Mathf.Clamp(amount, 0, maxCraftableAmount);
        craftUI.craftAmountInput.text = cardCraftAmount.ToString();
        craftUI.UpdateCostDisplay();
    }*/

    public bool CheckValidCraft()
    {
        bool valid = false;
        if (GameManager.CRM.cardCraftAmount != 0)
        {
            craftButton.GetComponent<Image>().sprite = validCraft;
            craftButton.enabled = true;
            craftButtonText.SetText("Craft card");
            valid = true;
        }
        else
        {
            craftButton.GetComponent<Image>().sprite = invalidCraft;
            craftButton.enabled = false;
            craftButtonText.SetText("Need amount");
            valid = false;
        }
        return valid;
    }
}
