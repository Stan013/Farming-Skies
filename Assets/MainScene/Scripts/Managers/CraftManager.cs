using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour
{
    public GameObject craftWindow;
    public int currentCardIndex = 0;
    public List<Card> craftableCards;
    public List<GameObject> selectionSlots;

    // Card scales & positions
    private readonly Vector3 centerScale = new Vector3(0.6f, 0.6f, 1f);
    private readonly Vector3 sideScale = new Vector3(0.45f, 0.45f, 1f);
    private readonly Vector3[] cardPositions =
    {
        new Vector3(-325f, 0f, 0f),  // Left card
        Vector3.zero,   // Center card
        new Vector3(325f, 0f, 0f)    // Right card
    };

    private bool isTransitioning = false;

    public void UpdateCraftingItems(bool animate)
    {
        if (craftableCards == null || craftableCards.Count == 0 || selectionSlots == null || selectionSlots.Count < 3)
        {
            Debug.LogWarning("CraftManager: Missing card data or slots.");
            return;
        }

        int leftIndex = (currentCardIndex - 1 + craftableCards.Count) % craftableCards.Count;
        int rightIndex = (currentCardIndex + 1) % craftableCards.Count;

        int[] indices = { leftIndex, currentCardIndex, rightIndex };
        Vector3[] scales = { sideScale, centerScale, sideScale };

        for (int i = 0; i < 3; i++)
        {
            AssignCardToSlot(selectionSlots[i], craftableCards[indices[i]], scales[i], cardPositions[i], animate);
        }
    }

    public void ChangeCraftingCard(int direction)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        currentCardIndex = (currentCardIndex + direction + craftableCards.Count) % craftableCards.Count;

        UpdateCraftingItems(true);
        StartCoroutine(ResetTransitionAfterDelay(0.3f));
    }

    private IEnumerator ResetTransitionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTransitioning = false;
    }

    private void AssignCardToSlot(GameObject slot, Card card, Vector3 scale, Vector3 position, bool animate)
    {
        if (slot == null || card == null)
        {
            Debug.LogWarning("CraftManager: Invalid slot or card.");
            return;
        }

        // Destroy existing card in slot
        if (slot.transform.childCount > 0)
            Destroy(slot.transform.GetChild(0).gameObject);

        // Instantiate the new card
        Card newCard = Instantiate(card, slot.transform);
        newCard.ToggleState(Card.CardState.InCraft, Card.CardState.Destroy);
        newCard.transform.localScale = animate ? sideScale : scale;
        newCard.transform.localPosition = animate ? new Vector3(position.x + (position.x > 0 ? 100f : -100f), position.y, position.z) : position;

        if (animate)
            StartCoroutine(AnimateCard(newCard.transform, position, scale, 0.3f));
        else
        {
            newCard.transform.localPosition = position;
            newCard.transform.localScale = scale;
        }
    }

    private IEnumerator AnimateCard(Transform cardTransform, Vector3 targetPosition, Vector3 targetScale, float duration)
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
}
