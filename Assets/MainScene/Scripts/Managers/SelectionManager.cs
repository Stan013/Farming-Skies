using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject pickSlotParent;
    public List<CardSlot> pickSlots;
    public List<Card> pickCards;

    public void SetupCardSelection()
    {
        foreach (Card card in pickCards)
        {
            CardSlot newSlot = Instantiate(GameManager.HM.cardSlotPrefab, new Vector3(-600f + (pickSlots.Count * 600f), -75f, 0f), Quaternion.identity, pickSlotParent.transform);
            newSlot.transform.localScale = Vector3.one;
            pickSlots.Add(newSlot);
            Card randomCard = Instantiate(card, Vector3.zero, Quaternion.identity);
            randomCard.SetCardState(Card.CardState.InChoosing);
            newSlot.AddCardToSlot(pickSlots.Count, randomCard);
        }
    }
}
