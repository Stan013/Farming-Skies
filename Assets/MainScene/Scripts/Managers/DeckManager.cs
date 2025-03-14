using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour, IDataPersistence
{
    public GameObject cardsInDeckParent;
    public List<Card> cardsInDeck = new List<Card>();

    public void CheckRefillHand()
    {
        if (GameManager.HM.lastFilledSlotIndex < 9)
        {
            GameManager.HM.SetCardsInHand();
        }
    }

    public void AddCardToDeck(string cardId)
    {
        Card newCard = Instantiate(GameManager.CM.FindCardById(cardId), Vector3.zero, Quaternion.identity);
        newCard.ToggleState(Card.CardState.InDeck, Card.CardState.Hidden);
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