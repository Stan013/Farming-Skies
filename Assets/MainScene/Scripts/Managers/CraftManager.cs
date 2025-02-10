using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public GameObject craftWindow;
    public int currentCardIndex = 0;
    public List<Card> craftableCards;
    public List<GameObject> selectionSlots;
    public Card startCraftCard;

    public void UpdateCraftingItems()
    {
        int leftIndex = (currentCardIndex - 1 + craftableCards.Count) % craftableCards.Count;
        int rightIndex = (currentCardIndex + 1) % craftableCards.Count;

        AssignCardToSlot(selectionSlots[0], craftableCards[leftIndex], 0.5f);
        AssignCardToSlot(selectionSlots[1], craftableCards[currentCardIndex], 1f);
        AssignCardToSlot(selectionSlots[2], craftableCards[rightIndex], 0.5f);
    }

    public void SelectCraftingCard(int index)
    {
        if (index >= 0 && index < craftableCards.Count)
        {
            currentCardIndex = index;
            UpdateCraftingItems();
        }
    }

    public void ChangeCraftingCard(int changeIndex)
    {
        currentCardIndex = (currentCardIndex + changeIndex + craftableCards.Count) % craftableCards.Count;
        UpdateCraftingItems();
    }

    private void AssignCardToSlot(GameObject slot, Card card, float scale)
    {
        if (slot.transform.childCount > 0)
            Destroy(slot.transform.GetChild(0).gameObject);

        Card newCard = Instantiate(card, slot.transform);
        newCard.transform.localScale = Vector3.one * scale;
    }
}
