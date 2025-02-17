using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    public int index;
    public Card cardInSlot;

    public void AddCardToSlot(int slotIndex, Card card)
    {
        card.transform.SetParent(this.transform);
        card.transform.localPosition = Vector3.zero;
        card.transform.localRotation = Quaternion.identity;
        card.transform.localScale = Vector3.one;
        index = slotIndex;
        cardInSlot = card;
    }
}
