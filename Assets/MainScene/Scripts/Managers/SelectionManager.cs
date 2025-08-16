using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject pickSlotParent;
    public CardSlot cardSlotTemplate;
    public List<CardSlot> pickSlots;

    public void SetupCardSelection()
    {
        foreach (Card card in GameManager.CM.unlockedCards)
        {
            CardSlot newSlot = Instantiate(cardSlotTemplate, new Vector3(-450f + (pickSlots.Count * 450f), -75f, 0f), Quaternion.identity, pickSlotParent.transform);
            newSlot.transform.localScale = Vector3.one;
            pickSlots.Add(newSlot);
            Card randomCard = Instantiate(card, Vector3.zero, Quaternion.identity);
            randomCard.SetCardState(Card.CardState.InChoosing);
            newSlot.AddCardToSlot(pickSlots.Count, randomCard);
        }
    }
}
