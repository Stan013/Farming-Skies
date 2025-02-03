using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    public int index { get; set; }
    public Card cardInSlot { get; set; }

    public void AddCardToSlot(int slotIndex, Card card)
    {
        card.transform.SetParent(this.transform);
        card.transform.localPosition = Vector3.zero;
        card.transform.localRotation = Quaternion.identity;
        card.transform.localScale = Vector3.one;
        index = slotIndex;
        cardInSlot = card;
        CheckSlotDuplicate();
    }

    public void CheckSlotDuplicate()
    {
        if (this.transform.childCount == 2)
        {
            Transform secondChild = this.transform.GetChild(1);
            Card card = secondChild.GetComponent<Card>();
            if (card != null)
            {
                GameManager.HM.MoveCardsInHand(card);
                Debug.LogWarning(this.index + "cardslot has duplicate card: " + card);
            }
            else
            {
                Debug.LogWarning("The second child does not have a Card component!");
            }
        }
    }
}
