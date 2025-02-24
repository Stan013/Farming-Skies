using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftManager : MonoBehaviour
{
    public GameObject craftWindow;
    public int currentCardIndex = 0;
    public List<Card> craftableCards;
    public List<Card> cardsInCrafting;
    public List<GameObject> selectionSlots;
    public Card selectedCard;
    public CraftUI craftUI;

    public Button craftButton;
    public TMP_Text craftButtonText;
    public bool isCrafting;
    public int biggestResourceCost;
    public bool craftSuccess;
    public float craftSpeed;

    public int cardCraftAmount;
    public int maxCraftableAmount;
    public bool matchingCard;
    public bool craftedPotassium;
    public bool craftedPhosphorus;

    // Card scales & positions
    private readonly Vector3 centerScale = new Vector3(0.6f, 0.6f, 1f);
    private readonly Vector3 sideScale = new Vector3(0.4f, 0.4f, 1f);
    private readonly Vector3 farSideScale = new Vector3(0.30f, 0.30f, 1f);
    private readonly Vector3[] cardPositions =
    {
        new Vector3(-515f, 0f, 0f),  // Far-left card
        new Vector3(-300f, 0f, 0f),  // Left card
        Vector3.zero,                // Center card
        new Vector3(300f, 0f, 0f),   // Right card
        new Vector3(515f, 0f, 0f)    // Far-right card
    };

    private bool isTransitioning = false;

    public void SetupCraftingMode()
    {
        if (cardsInCrafting.Count == 0)
        {
            GameManager.CRM.craftUI.leftButton.GetComponent<Image>().color = Color.green;
            GameManager.CRM.craftUI.rightButton.GetComponent<Image>().color = Color.green;
            for (int i = 0; i < selectionSlots.Count; i++)
            {
                Card newCard = Instantiate(craftableCards[i]);
                newCard.GetComponent<CardInspect>().enabled = false;
                newCard.ToggleState(Card.CardState.InCraft, Card.CardState.Destroy);
                newCard.transform.SetParent(selectionSlots[i].transform);
                newCard.transform.localPosition = cardPositions[i];
                newCard.transform.localScale = (i == 2) ? centerScale : (i == 1 || i == 3) ? sideScale : farSideScale;
                newCard.transform.localRotation = Quaternion.identity;
                newCard.cardCraftIndex = i;
                cardsInCrafting.Add(newCard);
                if (GameManager.TTM.tutorial && GameManager.TTM.tutorialCount == 12)
                {
                    if (newCard.cardName == "Nitrogen Fertilizer")
                    {
                        newCard.GetComponent<Image>().color = Color.green;
                    }
                }
            }
        }
        selectedCard = cardsInCrafting[2];
        craftUI.SetupCraftingUI();
        CalculateMaxCraftableAmount();
        CheckValidCraft();
        SetCardCraftAmount(0);
    }


    public void ChangeSelectedCard(int direction)
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
                    newCard = Instantiate(craftableCards[firstCard.cardCraftIndex - 1]);
                    newCard.cardCraftIndex = firstCard.cardCraftIndex - 1;
                }
                else
                {
                    newCard = Instantiate(craftableCards[firstCard.cardCraftIndex + 5]);
                    newCard.cardCraftIndex = firstCard.cardCraftIndex + 5;
                }
                newCard.GetComponent<CardInspect>().enabled = false;
                newCard.ToggleState(Card.CardState.InCraft, Card.CardState.Destroy);
                newCard.transform.SetParent(selectionSlots[4].transform);
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
                    newCard = Instantiate(craftableCards[lastCard.cardCraftIndex - 5]);
                    newCard.cardCraftIndex = lastCard.cardCraftIndex - 5;
                }
                else
                {
                    newCard = Instantiate(craftableCards[lastCard.cardCraftIndex + 1]);
                    newCard.cardCraftIndex = lastCard.cardCraftIndex + 1;
                }
                newCard.GetComponent<CardInspect>().enabled = false;
                newCard.ToggleState(Card.CardState.InCraft, Card.CardState.Destroy);
                newCard.transform.SetParent(selectionSlots[0].transform);
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
        if (GameManager.TTM.tutorial)
        {
            if (GameManager.TTM.tutorialCount == 13)
            {
                if (newCard.cardName == "Nitrogen Fertilizer")
                {
                    newCard.GetComponent<Image>().color = Color.green;
                }
            }
            if (GameManager.TTM.tutorialCount == 15)
            {
                if (newCard.cardName == "Phosphorus Fertilizer" && !craftedPhosphorus)
                {
                    newCard.GetComponent<Image>().color = Color.green;
                }
                if (newCard.cardName == "Potassium Fertilizer" && !craftedPotassium)
                {
                    newCard.GetComponent<Image>().color = Color.green;
                }
            }
        }
        UpdateCraftingCards(true);
        CalculateMaxCraftableAmount();
        SetCardCraftAmount(0);
        CheckValidCraft();
        StartCoroutine(ResetTransitionAfterDelay(0.3f));
    }

    public void UpdateCraftingCards(bool animate)
    {
        Vector3[] scales = { farSideScale, sideScale, centerScale, sideScale, farSideScale };
        for (int i = 0; i < cardsInCrafting.Count; i++)
        {
            cardsInCrafting[i].transform.SetParent(selectionSlots[i].transform);
            cardsInCrafting[i].transform.SetAsLastSibling();
            UpdateCardSlot(cardsInCrafting[i], scales[i], cardPositions[i], animate);
        }
        selectedCard = cardsInCrafting[2];
    }

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

    private IEnumerator ResetTransitionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTransitioning = false;
    }

    public void CalculateMaxCraftableAmount()
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
            validMaxValues.Add(GameManager.UM.fertilizer / selectedCard.cardCraftResources[2]);
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
    }

    public void SetCardCraftAmount(int amount)
    {
        cardCraftAmount = Mathf.Clamp(amount, 0, maxCraftableAmount);
        craftUI.craftAmountInput.text = cardCraftAmount.ToString();
        craftUI.UpdateCostDisplay();
    }

    public bool CheckValidCraft()
    {
        bool valid = false;
        if (GameManager.CRM.cardCraftAmount != 0)
        {
            craftButton.GetComponent<Image>().color = new Color(0.24f, 0.6f, 1f);
            craftButtonText.SetText("Craft card");
            valid = true;
        }
        else
        {
            craftButton.GetComponent<Image>().color = Color.red;
            craftButtonText.SetText("Invalid amount");
            valid = false;
        }
        if(GameManager.TTM.tutorial)
        {
            CheckSelectedCard();
        }
        return valid;
    }

    public void ResetCraftCard()
    {
        foreach (var slot in GameManager.CRM.selectionSlots)
        {
            slot.SetActive(true);
        }
        craftUI.craftCardCover.SetActive(false);
        isCrafting = false;
    }

    public void HighlightCards()
    {
        foreach(Card card in cardsInCrafting)
        {
            if(card.cardName == "Phosphorus Fertilizer" || card.cardName == "Potassium Fertilizer")
            {
                card.GetComponent<Image>().color = Color.green;
            }
        }
    }

    public void CheckSelectedCard()
    {
        if (selectedCard.cardName == "Nitrogen Fertilizer" && GameManager.TTM.tutorialCount == 13)
        {
            matchingCard = true;
        }
        if(selectedCard.cardName == "Potassium Fertilizer" && GameManager.TTM.tutorialCount == 15 && !craftedPotassium)
        {
            if (GameManager.DM.cardsInDeck.Exists(card => card.cardId == selectedCard.cardId))
            {
                craftedPotassium = true;
            }
            matchingCard = true;
        }
        if (selectedCard.cardName == "Phosphorus Fertilizer" && GameManager.TTM.tutorialCount == 15 && !craftedPhosphorus)
        {
            if (GameManager.DM.cardsInDeck.Exists(card => card.cardId == selectedCard.cardId))
            {
                craftedPhosphorus = true;
            }
            matchingCard = true;
        }
        if (matchingCard)
        {
            GameManager.CRM.craftUI.leftButton.enabled = false;
            GameManager.CRM.craftUI.rightButton.enabled = false;
            GameManager.CRM.craftUI.leftButton.GetComponent<Image>().color = Color.white;
            GameManager.CRM.craftUI.rightButton.GetComponent<Image>().color = Color.white;
            GameManager.CRM.craftUI.plusButton.GetComponent<Image>().color = Color.green;
            if (GameManager.CRM.cardCraftAmount == 1)
            {
                GameManager.CRM.craftUI.minButton.enabled = false;
                GameManager.CRM.craftUI.minusButton.enabled = false;
                GameManager.CRM.craftUI.craftAmountInput.enabled = false;
                GameManager.CRM.craftButton.GetComponent<Image>().color = Color.green;
                if(GameManager.TTM.tutorialCount == 13)
                {
                    GameManager.TTM.QuestCompleted = true;
                }
            }
        }
        else
        {
            GameManager.CRM.craftUI.leftButton.enabled = true;
            GameManager.CRM.craftUI.rightButton.enabled = true;
            GameManager.CRM.craftUI.minButton.enabled = true;
            GameManager.CRM.craftUI.minusButton.enabled = true;
            GameManager.CRM.craftUI.craftAmountInput.enabled = true;
            GameManager.CRM.craftUI.plusButton.GetComponent<Image>().color = Color.white;
        }
        if(craftedPhosphorus && craftedPotassium && GameManager.TTM.tutorialCount == 15)
        {
            GameManager.TTM.QuestCompleted = true;
        }
    }
}
