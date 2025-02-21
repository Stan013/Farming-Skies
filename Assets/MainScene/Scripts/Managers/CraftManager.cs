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

    public void UpdateCraftingItems(bool animate)
    {
        int[] indices = new int[5];
        for (int i = -2; i <= 2; i++)
        {
            indices[i + 2] = (currentCardIndex + i + craftableCards.Count) % craftableCards.Count;
        }

        Vector3[] scales = { farSideScale, sideScale, centerScale, sideScale, farSideScale };
        selectedCard = craftableCards[currentCardIndex];

        for (int i = 0; i < 5; i++)
        {
            AssignCardToSlot(selectionSlots[i], craftableCards[indices[i]], scales[i], cardPositions[i], animate);
        }
    }

    public void ChangeSelectedCard(int direction)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        currentCardIndex = (currentCardIndex + direction + craftableCards.Count) % craftableCards.Count;

        UpdateCraftingItems(true);
        CalculateMaxCraftableAmount();
        StartCoroutine(ResetTransitionAfterDelay(0.3f));
    }

    private IEnumerator ResetTransitionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTransitioning = false;
    }

    public void AssignCardToSlot(GameObject slot, Card card, Vector3 scale, Vector3 position, bool animate)
    {
        if (slot == null || card == null)
        {
            Debug.LogWarning("CraftManager: Invalid slot or card.");
            return;
        }

        if (slot.transform.childCount > 0)
            Destroy(slot.transform.GetChild(0).gameObject);

        Card newCard = Instantiate(card, slot.transform);
        newCard.GetComponent<CardInspect>().enabled = false;
        newCard.ToggleState(Card.CardState.InCraft, Card.CardState.Destroy);
        newCard.transform.localScale = animate ? sideScale : scale;
        newCard.transform.localPosition = animate ? new Vector3(position.x + (position.x > 0 ? 100f : -100f), position.y, position.z) : position;
        
        if (GameManager.TTM.tutorial && GameManager.TTM.tutorialCount == 12)
        {
            CheckSelectedCard(newCard);
        }

        if (animate)
            StartCoroutine(AnimateCraftingCards(newCard.transform, position, scale, 0.3f));
        else
        {
            newCard.transform.localPosition = position;
            newCard.transform.localScale = scale;
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

    public void CheckSelectedCard(Card card)
    {
        if (card.cardName == "Nitrogen Fertilizer" || card.cardName == "Phosphorus Fertilizer" || card.cardName == "Potassium Fertilizer")
        {
            card.GetComponent<Image>().color = Color.green;
        }
    }

    public bool CheckValidCraft()
    {
        if (GameManager.CRM.cardCraftAmount != 0)
        {
            craftButton.GetComponent<Image>().color = new Color(0.24f, 0.6f, 1f);
            craftButtonText.SetText("Craft card");
            return true;
        }
        else
        {
            craftButton.GetComponent<Image>().color = Color.red;
            craftButtonText.SetText("Invalid amount");
            return false;
        }
    }

    public void ResetCraftCard()
    {
        foreach (var slot in GameManager.CRM.selectionSlots)
        {
            slot.SetActive(true);
        }
        selectedCard.cardCraftResources[0] = craftUI.coinCost;
        selectedCard.cardCraftResources[1] = craftUI.waterCost;
        selectedCard.cardCraftResources[2] = craftUI.fertilizerCost;
        craftUI.craftCardCover.SetActive(false);
        isCrafting = false;
    }
}
