using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class DeckManager : MonoBehaviour, IDataPersistence
{
    public GameObject cardsInDeckParent;
    public List<Card> cardsInDeck = new List<Card>();

    public void AddCardToDeck(string cardId)
    {
        Card newCard = Instantiate(GameManager.CM.FindCardById(cardId), Vector3.zero, Quaternion.identity);
        newCard.ToggleState(Card.CardState.InDeck, Card.CardState.Destroy);
    }

    public void CheckSlotDuplicate()
    {
        foreach(CardSlot cardSlot in GameManager.HM.handSlots)
        {
            if (cardSlot.transform.childCount == 2)
            {
                Transform secondChild = cardSlot.transform.GetChild(1);
                Card card = secondChild.GetComponent<Card>();
                if (card != null)
                {
                    GameManager.HM.MoveCardsInHand(card);
                    Debug.LogWarning(cardSlot.index + "cardslot has duplicate card: " + card);
                }
                else
                {
                    Debug.LogWarning("The second child does not have a Card component!");
                }
            }
        }
    }

    public void LoadData(GameData data)
    {
        cardsInDeck.Clear();
        foreach (string cardID in data.cardsInDeck)
        {
            AddCardToDeck(cardID);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.cardsInDeck.Clear();
        foreach (Card card in cardsInDeck)
        {
            data.cardsInDeck.Add(card.cardId);
        }
    }
}