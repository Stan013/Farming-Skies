using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour, IDataPersistence
{
    [Header("Deck variables")]
    public GameObject cardsInDeckParent;

    [Header("Cards list")]
    public List<Card> cardsInDeck = new List<Card>();

    public void SetStartingDeck()
    {
        foreach (Card card in GameManager.CM.starterCards)
        {
            AddCardToDeck(card.cardId);
        }
    }

    public void AddCardToDeck(string cardId)
    {
        GameManager.UM.Deck += 1;
        Card newCard = Instantiate(GameManager.CM.FindCardByID(cardId), Vector3.zero, Quaternion.identity);
        newCard.SetCardState(Card.CardState.InDeck);
    }

    public Card FindCardInDeckByID(string cardId)
    {
        return cardsInDeck.Find(card => card.cardId == cardId);
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